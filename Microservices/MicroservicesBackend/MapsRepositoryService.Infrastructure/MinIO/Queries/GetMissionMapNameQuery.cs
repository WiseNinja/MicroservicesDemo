using MapsRepositoryService.Core.DB.Queries;
using Microsoft.Extensions.Logging;
using Minio.DataModel;
using Minio;

namespace MapsRepositoryService.Infrastructure.MinIO.Queries
{
    internal class GetMissionMapNameQuery : IGetMissionMapNameQuery
    {
        private readonly MinioClient _minioClient;
        private readonly ILogger<GetMissionMapNameQuery> _logger;

        public GetMissionMapNameQuery(MinioClient minioClient, ILogger<GetMissionMapNameQuery> logger)
        {
            _minioClient = minioClient;
            _logger = logger;
        }
        public async Task<string> GetMissionMapNameAsync()
        {
            List<string> missionMapNameInList = new List<string>();
            try
            {
                TaskCompletionSource<object> doneFetchingMissionMapName = new TaskCompletionSource<object>();
                var listArgs = new ListObjectsArgs()
                    .WithBucket("missionmap-bucket");

                IObservable<Item>? observable = _minioClient.ListObjectsAsync(listArgs);
                observable.Subscribe(
                    item => missionMapNameInList.Add(item.Key),
                    ex => _logger.LogError(
                        $"A MinIO exception occurred during fetching of all file names from missionmap-bucket, details: {ex}"),
                    () => { doneFetchingMissionMapName.SetResult(new object()); });
                await doneFetchingMissionMapName.Task;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while trying to get the Mission Map name, details: {ex}");
            }

            var missionMapName = missionMapNameInList.Any() ? missionMapNameInList[0] : string.Empty;
            return missionMapName;
        }
    }
}
