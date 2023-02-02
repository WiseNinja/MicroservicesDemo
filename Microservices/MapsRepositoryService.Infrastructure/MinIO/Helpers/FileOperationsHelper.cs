using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.Exceptions;
using Minio.DataModel;

namespace MapsRepositoryService.Infrastructure.MinIO.Helpers;

public class FileOperationsHelper
{
    private readonly MinioClient _minioClient;
    private readonly ILogger<FileOperationsHelper> _logger;
    public FileOperationsHelper(MinioClient minioClient, ILogger<FileOperationsHelper> logger)
    {
        _logger = logger;
        _minioClient = minioClient;
    }

    #region File Operations Methods
    public async Task UploadFileAsync(string fileName, string fileData, string bucketName)
    {
        try
        {
            await CreateBucketIfNotExists(bucketName);
            await UploadFileToBucket(fileName, fileData, bucketName);
        }
        catch (MinioException ex)
        {
            _logger.LogError($"A MinIO exception occurred during file upload, details: {ex}");
            throw ex;
        }
    }

    public async Task DeleteFileAsync(string fileName, string bucketName)
    {
        try
        {
            RemoveObjectArgs? removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName);

            await _minioClient.RemoveObjectAsync(removeObjectArgs);
        }
        catch (MinioException ex)
        {
            _logger.LogError($"A MinIO exception occurred during file deletion, details: {ex}");
            throw ex;
        }
    }

    public async Task<List<string>> GetAllFileNamesFromBucketAsync(string bucketName)
    {
        List<string> allFileNames = new List<string>();
        try
        {
            TaskCompletionSource<object> done = new TaskCompletionSource<object>();
            ListObjectsArgs? listArgs = new ListObjectsArgs()
                .WithBucket(bucketName);

            IObservable<Item>? observable = _minioClient.ListObjectsAsync(listArgs);
            observable.Subscribe(
                item => allFileNames.Add(item.Key),
                ex => _logger.LogError($"A MinIO exception occurred during fetching of all file names from bucket {bucketName}, details: {ex}"),
                () => { done.SetResult(null); });
            await done.Task;
        }
        catch (MinioException ex)
        {
            _logger.LogError($"A MinIO exception occurred during fetching of all file names from bucket {bucketName}, details: {ex}");
            throw ex;
        }

        return allFileNames;
    }

    public async Task<string> GetFileDataAsBase64(string objectName, string bucketName)
    {
        string fileData = string.Empty;

        try
        {
            GetObjectArgs? args = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName).WithCallbackStream(stream => fileData = ConvertToBase64(stream));
            await _minioClient.GetObjectAsync(args);
        }
        catch (MinioException ex)
        {
            _logger.LogError($"A MinIO exception occurred during fetching of file data for file: {objectName} from bucket: {bucketName}, details: {ex}");
            throw ex;
        }
        return fileData;
    }

    public async Task CopyFileToBucketAsync(string fileName, string sourceBucketName, string destBucketName)
    {
        try
        {
            BucketExistsArgs? beArgs = new BucketExistsArgs()
                .WithBucket(destBucketName);
            bool found = await _minioClient.BucketExistsAsync(beArgs).ConfigureAwait(false);
            if (found)
            {
                //clean up bucket as we only want to have one file at all times in destination
                List<string> filesToDelete = await GetAllFileNamesFromBucketAsync(destBucketName);
                RemoveObjectsArgs? objArgs = new RemoveObjectsArgs()
                    .WithBucket(destBucketName)
                    .WithObjects(filesToDelete);
                IObservable<DeleteError>? objectsOservable = await _minioClient.RemoveObjectsAsync(objArgs).ConfigureAwait(false);
                objectsOservable.Subscribe(
                    objDeleteError => _logger.LogInformation($"Object: {objDeleteError.Key}"),
                    ex => _logger.LogError($"OnError: {ex}"),
                    () => { _logger.LogInformation($"Removed objects in list from {destBucketName}\n"); });
            }
            else
            {
                MakeBucketArgs? mbArgs = new MakeBucketArgs()
                    .WithBucket(destBucketName);
                await _minioClient.MakeBucketAsync(mbArgs).ConfigureAwait(false);
            }
            CopySourceObjectArgs? cpSrcArgs = new CopySourceObjectArgs()
                .WithBucket(sourceBucketName)
                .WithObject(fileName);

            CopyObjectArgs? args = new CopyObjectArgs()
                .WithBucket(destBucketName)
                .WithObject(fileName)
                .WithCopyObjectSource(cpSrcArgs);

            await _minioClient.CopyObjectAsync(args);
        }
        catch (MinioException ex)
        {
            _logger.LogError($"A MinIO exception occurred during copying of file {fileName} from bucket: {sourceBucketName} to {destBucketName}, details: {ex}");
            throw ex;
        }
    }

    #endregion

    #region Private Methods
    private async Task UploadFileToBucket(string fileName, string fileData, string bucketName)
    {
        fileData = StripFileType(fileData);

        byte[] fileBytes = Convert.FromBase64String(fileData);
        StreamContent fileContents = new StreamContent(new MemoryStream(fileBytes));
        Stream fileContentsStream = await fileContents.ReadAsStreamAsync();
        string? contentType = GetContentTypeFromBase64String(fileData);

        PutObjectArgs? putObjectArgs = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName)
            .WithObjectSize(fileContentsStream.Length)
            .WithStreamData(fileContentsStream)
            .WithContentType(contentType);
        await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
    }

    private async Task CreateBucketIfNotExists(string bucketName)
    {
        BucketExistsArgs? beArgs = new BucketExistsArgs()
            .WithBucket(bucketName);
        bool found = await _minioClient.BucketExistsAsync(beArgs).ConfigureAwait(false);
        if (!found)
        {
            MakeBucketArgs? mbArgs = new MakeBucketArgs()
                .WithBucket(bucketName);
            await _minioClient.MakeBucketAsync(mbArgs).ConfigureAwait(false);
        }
    }

    private static string StripFileType(string fileData)
    {
        if (fileData.Contains(","))
        {
            fileData = fileData.Substring(fileData.IndexOf(",") + 1);
        }

        return fileData;
    }

    private string? GetContentTypeFromBase64String(string base64String)
    {
        byte[] imageBytes = Convert.FromBase64String(base64String);
        using (MemoryStream memoryStream = new MemoryStream(imageBytes))
        {
            Image? image = Image.FromStream(memoryStream);
            ImageCodecInfo? codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == image.RawFormat.Guid);
            return codec?.MimeType;
        }
    }

    private string ConvertToBase64(Stream stream)
    {
        using MemoryStream memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        byte[] bytes = memoryStream.ToArray();

        string base64 = Convert.ToBase64String(bytes);
        return base64;
    }

    #endregion
}