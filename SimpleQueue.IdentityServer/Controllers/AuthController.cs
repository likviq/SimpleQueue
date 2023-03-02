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
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IIdentityServerInteractionService interactionService,
            IMapper mapper,
            IUserService userService,
            ILogger<AuthController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _interactionService = interactionService;
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("Sign out the current user");

            var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);
            
            if (logoutRequest.PostLogoutRedirectUri == null)
            {
                _logger.LogInformation("PostLogoutRedirectUri from logout request is null");
                return RedirectToAction("Index", "Home");
            }
            _logger.LogInformation($"logout request from client - {logoutRequest.ClientId}");

            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var externalProviders = await _signInManager.GetExternalAuthenticationSchemesAsync();
            _logger.LogInformation($"Method {nameof(_signInManager.GetExternalAuthenticationSchemesAsync)} executed without error");

            if (externalProviders == null)
            {
                _logger.LogWarning($"No advanced authentication method was found " +
                    $"from {nameof(_signInManager.GetExternalAuthenticationSchemesAsync)} method");
            }
            
            return View(new LoginViewModel 
            { 
                ReturnUrl = returnUrl, 
                ExternalProviders = externalProviders 
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Model {nameof(LoginViewModel)} in {nameof(Login)} is invalid");
                return View(vm);
            }

            var result = await _signInManager
                .PasswordSignInAsync(vm.Username, vm.Password, false, false);

            if (result.IsLockedOut)
            {
                _logger.LogError("User sign in went wrong:", result.ToString());
                return Redirect("/");
            }

            _logger.LogInformation($"{vm.Username} successfully logged in");
            return Redirect(vm.ReturnUrl);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            _logger.LogInformation($"Creating a {nameof(RegisterViewModel)} with such a prepared returnurl - {returnUrl}");
            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Model {nameof(RegisterViewModel)} in {nameof(Register)} is invalid");
                return View(vm);
            }

            var user = new IdentityUser(vm.Username);
            _logger.LogInformation($"Create {nameof(IdentityUser)} object with username - {vm.Username}");
            
            var result = await _userManager.CreateAsync(user, vm.Password);

            if (!result.Succeeded)
            {
                _logger.LogWarning($"Cannot create {user} due to unexpected error. Result - {result}");
                return Redirect("/");
            }

            try
            {
                vm.Id = new Guid(user.Id);            
                var userEntity = _mapper.Map<User>(vm);
                _logger.LogInformation($"{nameof(RegisterViewModel)} object has been converted to an {nameof(userEntity)} object");

                await _userService.RegisterUserAsync(userEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot register user due to unexpected error - {ex.Message}");
                await _userManager.DeleteAsync(user);

                _logger.LogInformation($"Delete {nameof(IdentityUser)} with username - {vm.Username} from Identity database");
                return BadRequest();
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            _logger.LogInformation($"Logged in a user with this username - {user.UserName}");
            return Redirect(vm.ReturnUrl);
        }

        public IActionResult ExternalLogin(string returnUrl, string provider)
        {
            var redirectUri = Url.Action(nameof(ExternalLoginCallback), "Auth", new { returnUrl });
            _logger.LogInformation($"Create redirect URI - {redirectUri} from return URL - {returnUrl}");

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUri);
            _logger.LogInformation($"Configure external authentication - {provider}");

            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                _logger.LogInformation("There is no data about external login");
                return RedirectToAction(nameof(Login));
            }
            _logger.LogInformation($"Received information from - {info.ProviderDisplayName}");

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

            if (result.Succeeded)
            {
                _logger.LogInformation($"Successful authorization in - {info.LoginProvider} " +
                    $"with unique identifier - {info.ProviderKey}");
                return Redirect(returnUrl);
            }

            var username = info.Principal.FindFirst(ClaimTypes.Name).Value.Replace(" ", "");
            var firstname = info.Principal.FindFirst(ClaimTypes.GivenName).Value;
            
            var surname = "";
            try 
            { 
                surname = info.Principal.FindFirst(ClaimTypes.Surname).Value; 
            }
            catch 
            {
                _logger.LogError("The Surname field does not exist or is equal to zero");
            }

            var email = info.Principal.FindFirst(ClaimTypes.Email).Value;

            _logger.LogInformation($"Generate {nameof(ExternalRegisterViewModel)} " +
                $"object with principals from - {info.ProviderDisplayName}");
            return View("ExternalRegister", new ExternalRegisterViewModel
            {
                Username = username,
                FirstName = firstname,
                Surname = surname,
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
                _logger.LogInformation("There is no data about external login");
                return RedirectToAction(nameof(Login));
            }
            _logger.LogInformation($"Received information from - {info.ProviderDisplayName}");

            var user = new IdentityUser(vm.Username);
            _logger.LogInformation($"Create {nameof(IdentityUser)} object with username - {vm.Username}");
            
            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                _logger.LogWarning($"Cannot create {nameof(user)} due to unexpected error. Result - {result}");
                return View(vm);
            }

            result = await _userManager.AddLoginAsync(user, info);

            if (!result.Succeeded)
            {
                _logger.LogWarning($"Cannot login {nameof(user)}. Result - {result}");
                return View(vm);
            }
            _logger.LogInformation($"Added Identity User for {vm.Username}");

            try
            {
                vm.Id = new Guid(user.Id);
                var userEntity = _mapper.Map<User>(vm);

                await _userService.RegisterUserAsync(userEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                
                await _userManager.DeleteAsync(user);
                _logger.LogInformation($"The user - {vm.Username} was deleted from the Identity database");

                return BadRequest();
            }
            _logger.LogInformation($"The user - {vm.Username} was created in the SimpleQueueDB database");

            await _signInManager.SignInAsync(user, false);
            _logger.LogInformation($"The user - {vm.Username} has logged in");

            return Redirect(vm.ReturnUrl);
        }
    }
}
