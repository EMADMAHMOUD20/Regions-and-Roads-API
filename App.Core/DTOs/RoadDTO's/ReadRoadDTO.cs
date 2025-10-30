using System.ComponentModel.DataAnnotations;

namespace App.Core.DTO_s.RoadDTO_s
{
    public class ReadRoadDTO
    {
        public Guid Id { get; set; }    
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double LengthInKm { get; set; }

        public string? RoadImageUrl { get; set; }

        [Required]
        public Guid RegionId { get; set; }

        [Required]
        public string RegionName { get; set; }

        public Guid DifficultyId { get; set; }
        [Required]
        public string DifficultyName { get; set; }
    }
}
