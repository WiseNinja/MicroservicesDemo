using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapsRepositoryService.Core.DB.Queries;

namespace MapsRepositoryService.Infrastructure.MinIO.Queries
{
    internal class GetAllMapsQuery : IGetAllMapsQuery
    {
        public Task<List<string>> GetAllMapsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
