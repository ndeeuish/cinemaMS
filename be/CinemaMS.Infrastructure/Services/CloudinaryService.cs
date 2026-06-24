using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CinemaMS.Application.DTOs;
using CinemaMS.Application.Interfaces.Services;
using CinemaMS.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace CinemaMS.Infrastructure.Services;

public class CloudinaryService : IImageService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IOptions<CloudinarySettings> config)
    {
        var account = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );

        _cloudinary = new Cloudinary(account);
    }

    public async Task<ImageUploadResultDto> UploadImageAsync(Stream fileStream, string fileName, string folderName, string? tag = null)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, fileStream),
            Folder = folderName,
            UseFilename = false,
            UniqueFilename = true,
            Overwrite = false
        };

        if (!string.IsNullOrEmpty(tag))
        {
            uploadParams.Tags = tag;
        }

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error != null)
        {
            throw new Exception($"Image upload failed: {uploadResult.Error.Message}");
        }

        return new ImageUploadResultDto
        {
            Url = uploadResult.SecureUrl.ToString(),
            PublicId = uploadResult.PublicId
        };
    }

    public async Task<bool> DeleteImageAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);

        return result.Result == "ok";
    }
}
