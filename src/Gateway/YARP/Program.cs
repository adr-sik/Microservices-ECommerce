using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Shared.Logging.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using Yarp.ReverseProxy.Transforms;

namespace YARP
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.AddObservability();
            builder.Services.AddObservability(builder.Configuration, builder.Environment.ApplicationName);

            builder.Services.AddOpenApi();
            builder.Services.AddOpenApi("catalog");
            builder.Services.AddOpenApi("user");
            builder.Services.AddOpenApi("inventory");
            builder.Services.AddOpenApi("ordering");

            builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
                .AddTransforms(builderContext =>
                {
                    builderContext.AddRequestTransform(async transformContext =>
                    {
                        var token = await transformContext.HttpContext.GetTokenAsync("access_token");

                        if (!string.IsNullOrEmpty(token))
                        {
                            transformContext.ProxyRequest.Headers.Authorization =
                                new AuthenticationHeaderValue("Bearer", token);
                        }
                    });
                });

            // --- AUTHENTICATION SETUP START ---

            RsaSecurityKey signingKey;

            if (builder.Environment.IsDevelopment())
            {
                var rsa = RSA.Create();
                rsa.ImportFromPem(builder.Configuration["Jwt:RsaPublicKey"]!);
                signingKey = new RsaSecurityKey(rsa);
            }
            else
            {
                var keyClient = new KeyClient(
                    new Uri(builder.Configuration["KeyVault:Url"]!),
                    new DefaultAzureCredential());

                KeyVaultKey vaultKey = await keyClient.GetKeyAsync("jwt-signing-key");
                var rsa = vaultKey.Key.ToRSA(includePrivateParameters: false);
                signingKey = new RsaSecurityKey(rsa)
                {
                    KeyId = $"jwt-signing-key/{vaultKey.Properties.Version}"
                };
            }

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey,
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        ValidateLifetime = true,
                        RoleClaimType = "role",
                        NameClaimType = "username"
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Cookies["access_token"];
                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            // --- AUTHENTICATION SETUP END ---

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AuthenticatedUser", policy => policy.RequireAuthenticatedUser());
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference("/scalar/v1", options =>
                {
                    options.WithTitle("Master Gateway Docs");
                });

                app.MapGet("/", () => Results.Redirect("/scalar/v1")).ExcludeFromDescription();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapReverseProxy();

            app.Run();
        }
    }
}
