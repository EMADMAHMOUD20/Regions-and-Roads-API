using System.ComponentModel.DataAnnotations;

namespace App.Core.Domain.Models
{
    public class Difficulty
    {

        [Key]
        public Guid id { get; set; }

        public string Name { get; set; }
        
        public List<Road> roads { get; set; }
    }
}
