namespace MapsRepositoryService.Infrastructure;

internal static class StreamExtensions
{
    public static string ConvertToBase64(this Stream stream)
    {
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        var bytes = memoryStream.ToArray();

        var base64 = Convert.ToBase64String(bytes);
        return base64;
    }
}