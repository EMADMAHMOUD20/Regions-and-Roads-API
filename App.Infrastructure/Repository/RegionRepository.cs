using App.Core.Domain.Models;
using App.Core.Domain.RepositoryContracts;
using App.Core.Filters;
using App.Infrastructure.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Repository
{
    [TypeFilter(typeof(GlobalLoggerFilter))]
    public class RegionRepository : IRegionRepositoryContract
    {
        private readonly RoadCityDbContext _dbContext;
        public RegionRepository(RoadCityDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Region> AddRegion(Region region)
        {
            await _dbContext.AddAsync(region);
            await _dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<int> DeleteRegion(Guid id)
        {
            Region r = await getRegionByID(id);
            _dbContext.Remove(id);
            int rows = await _dbContext.SaveChangesAsync();
            return rows;

        }

        public async Task<Region> getRegionByID(Guid id)
        {
            Region r = await _dbContext.regions.Include(r=> r.roads).FirstOrDefaultAsync(r=> r.id == id);
            if(r == null)
            {
                return null;
            }
            return r;
        }

        public async Task<Region> getRegionByName(string name)
        {
            Region? region = await _dbContext.regions.Include(r=> r.roads).FirstOrDefaultAsync(r=> r.Name.ToLower() == name.ToLower());
            if (region == null) {
                return null;
            }
            return region;

        }

        public async Task<List<Region>> GetRegions()
        {
            return await _dbContext.regions.Include(r=> r.roads).ToListAsync();
            
        }

        public async Task<Region> UpdateRegion(Guid id, Region region)
        {
            Region r = await _dbContext.regions.FindAsync(id);
            if (r == null) {
                return null;
            }
            r.Name = region.Name;
            r.Code = region.Code;
            r.RegionImageUrl = region.RegionImageUrl;
            await _dbContext.SaveChangesAsync();
            return r;
        }
    }
}
