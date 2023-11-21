using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using Web_153501_Kiselev.IdentityServer.Data;
using Web_153501_Kiselev.IdentityServer.Models;

namespace Web_153501_Kiselev.IdentityServer
{
    public class SeedData
    {
        public static void EnsureSeedData(WebApplication app)
        {
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.Migrate();

                var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var userRole = roleMgr.FindByNameAsync("user").Result;
                if (userRole == null)
                {
                    userRole = new IdentityRole("user");

                    var result = roleMgr.CreateAsync(userRole).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                }

                var adminRole = roleMgr.FindByNameAsync("admin").Result;
                if (adminRole == null)
                {
                    adminRole = new IdentityRole("admin");

                    var result = roleMgr.CreateAsync(adminRole).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                }

                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                var user = userMgr.FindByNameAsync("user").Result;
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = "user",
                        Email = "user@gmail.com",
                        EmailConfirmed = true,

                    };
                    var createResult = userMgr.CreateAsync(user, "Pass123$").Result;
                    if (!createResult.Succeeded)
                    {
                        throw new Exception(createResult.Errors.First().Description);
                    }

                    var roleResult = userMgr.AddToRoleAsync(user, "user").Result;
                    if (!roleResult.Succeeded)
                    {
                        throw new Exception(roleResult.Errors.First().Description);
                    }

                    createResult = userMgr.AddClaimsAsync(user, new Claim[]{
                                   new Claim(JwtClaimTypes.Name, "First User"),
                                   new Claim(JwtClaimTypes.GivenName, "First"),
                                   new Claim(JwtClaimTypes.FamilyName, "User"),
                               }).Result;
                    if (!createResult.Succeeded)
                    {
                        throw new Exception(createResult.Errors.First().Description);
                    }
                    Log.Debug("user created");
                }
                else
                {
                    Log.Debug("user already exists");
                }

                var admin = userMgr.FindByNameAsync("admin").Result;
                if (admin == null)
                {
                    admin = new ApplicationUser
                    {
                        UserName = "admin",
                        Email = "admin@gmail.com",
                        EmailConfirmed = true,

                    };
                    var createResult = userMgr.CreateAsync(admin, "Pass123$").Result;
                    if (!createResult.Succeeded)
                    {
                        throw new Exception(createResult.Errors.First().Description);
                    }

                    var roleResult = userMgr.AddToRoleAsync(admin, "admin").Result;
                    if (!roleResult.Succeeded)
                    {
                        throw new Exception(roleResult.Errors.First().Description);
                    }

                    createResult = userMgr.AddClaimsAsync(admin, new Claim[]{
                                    new Claim(JwtClaimTypes.Name, "First Admin"),
                                    new Claim(JwtClaimTypes.GivenName, "First"),
                                    new Claim(JwtClaimTypes.FamilyName, "Admin"),
                                }).Result;
                    if (!createResult.Succeeded)
                    {
                        throw new Exception(createResult.Errors.First().Description);
                    }
                    Log.Debug("admin created");
                }
                else
                {
                    Log.Debug("admin already exists");
                }
            }
        }
    }
}