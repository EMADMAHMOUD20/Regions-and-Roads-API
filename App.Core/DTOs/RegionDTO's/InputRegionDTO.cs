using App.Core.Domain.Models;
using App.Core.DTO_s.RegionDTO_s;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.DTOs.RegionDTO_s
{
    public class InputRegionDTO
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
        protected Region ToRegion(AddRegionDTO regionDTO)
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
