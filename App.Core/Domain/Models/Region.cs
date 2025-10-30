using System.ComponentModel.DataAnnotations;

namespace App.Core.Domain.Models
{
    public class Region
    {

        [Key]
        public Guid id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }

        public List<Road> roads { get; set; }

    }
}
// one walk is connected to one region 
/*
one to one road - region 
 */

