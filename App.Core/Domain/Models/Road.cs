using System.ComponentModel.DataAnnotations;

namespace App.Core.Domain.Models
{
    public class Road
    {
        [Key]
        public Guid id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double LengthInKm { get; set; }

        public string? RoadImageUrl { get; set; }
        
        // navigation property to regions
        public Guid regionId { get; set; }
        public Region region { get; set; }
        

        // navigation property to difficulty 
        public Guid difficultyID { get; set; }
        public Difficulty difficulty { get; set; }


    }
}
