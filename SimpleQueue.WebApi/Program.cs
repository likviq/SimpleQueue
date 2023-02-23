using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SimpleQueue.Data;
using SimpleQueue.Domain.Interfaces;
using SimpleQueue.Services;
using SimpleQueue.WebApi.AutoMapper;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSwaggerGen();

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
    app.UseSwaggerUI();
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
