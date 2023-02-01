using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapsRepositoryService.Core.DB.Commands;
using MapsRepositoryService.Core.DTOs;
using MapsRepositoryService.Infrastructure.MinIO.Helpers;

namespace MapsRepositoryService.Infrastructure.MinIO.Commands
{
    public class SetMissionMapCommand : ISetMissionMapCommand
    {
        private readonly FileOperationsHelper _fileOperationsHelper;

        public SetMissionMapCommand(FileOperationsHelper fileOperationsHelper)
        {
            _fileOperationsHelper = fileOperationsHelper;
        }

        public async Task SetMainMissionMapAsync(string mapName)
        {
            await _fileOperationsHelper.CopyFileToBucketAsync(mapName, "maps-bucket", "missionmap-bucket");
        }
    }
}
