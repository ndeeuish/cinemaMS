using CinemaMS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UploadsController : ControllerBase
{
    private readonly IImageService _imageService;

    public UploadsController(IImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpPost("image")]
    [Authorize]
    public async Task<IActionResult> UploadImage(IFormFile file, [FromForm] string folder, [FromForm] string? tag = null)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "No file uploaded." });
        }

        if (string.IsNullOrEmpty(folder))
        {
            return BadRequest(new { message = "Folder parameter is required." });
        }

        try
        {
            using var stream = file.OpenReadStream();
            var result = await _imageService.UploadImageAsync(stream, file.FileName, folder, tag);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpDelete("image")]
    [Authorize]
    public async Task<IActionResult> DeleteImage([FromQuery] string publicId)
    {
        if (string.IsNullOrEmpty(publicId))
        {
            return BadRequest(new { message = "Public ID is required." });
        }

        try
        {
            var result = await _imageService.DeleteImageAsync(publicId);

            if (result)
            {
                return Ok(new { message = "Image deleted successfully." });
            }

            return BadRequest(new { message = "Failed to delete image from Cloudinary." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
