using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SimpleQueue.Data;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.IdentityServer;
using SimpleQueue.IdentityServer.AutoMapper;
using SimpleQueue.IdentityServer.Data;
using SimpleQueue.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var connectionConfigString = builder.Configuration.GetConnectionString("mySqlConfigConnection");

builder.Services.AddAutoMapper(typeof(MappingUserProfile));

builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddDbContext<SimpleQueueDBContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("mySqlConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddDbContext<AppDbContext>(config =>
{
    config.UseMySql(connectionConfigString, ServerVersion.AutoDetect(connectionConfigString));
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

var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<IdentityUser>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b => b.UseMySql(connectionConfigString, 
            ServerVersion.AutoDetect(connectionConfigString), 
            sql => sql.MigrationsAssembly(migrationsAssembly));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b => b.UseMySql(connectionConfigString,
            ServerVersion.AutoDetect(connectionConfigString), 
            sql => sql.MigrationsAssembly(migrationsAssembly));
    })
    //.AddInMemoryApiResources(Configuration.GetApis())
    //.AddInMemoryIdentityResources(Configuration.GetIdentityResources())
    //.AddInMemoryClients(Configuration.GetClients())
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

    scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

    var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
    context.Database.Migrate();
    
    if (!context.Clients.Any())
    {
        foreach (var client in Configuration.GetClients())
        {
            context.Clients.Add(client.ToEntity());
        }
        context.SaveChanges();
    }

    if (!context.IdentityResources.Any())
    {
        foreach (var resource in Configuration.GetIdentityResources())
        {
            context.IdentityResources.Add(resource.ToEntity());
        }
        context.SaveChanges();
    }

    if (!context.ApiResources.Any())
    {
        foreach (var resource in Configuration.GetApis())
        {
            context.ApiResources.Add(resource.ToEntity());
        }
        context.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();
