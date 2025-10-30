
using App.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Domain.RepositoryContracts
{
    public interface IRegionRepositoryContract
    {
        public Task<List<Region>> GetRegions();
        public Task<Region> getRegionByID(Guid id);
        public Task<Region> getRegionByName(string name);
        public Task<Region> AddRegion(Region region);
        public Task<Region> UpdateRegion(Guid id,Region region);
        public Task<int> DeleteRegion(Guid id);

    }
}
