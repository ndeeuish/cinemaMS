namespace CinemaMS.Application.Interfaces.Services;

public interface IImageService
{
    Task<DTOs.ImageUploadResultDto> UploadImageAsync(Stream fileStream, string fileName, string folderName, string? tag = null);
    Task<bool> DeleteImageAsync(string publicId);
}
