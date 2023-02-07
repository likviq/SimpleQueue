using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SimpleQueue.IdentityServer;
using SimpleQueue.IdentityServer.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(config =>
{
    config.UseInMemoryDatabase("Memory");
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(config =>
{
    config.Password.RequiredLength = 4;
    config.Password.RequireDigit = false;
    config.Password.RequireNonAlphanumeric = false;
    config.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "Identity.Cookie";
    config.LoginPath = "/Auth/Login";
    config.LogoutPath = "/Auth/Logout";
});

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<IdentityUser>()
    .AddInMemoryApiResources(Configuration.GetApis())
    .AddInMemoryIdentityResources(Configuration.GetIdentityResources())
    .AddInMemoryClients(Configuration.GetClients())
    .AddDeveloperSigningCredential();

builder.Services.AddAuthentication().AddFacebook(config =>
{
    config.AppId = "541927581259891";
    config.AppSecret = "6f5406d63d0f008af028bdf922121a2c";
})
    .AddGoogle(config =>
{
    config.ClientId = "1070453281257-ad8p7qpg890aqk9ngbl98qptet9ctf8f.apps.googleusercontent.com";
    config.ClientSecret = "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";
});

builder.Services.AddControllersWithViews();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider
        .GetRequiredService<UserManager<IdentityUser>>();

    var user = new IdentityUser("bob");
    userManager.CreateAsync(user, "password").GetAwaiter().GetResult();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseIdentityServer();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();
