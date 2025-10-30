using App.Core.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace App.Core.DTO_s.RegionDTO_s
{
    public class AddRegionDTO
    {
        
        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
        // not used
        protected Region ToRegion(AddRegionDTO regionDTO)
        {
            Region r = new Region() {
                Name = this.Name,
                RegionImageUrl = this.RegionImageUrl, 
                Code = this.Code 
            };
            return r;
        }
    }

}
