using App.Core.Domain.IdentityModels;
using App.Core.DTOs.IdentityDTO_s;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.ServicesContract
{
    public interface IJwtService
    {
        JwtSecurityToken CreateJwtToken(ApplicationUser user);
        Task<AuthenticationResponse> GetTokenForLogin(ApplicationUser user);
        Task<AuthenticationResponse> GetTokenForRegister(ApplicationUser user);
        Task <AuthenticationResponse> GetTokenBasedOnRefreshToken(string refreshToken);
        public bool isTokenExpired(string token);


    }
}
