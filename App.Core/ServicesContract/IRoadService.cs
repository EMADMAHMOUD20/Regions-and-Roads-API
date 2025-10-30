using App.Core.DTO_s.RoadDTO_s;
using App.Core.DTOs.RoadDTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.ServicesContract
{
    public interface IRoadService
    {
        public Task<List<ReadRoadDTO>> GetAllRoads(string? DifficultyLevel = null,string? RoadLengthOrder=null,string? SearchByName= null);
        public Task<ReadRoadDTO> GetRoadById(Guid id);
        public Task<ReadRoadDTO> AddRoad(InputRoadDTO addedRoad);
        public Task<ReadRoadDTO> UpdateRoad(Guid id,InputRoadDTO updatedRoad);
        public Task<int> DeleteRoad(Guid id);
    }
}

/*
 
 
 public Task<List<ReadRegionDTO>> GetAllRegions();
        public Task<ReadRegionDTO> GetRegionById(Guid id);
        public Task<ResultFromAddRegion> AddRegion(InputRegionDTO regionDTO);
        public Task<ReadRegionDTO> updateRegion(Guid id,InputRegionDTO r);
        public Task<int> DeleteRegion(Guid id);
 
 */