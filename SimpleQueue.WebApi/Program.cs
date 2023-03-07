using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SimpleQueue.Data;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.Services;
using SimpleQueue.WebApi.AutoMapper;
using System.Reflection;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Extensions.AspNetCore.Configuration.Secrets;

var builder = WebApplication.CreateBuilder(args);

string kvURL = builder.Configuration.GetValue<string>("KeyVaultConfig:KVUrl");
string tenantId = builder.Configuration.GetValue<string>("KeyVaultConfig:TenantId");
string clientKeyVaultId = builder.Configuration.GetValue<string>("KeyVaultConfig:ClientId");
string clientKeyVaultSecret = builder.Configuration.GetValue<string>("KeyVaultConfig:ClientSecretId");

var credentials = new ClientSecretCredential(tenantId, clientKeyVaultId, clientKeyVaultSecret);
var clientAzure = new SecretClient(new Uri(kvURL), credentials);

builder.Configuration.AddAzureKeyVault(clientAzure, new AzureKeyVaultConfigurationOptions());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(config =>
    {
        string authority = "https://localhost:7210";
        string validIssuer = "https://localhost:7210";
        string validAudience = "webapi";

        config.Authority = authority;
        config.Audience = validAudience;

        config.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidAudience = validAudience,
            ValidIssuer = validIssuer,

            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "SimpleQueue.WebApi", 
        Version = "v1",
        Description = "An API to perform operations with database",
        Contact = new OpenApiContact
        {
            Name = "Petro Pavliuk",
            Email = "p4tro.pavlyk@gmail.com"
        }
    });
});

builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<IQueueService, QueueService>();
builder.Services.AddScoped<IUserInQueueService, UserInQueueService>();

builder.Services.AddAutoMapper(typeof(MappingUserProfile));
builder.Services.AddAutoMapper(typeof(MappingQueueProfile));

var corsName = "WebUI";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsName,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7253");
                      });
});

builder.Services.AddDbContext<SimpleQueueDBContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("mySqlConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(option =>
    {
        option.SwaggerEndpoint("/swagger/v1/swagger.json", "SimpleQueue.WebApi");
    });
}

app.UseRouting();

app.UseCors(corsName);

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
