using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.DTOs.IdentityDTO_s
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress(ErrorMessage ="must be in email format")]
        public string email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
