using App.Core.Domain.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.Core.DTOs.IdentityDTO_s
{
    public class AuthenticationResponse
    {
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
        public DateTime Expiration { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }

        /*public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpirationDateTime { get; set; }*/
    }
}
//dotnet ef migrations add addRefreshTokenExpirationTime --project App.Infrastructure --startup-project SimpleProjectWebAPIwithDIandEF


//dotnet ef database update --project App.Infrastructure --startup-project SimpleProjectWebAPIwithDIandEF