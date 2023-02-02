using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapsRepositoryService.Core.DB.Queries;
using MapsRepositoryService.Infrastructure.MinIO.Helpers;

namespace MapsRepositoryService.Infrastructure.MinIO.Queries
{
    public class GetMissionMapNameQuery : IGetMissionMapNameQuery
    {
        private readonly FileOperationsHelper _fileOperationsHelper;

        public GetMissionMapNameQuery(FileOperationsHelper fileOperationsHelper)
        {
            _fileOperationsHelper = fileOperationsHelper;
        }
        public async Task<string> GetMissionMapNameAsync()
        {
           List<string> missionMapNameInList = await _fileOperationsHelper.GetAllFileNamesFromBucketAsync("missionmap-bucket");
           string missionMapName = missionMapNameInList.Any() ? missionMapNameInList[0] : string.Empty;
           return missionMapName;
        }
    }
}
