using System.Drawing;
using MapsRepositoryService.Core.DB.Commands;
using MapsRepositoryService.Core.DTOs;
using Minio;
using System.Drawing.Imaging;
using Microsoft.Extensions.Logging;

namespace MapsRepositoryService.Infrastructure.MinIO.Commands;

internal class InsertMapCommand : IInsertMapCommand
{
    private readonly MinioClient _minioClient;
    private readonly ILogger<InsertMapCommand> _logger;

    public InsertMapCommand(MinioClient minioClient, ILogger<InsertMapCommand> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }
    public async Task<bool> InsertMapAsync(MapDto mapDto)
    {
        Stream? mapFileContentsStream;
        var mapFileData = StripFileType(mapDto.Data);
        var contentType = GetContentTypeFromBase64String(mapFileData);

        try
        {
            var mapFileBytes = Convert.FromBase64String(mapFileData);
            var mapFileContents = new StreamContent(new MemoryStream(mapFileBytes)); 
            mapFileContentsStream = await mapFileContents.ReadAsStreamAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured while generating a stream from the Map file, details: {ex}");
            return false;
        }
        try
        {
            var putObjectArgs = new PutObjectArgs()
                .WithBucket("maps-bucket")
                .WithObject(mapDto.Name)
                .WithObjectSize(mapFileContentsStream.Length)
                .WithStreamData(mapFileContentsStream)
                .WithContentType(contentType);
            await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occurred while saving the Map file to storage, details: {ex}");
            return false;
        }

        return true;
    }

    private static string StripFileType(string fileData)
    {
        if (fileData.Contains(","))
        {
            fileData = fileData.Substring(fileData.IndexOf(",", StringComparison.Ordinal) + 1);
        }

        return fileData;
    }

    private string? GetContentTypeFromBase64String(string base64String)
    {
        var imageBytes = Convert.FromBase64String(base64String);
        using (var memoryStream = new MemoryStream(imageBytes))
        {
            var image = Image.FromStream(memoryStream);
            var codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == image.RawFormat.Guid);
            return codec?.MimeType;
        }
    }
}