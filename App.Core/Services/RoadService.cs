using App.Core.Domain.Models;
using App.Core.Domain.RepositoryContracts;
using App.Core.DTO_s.RoadDTO_s;
using App.Core.DTOs.RoadDTO_s;
using App.Core.Exceptions.RoadsExceptions;
using App.Core.ExtentionMethods;
using App.Core.Filters;
using App.Core.ServicesContract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Services
{
    [TypeFilter(typeof(GlobalLoggerFilter))]
    public class RoadService : IRoadService
    {
        private readonly IRoadRepositoryContract  _roadRepo;
        public RoadService(IRoadRepositoryContract repo)
        {
            _roadRepo = repo;
        }

        public async Task<ReadRoadDTO> AddRoad(InputRoadDTO addedRoad)
        {
            Road r = await _roadRepo.getRoadByName(addedRoad.Name);
            if(r != null)
            {
                throw new DuplicatedRoadFoundException($"there is a road with this name: {addedRoad.Name} in our system");
            }
            Road road = new Road()
            {
                Name = addedRoad.Name,
                Description = addedRoad.Description,
                LengthInKm = addedRoad.LengthInKm,
                RoadImageUrl = addedRoad.RoadImageUrl,
                regionId = addedRoad.regionId,
                //difficultyID = addedRoad.difficultyID,
                difficultyID = RoadExtentionMethod.GetDifficultyId(addedRoad.difficultyType)
            };
            Road returnedFromAdd = await _roadRepo.AddRoad(road);
            //returndeFromAdd.
            if (returnedFromAdd == null) {
                throw new ErrorWhileAddException($"error when add this road to db {addedRoad}");
            }
            
            return returnedFromAdd.toReadRoadDTO();
        }

        public async Task<int> DeleteRoad(Guid id)
        {   
            Road DeletedRoad = await _roadRepo.getRoadByID(id);
            if (DeletedRoad == null) {
                throw new NoRoadFoundWithThisIdException($"no road found with this id : {id}");
            }
            int deleted = await _roadRepo.DeleteRoad(id);
            if(deleted == 0)
            {
                throw new CannotDeleteRoadException($"can not delete this Road with this id : {id}");
            }
            return deleted;
        }

        public async Task<List<ReadRoadDTO>> GetAllRoads(string? DifficultyLevel = null, string? RoadLengthOrder=null, string? SearchByName = null)
        {
            List<Road> roads = await _roadRepo.GetRoads(DifficultyLevel,RoadLengthOrder,SearchByName);
            if(roads == null || roads.Count() == 0)
            {
                throw new NoRoadsFoundException("There is no region in your Db");
            }

            List<ReadRoadDTO> roadsDTO= roads.Select(r=> r.toReadRoadDTO()).ToList();
            return roadsDTO;
        }


        public async Task<ReadRoadDTO> GetRoadById(Guid id)
        {
            Road road = await _roadRepo.getRoadByID(id);
            if(road == null)
            {
                throw new NoRoadFoundException($"there is no road found with this id : {id}");
            }
            return road.toReadRoadDTO();
        }

        public async Task<ReadRoadDTO> UpdateRoad(Guid id, InputRoadDTO updatedRoad)
        {
            Road r = await _roadRepo.getRoadByID(id);
            if (r == null) {
                throw new RoadNotFoundException($"No Road found with this id {id}");
            }
            Road tobeUpdated = new Road()
            {
                Name        = updatedRoad.Name,
                Description = updatedRoad.Description,
                LengthInKm  = updatedRoad.LengthInKm,
                RoadImageUrl= updatedRoad.RoadImageUrl,
                regionId    = updatedRoad.regionId,
                difficultyID= RoadExtentionMethod.GetDifficultyId(updatedRoad.difficultyType)
            };
            Road ConfirmationUpdate = await _roadRepo.UpdateRoad(id, tobeUpdated);
            if (ConfirmationUpdate == null)
            {
                throw new ErrorWhileAddException($"can not apply this updated in to database {updatedRoad}");
            }
            return ConfirmationUpdate.toReadRoadDTO();
        }
    }
}
// setx JWT_KEY "your-very-secret-jwt-key"
