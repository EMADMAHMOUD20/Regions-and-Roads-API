using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Domain.IdentityModels
{
    [Owned]
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime ExpiredOn { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiredOn; 
        public DateTime? CreatedOn { get; set; }
        public DateTime? RevokenOn { get; set; }
        public bool IsActive => RevokenOn == null || !IsExpired;
    }
}
