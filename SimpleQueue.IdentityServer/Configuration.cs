using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Security.Claims;

namespace SimpleQueue.IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        
        public static IEnumerable<ApiResource> GetApis(WebApplicationBuilder builder) =>
            new List<ApiResource> {
                new ApiResource("ApiOne"),
                new ApiResource("ApiTwo"),
                new ApiResource()
                {
                    Name = "webapi",
                    DisplayName = "Web Api",
                    ApiSecrets = { new Secret(builder.Configuration
                        .GetValue<string>("ApiResoursesSecret:webapi").ToSha256()) },
                    Scopes = new List<string> { "simplequeue-webapi" },
                    UserClaims = { ClaimTypes.Email, ClaimTypes.NameIdentifier, ClaimTypes.Sid}
                }
            };

        public static IEnumerable<ApiScope> GetScopes() =>
            new List<ApiScope>
            {
                new ApiScope()
                {
                    Name = "simplequeue-webapi",
                    UserClaims = { ClaimTypes.Email, ClaimTypes.Sid, ClaimTypes.NameIdentifier }
                }
            };

        public static IEnumerable<Client> GetClients(WebApplicationBuilder builder) =>
            new List<Client>{
                new Client{
                    ClientId = "client_id",
                    ClientSecrets = { new Secret(builder.Configuration
                        .GetValue<string>("IdentityClientSecrets:client_id").ToSha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "ApiOne" }
                },
                new Client{
                    ClientId = "client_id_mvc",
                    ClientSecrets = { new Secret(builder.Configuration
                        .GetValue<string>("IdentityClientSecrets:client_id_mvc").ToSha256())},
                    
                    AllowedGrantTypes = GrantTypes.Code,
                    
                    RedirectUris = { "https://localhost:7253/signin-oidc"},
                    PostLogoutRedirectUris = { "https://localhost:7253/Home/Index" },
                    
                    AllowedScopes = {
                        "simplequeue-webapi",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    
                    RequirePkce = true,
                    RequireConsent = false,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowOfflineAccess = true
                }
            };
    }
}
