

using App.Core.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace App.Core.DTO_s.RegionDTO_s
{
    public class UpdateRegionDTO
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }

        public Region ToRegion(UpdateRegionDTO regionDTO)
        {
            Region r = new Region()
            {
                Name = this.Name,
                RegionImageUrl = this.RegionImageUrl,
                Code = this.Code
            };
            return r;
        }
    }
}
