using App.Core.Domain.Models;
using App.Core.DTO_s.RoadDTO_s;
using App.Core.DTOs.RegionDTO_s;
using System.ComponentModel.DataAnnotations;

namespace App.Core.DTO_s.RegionDTO_s
{
    public class ReadRegionDTO
    {
        public Guid Id { get; set; }    

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
        public List<RoadsForReadRegionDTO> Roads { get; set; }
    }
    
}
