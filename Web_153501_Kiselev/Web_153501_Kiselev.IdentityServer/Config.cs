using Duende.IdentityServer.Models;

namespace Web_153501_Kiselev.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
            new ApiScope("api.read"),
            new ApiScope("api.write"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
            // m2m client credentials flow client
            new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                AllowedScopes = { "api.read", "api.write" }
            },

            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "interactive",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:7001/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:7001/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:7001/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "api.read", "api.write" }
            },
			new Client
			{
				ClientId = "blazorApp",
				AllowedGrantTypes = GrantTypes.Code,
				RequireClientSecret = false,
				RedirectUris = { "https://localhost:7222/authentication/login-callback" },
				PostLogoutRedirectUris = { "https://localhost:7222/authentication/logout-callback" },
				AllowOfflineAccess = true,
				AllowedScopes = { "openid", "profile", "api.read","api.write" }
			},
			};
    }
}