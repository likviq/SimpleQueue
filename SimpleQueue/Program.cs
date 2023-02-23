using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using NLog.Web;
using SimpleQueue.Data;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.Services;
using SimpleQueue.WebUI.Automapper;
using SimpleQueue.WebUI.Hubs;
using SimpleQueue.WebUI.Middlewares;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(config =>
{
    config.DefaultScheme = "Cookie";
    config.DefaultChallengeScheme = "oidc";
})
    .AddCookie("Cookie")
    .AddOpenIdConnect("oidc", config =>
    {
        config.Authority = "https://localhost:7210";
        config.ClientId = "client_id_mvc";
        config.ClientSecret = "client_secret_mvc";
        config.SaveTokens = true;
        config.ResponseType = "code";
        config.SignedOutCallbackPath = "/Home/Index";

        config.Scope.Add(OpenIdConnectScope.OpenId);

        config.Scope.Add("simplequeue-webapi");
    });

builder.Services.AddControllersWithViews()
    .AddViewLocalization();

// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddTransient<ExceptionHandlingException>();
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<IQueueService, QueueService>();
builder.Services.AddScoped<IUserInQueueService, UserInQueueService>();
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
builder.Services.AddAutoMapper(typeof(MappingQueueProfile));

builder.Services.AddSignalR();

builder.Services.AddDbContext<SimpleQueueDBContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("mySqlConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

const string defaultCulture = "en";
const string ukraineCultureName = "uk-UA";

var supportedCultures = new[]
{
    new CultureInfo(defaultCulture),
    new CultureInfo(ukraineCultureName)
};
builder.Services.Configure<RequestLocalizationOptions>(options => {
    options.DefaultRequestCulture = new RequestCulture(defaultCulture);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithRedirects("/Home/Error?statuscode={0}");
app.UseMiddleware<ExceptionHandlingException>();

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHub<QueueHub>("/hub/queue");

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

NLog.LogManager.Shutdown();

