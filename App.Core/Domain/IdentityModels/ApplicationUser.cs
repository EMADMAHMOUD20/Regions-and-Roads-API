using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Domain.IdentityModels
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? PersonName { get; set; }

        public List<RefreshToken>? RefreshTokens { get; set; }

        //public string RefreshToken { get; set; }
        //public DateTime RefreshTokenExpirationDateTime { get; set; }

        public string? OtpCode { get; set; }
        public DateTime? OtpExpirationTime { get; set; }
        public bool IsOtpVerified { get; set; }

    }
}
