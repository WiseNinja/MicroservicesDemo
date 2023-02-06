using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapsRepositoryService.Core.DB.Queries
{
    public interface IGetMissionMapDataQuery
    {
        Task<string> GetMissionMapDataByNameAsync(string mapName);
    }
}
