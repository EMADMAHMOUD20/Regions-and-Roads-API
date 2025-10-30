using App.Core.Domain;
using App.Core.Domain.Models;
using App.Core.Domain.RepositoryContracts;
using App.Core.DTO_s.RegionDTO_s;
using App.Core.DTOs.RegionDTO_s;
using App.Core.Exceptions;
using App.Core.Filters;
using App.Core.ServicesContract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleProjectWebAPIwithDIandEF.ExtentionMethods;

namespace App.Core.Services
{
    [TypeFilter(typeof(GlobalLoggerFilter))]
    public class RegionService : IRegionService
    {

        private readonly IRegionRepositoryContract _regionRepo;
        private readonly ILogger<RegionService> _logger;
       
        public RegionService(IRegionRepositoryContract regionRepo,ILogger<RegionService> logger)
        {
            _regionRepo = regionRepo;
            _logger = logger;
        
        }

        public async Task<ResultFromAddRegion> AddRegion(InputRegionDTO regionDTO)
        {
            
            Region r = await _regionRepo.getRegionByName(regionDTO.Name);
            if (r != null) {
                _logger.LogWarning($"CANNOT ADD A REGION,There is a region with this name {regionDTO.Name}");
                throw new DublicateCountryException($"there is country with this name : {regionDTO.Name}");
            }
            //convert from DTO to Domain Model 
            Region AddedRegion = new Region()
            {
                Name = regionDTO.Name,
                Code = regionDTO.Code,
                RegionImageUrl = regionDTO.RegionImageUrl,
            };
            // PROBLEM WITH AUTO MAPPER 
            //Region AddedRegion = _mapper.Map<Region>(regionDTO);
            AddedRegion =  await _regionRepo.AddRegion(AddedRegion);
            if (AddRegion == null) {
                _logger.LogWarning($"CANNOT ADD THIS REGION TO DATABASE {regionDTO}");
                throw new UnableToAddToDbException($"Unable to to add this region to db {regionDTO}");
            }
            _logger.LogInformation($"Region added Successfully with id = {AddedRegion.id}");
            // swich back to DTO to return to Controller
            return AddedRegion.ReturnedFromADD();
        }

        public async Task<int> DeleteRegion(Guid id)
        {
            _logger.LogInformation($"Searching for region with id : {id}");
            Region DeletedRegion = await _regionRepo.getRegionByID(id);
            if (DeletedRegion == null) {
                _logger.LogWarning($"ca");
                throw new RegionNotFoundException($"this region with this id {id} not found");
            }
            _logger.LogInformation($"Region Founded Successfully with this id : {id}");
            int success = await _regionRepo.DeleteRegion(id);
            if(success != 1)
            {
                throw new UnableToDeleteFromDbException($"unable to delete this region with that id {id}");
            }
            return success;

        }

        public async Task<List<ReadRegionDTO>> GetAllRegions()
        {
            List<Region> regions = await _regionRepo.GetRegions();
            if(regions.Count == 0)
            {
                return null;
            }
            List<ReadRegionDTO> returndeRegions = regions.Select(r=> r.toReadRegionDTO()).ToList();
            return returndeRegions;
        }

        public async Task<ReadRegionDTO> GetRegionById(Guid id)
        {
            Region r = await _regionRepo.getRegionByID(id);
            if (r == null) {
                throw new RegionNotFoundException($"There is no region with this id {id}");
            }
            return r.toReadRegionDTO();
        }

        public async Task<ReadRegionDTO> updateRegion(Guid id, InputRegionDTO tobeUdated)
        {

            Region r = await _regionRepo.getRegionByID(id);
            if (r == null)
            {
                throw new RegionNotFoundException($"There is no region with this id {id}");
            }
            // map from DTO to Domain model 
            Region UpdatedRegion = new Region()
            {
                Name = tobeUdated.Name,
                Code = tobeUdated.Code,
                RegionImageUrl = tobeUdated.RegionImageUrl,
            };
            //PROBLEM WITH USING AUTO MAPPER
            //Region UpdatedRegion = _mapper.Map<Region>(tobeUdated);

            Region result = await _regionRepo.UpdateRegion(id, UpdatedRegion);
            if (result == null)
            {
                throw new UnableToUpdateFromDbException($"unable to update this region with that id {id}");
            }

            return result.toReadRegionDTO();
        }

        
    }
}
