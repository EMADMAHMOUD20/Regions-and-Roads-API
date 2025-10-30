using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.DTOs.IdentityDTO_s
{
    public class VerifyOtpDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Length(6,6,ErrorMessage ="please enter the valid otp")]
        public string OTP {  get; set; }
    }
}
