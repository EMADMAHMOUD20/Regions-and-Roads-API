using App.Core.Domain.Models;
using App.Core.DTO_s.RegionDTO_s;
using App.Core.DTO_s.RoadDTO_s;
using App.Core.DTOs.RegionDTO_s;


namespace SimpleProjectWebAPIwithDIandEF.ExtentionMethods
{
    public static class RegionExtentionMethod
    {
        public static ReadRegionDTO toReadRegionDTO(this Region r)
        {
            ReadRegionDTO regionDTO = new ReadRegionDTO()
            {
                Id = r.id,
                Code = r.Code,
                RegionImageUrl = r.RegionImageUrl,
                Name = r.Name,
                Roads = r.roads.OrderBy(x=> x.LengthInKm).Select(x => x.toRoadInRegion()).ToList()
            };
            return regionDTO;
        }

        public static RoadsForReadRegionDTO toRoadInRegion(this Road road) {

            RoadsForReadRegionDTO r = new RoadsForReadRegionDTO()
            {
                Name = road.Name,
                Description = road.Description,
                LengthInKm = road.LengthInKm,
            };
            return r;
            
        }

        public static ResultFromAddRegion ReturnedFromADD(this Region r)
        {
            ResultFromAddRegion regionDTO = new ResultFromAddRegion()
            {
                Id = r.id,
                Code = r.Code,
                RegionImageUrl = r.RegionImageUrl,
                Name = r.Name
            };
            return regionDTO;
        }

    }
}
