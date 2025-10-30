using App.Core.Domain.Models;
using App.Core.Domain.RepositoryContracts;
using App.Core.Filters;
using App.Infrastructure.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace App.Infrastructure.Repository
{
    [TypeFilter(typeof(GlobalLoggerFilter))]
    public class RoadRepository : IRoadRepositoryContract
    {
        private readonly RoadCityDbContext _dbContext;
        public RoadRepository(RoadCityDbContext db)
        {
            _dbContext = db;
        }
        public async Task<Road> AddRoad(Road road)
        {
            await _dbContext.AddAsync(road);
            await _dbContext.SaveChangesAsync();
            return road;
        }

        public async Task<int> DeleteRoad(Guid id)
        {
            Road r = await getRoadByID(id);
            _dbContext.Remove(r);
            int rows = await _dbContext.SaveChangesAsync();
            return rows;
        }

        public async Task<Road> getRoadByID(Guid id)
        {
            Road? r = await _dbContext.roads.Include(r=> r.region).Include(r => r.difficulty).FirstOrDefaultAsync(r=> r.id == id);  
            if (r == null)
            {
                return null;
            }
            return r;
        }

        public async Task<Road> getRoadByName(string name)
        {

            Road? road = await _dbContext.roads.Include(r => r.region).Include(r=> r.difficulty).FirstOrDefaultAsync(r => r.Name.ToLower() == name.ToLower());
            if (road == null)
            {
                return null;
            }
            return road;
        }

        public async Task<List<Road>> GetRoads(string? DifficultyLevel = null,string? RoadLengthOrder=null, string? SearchByName = null)
        {
            var roads = _dbContext.roads.Include(r => r.region).Include(r => r.difficulty).AsQueryable();
            if(DifficultyLevel != null)
            {
                roads =  roads.Where(x => x.difficulty.Name.Equals(DifficultyLevel));
            }
            if (RoadLengthOrder !=null )
            {
                if (RoadLengthOrder.Equals("Asc"))
                {
                    roads = roads.OrderBy(x => x.LengthInKm);
                }
                else if (RoadLengthOrder.Equals("Desc"))
                {
                    roads = roads.OrderByDescending(x => x.LengthInKm);
                }
            }
            if(SearchByName != null)
            {
                roads = roads.Where(x=> x.Name.Contains(SearchByName));
            }
            return await roads.ToListAsync();

            //return await _dbContext.roads.Include(r => r.region).Include(r=> r.difficulty).ToListAsync();
        }

        public async Task<Road> UpdateRoad(Guid id, Road road)
        {
            Road? UpdatedRoad = await _dbContext.roads.FindAsync(id);
            if (UpdatedRoad == null)
            {
                return null;
            }
            UpdatedRoad.Name = road.Name;
            UpdatedRoad.Description= road.Description;
            UpdatedRoad.RoadImageUrl = road.RoadImageUrl;
            UpdatedRoad.regionId = road.regionId;
            UpdatedRoad.difficultyID = road.difficultyID;
            await _dbContext.SaveChangesAsync();
            return UpdatedRoad;
        }
    }
}
