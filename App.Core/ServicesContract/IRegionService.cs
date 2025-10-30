
using App.Core.DTO_s.RegionDTO_s;
using App.Core.DTOs.RegionDTO_s;

namespace App.Core.ServicesContract
{
    public interface IRegionService
    {
        public Task<List<ReadRegionDTO>> GetAllRegions();
        public Task<ReadRegionDTO> GetRegionById(Guid id);
        public Task<ResultFromAddRegion> AddRegion(InputRegionDTO regionDTO);
        public Task<ReadRegionDTO> updateRegion(Guid id,InputRegionDTO r);
        public Task<int> DeleteRegion(Guid id);

    }
}
