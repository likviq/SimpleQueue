using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
    })
    .AddCookie("Cookie")
    .AddOpenIdConnect("oidc", config =>
    {
        config.Authority = "https://localhost:7210";
        config.ClientId = "client_id_api";
        config.ClientSecret = "client_secret_api";
        config.SaveTokens = true;
        config.ResponseType = "code";
    });

builder.Services.AddControllers();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
