using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Web_153501_Kiselev.IdentityServer.Models;

namespace Web_153501_Kiselev.IdentityServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AvatarController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ILogger<AvatarController> _logger;
        private readonly IWebHostEnvironment _environment;

        public AvatarController(UserManager<ApplicationUser> userManager, ILogger<AvatarController> logger, IWebHostEnvironment environment)
        {
            _environment = environment;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAvatar()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            FileExtensionContentTypeProvider contentTypeProvider = new();

            var contentType = string.Empty;

            if (user != null)
            {
                string imagePath = Path.Combine(_environment.WebRootPath, "images", $"{user.Id}.png");
                contentTypeProvider.TryGetContentType(imagePath, out contentType);
                if (System.IO.File.Exists(imagePath))
                {
                    _logger.LogDebug("User avatar was found");
                    return PhysicalFile(imagePath, contentType);
                }
            }
            _logger.LogDebug("Default image used, user avatar was not found");
            string defaultFilePath = Path.Combine(_environment.WebRootPath, "images", "default-profile-picture.png");
            contentTypeProvider.TryGetContentType(defaultFilePath, out contentType);

            return PhysicalFile(defaultFilePath, contentType);
        }
    }
}
