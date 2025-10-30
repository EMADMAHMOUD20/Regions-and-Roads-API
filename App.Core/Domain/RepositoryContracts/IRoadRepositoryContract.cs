using App.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Domain.RepositoryContracts
{
    public interface IRoadRepositoryContract
    {
        public Task<List<Road>> GetRoads(string? difficultyLevel=null,string? RoadLengthOrder=null,string? searchByName=null);
        public Task<Road> getRoadByID(Guid id);
        public Task<Road> getRoadByName(string name);
        public Task<Road> AddRoad(Road road);
        public Task<Road> UpdateRoad(Guid id, Road road);
        public Task<int> DeleteRoad(Guid id);
    }
}
