using AutoMapper;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;
using System.Security.Claims;

namespace SimpleQueue.IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public AuthController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IIdentityServerInteractionService interactionService,
            IMapper mapper,
            IUserService userService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _interactionService = interactionService;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();

            var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);

            if (logoutRequest.PostLogoutRedirectUri == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var externalProviders = await _signInManager.GetExternalAuthenticationSchemesAsync();
            return View(new LoginViewModel 
            { 
                ReturnUrl = returnUrl, 
                ExternalProviders = externalProviders 
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            var result = await _signInManager
                .PasswordSignInAsync(vm.Username, vm.Password, false, false);

            if (result.IsLockedOut)
            {
                return Redirect("/");
            }

            return Redirect(vm.ReturnUrl);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = new IdentityUser(vm.Username);
            var result = await _userManager.CreateAsync(user, vm.Password);

            if (!result.Succeeded)
            {
                return Redirect("/");
            }

            try
            {
                vm.Id = new Guid(user.Id);
            
                var userEntity = _mapper.Map<User>(vm);

                await _userService.RegisterUser(userEntity);
            }
            catch (Exception ex)
            {
                await _userManager.DeleteAsync(user);

                return BadRequest();
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return Redirect(vm.ReturnUrl);
        }

        public IActionResult ExternalLogin(string returnUrl, string provider)
        {
            var redirectUri = Url.Action(nameof(ExternalLoginCallback), "Auth", new { returnUrl });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUri);

            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }

            var username = info.Principal.FindFirst(ClaimTypes.Name).Value.Replace(" ", "");
            var firstname = info.Principal.FindFirst(ClaimTypes.GivenName).Value;
            //var surname = info.Principal.FindFirst(ClaimTypes.Surname).Value;
            var email = info.Principal.FindFirst(ClaimTypes.Email).Value;
            return View("ExternalRegister", new ExternalRegisterViewModel
            {
                Username = username,
                FirstName = firstname,
                //Surname = surname,
                Email = email,
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> ExternalRegister(ExternalRegisterViewModel vm)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var user = new IdentityUser(vm.Username);
            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                return View(vm);
            }

            result = await _userManager.AddLoginAsync(user, info);

            if (!result.Succeeded)
            {
                return View(vm);
            }

            await _signInManager.SignInAsync(user, false);

            return Redirect(vm.ReturnUrl);
        }
    }
}
