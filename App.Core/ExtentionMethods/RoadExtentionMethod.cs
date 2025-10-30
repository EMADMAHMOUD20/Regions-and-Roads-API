using App.Core.Domain.Models;
using App.Core.DTO_s.RoadDTO_s;
using App.Core.Exceptions.RoadsExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.ExtentionMethods
{
    public static class RoadExtentionMethod
    {
        public static ReadRoadDTO toReadRoadDTO(this Road road)
        {
            ReadRoadDTO roadDTO = new ReadRoadDTO()
            {
                Id = road.id,
                Name = road.Name,
                Description = road.Description,
                LengthInKm = road.LengthInKm,
                RoadImageUrl = road.RoadImageUrl,
                RegionId = road.regionId,
                RegionName = road.region?.Name,
                DifficultyId = road.difficultyID,
                DifficultyName = road.difficulty?.Name
            };
            return roadDTO;
        }

       public static Guid GetDifficultyId(string value)
       {
            value = value.ToLower();
            value = char.ToUpper(value[0]) + value.Substring(1);
            switch (value)
            {
                case "Easy":
                    return Guid.Parse("ddddddd1-dddd-dddd-dddd-ddddddddddd1");
                case "Medium":
                    return Guid.Parse("ddddddd2-dddd-dddd-dddd-ddddddddddd2");
                case "Hard":
                    return Guid.Parse("ddddddd3-dddd-dddd-dddd-ddddddddddd3");
                default:
                    throw new UnvalidDifficultyTypeException($"This difficulty {value} is not found Available are [Easy,Medium,Hard]");
            }

       }
    }
}


/*
 
 
  var difficulty1Id = Guid.Parse("ddddddd1-dddd-dddd-dddd-ddddddddddd1");
  var difficulty2Id = Guid.Parse("ddddddd2-dddd-dddd-dddd-ddddddddddd2");
  var difficulty3Id = Guid.Parse("ddddddd3-dddd-dddd-dddd-ddddddddddd3"); 

 */