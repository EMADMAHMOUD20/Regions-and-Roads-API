using App.Core.Domain.Models;
using App.Core.ExtentionMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.DTOs.RoadDTO_s
{
    public class InputRoadDTO
    {
        [Required]
        public string Name {  get; set; }

        [Required]
        public string Description { get; set; }
        [Required]
        [Range(1,20000)]
        public double LengthInKm { get; set; }
        
        public string RoadImageUrl { get; set; }

        [Required]
        public Guid regionId { get; set; }

        //public Guid difficultyID { get; set; }
        [Required]
        public string difficultyType { get; set; }

        public Road ToRoad()
        {
            Road road = new Road()
            {
                Name = this.Name,
                Description = this.Description,
                LengthInKm = this.LengthInKm,
                RoadImageUrl = this.RoadImageUrl,
                regionId = this.regionId,
                //difficultyID = this.difficultyID,
                difficultyID = RoadExtentionMethod.GetDifficultyId(difficultyType)

            };
            return road;
        }


    }
}
