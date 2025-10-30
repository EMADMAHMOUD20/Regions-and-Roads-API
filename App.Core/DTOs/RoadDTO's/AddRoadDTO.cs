using System.ComponentModel.DataAnnotations;

namespace App.Core.DTO_s.RoadDTO_s
{
    public class AddRoadDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Length(7,2000)]
        public string Description { get; set; }
        [Required]
        public double LengthInKm { get; set; }

        public string? RoadImageUrl { get; set; }

        [Required]
        public Guid regionId { get; set; }   

    }
    /*  public Guid id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double LengthInKm { get; set; }

        public string? RoadImageUrl { get; set; }
        
        // navigation property to regions
        public Guid regionId { get; set; }
        public Region region { get; set; }
        

        // navigation property to difficulty 
        public Guid difficultyID { get; set; }
        public Difficulty difficulty { get; set; }*/
}
