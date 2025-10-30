using App.Core.Domain.IdentityModels;
using App.Core.DTOs.IdentityDTO_s;
using App.Core.Exceptions.JwtTokenExceptions;
using App.Core.Filters;
using App.Core.ServicesContract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Services
{
    [TypeFilter(typeof(GlobalLoggerFilter))]
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _cofiguration;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly ILogger<JwtService> _Logger;
        public JwtService(IConfiguration configuration,UserManager<ApplicationUser> usermanager,ILogger<JwtService> logger)
        {
            _Logger = logger;
            _cofiguration= configuration;
            _usermanager= usermanager;
        }
        public JwtSecurityToken CreateJwtToken(ApplicationUser user)
        {
            DateTime Expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_cofiguration["Jwt:Expiration_Minutes"]));
            Claim[] claims = new Claim[] {
                // user id 
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                // token id 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                // date and time of token generation 
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString()),
                //new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),

                // unique name identifier of the user (you can choose any thing)(optional)
                new Claim(ClaimTypes.NameIdentifier, user.Email),

                new Claim(ClaimTypes.Name, user.PersonName)
            };

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cofiguration["Jwt:Key"]));

            // key with hashing algorithm 
            SigningCredentials signingCredentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            JwtSecurityToken tokenSetup = new JwtSecurityToken(
                _cofiguration["Jwt:Issuer"],
                _cofiguration["Jwt:Audience"],
                claims,
                expires: Expiration,
                signingCredentials:signingCredentials
                );
            /*JwtSecurityTokenHandler token = new JwtSecurityTokenHandler();
            string finalToken = token.WriteToken(tokenSetup);
            
            AuthenticationResponse response = new AuthenticationResponse()
            {
                PersonName = user.PersonName,
                Email = user.Email,
                Token = finalToken,
                Expiration = Expiration,
               // RefreshToken = GenerateRefreshToken(),
                //RefreshTokenExpirationDateTime = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_cofiguration["RefreshToken:Expiration_Minutes"]))
            };*/

            return tokenSetup;
        }

        public async Task<AuthenticationResponse> GetTokenBasedOnRefreshToken(string refreshToken)
        {
            var user = _usermanager.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token.Equals(refreshToken)));
            if(user == null)
            {
                throw new InvalidRefreshTokenException("Invalid Token");
            }

            var userRefreshToken = user.RefreshTokens.Single(t => t.Token.Equals(refreshToken));
            if (userRefreshToken.ExpiredOn <= DateTime.UtcNow || userRefreshToken.RevokenOn != null)
            { 
                throw new InvalidRefreshTokenException("Invalid Token");
            }

            userRefreshToken.RevokenOn = DateTime.UtcNow;

            AuthenticationResponse response = new AuthenticationResponse();

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _usermanager.UpdateAsync(user);

            JwtSecurityToken jwtToken = CreateJwtToken(user);

            response.PersonName = user.PersonName;
            response.Email = user.Email;
            response.RefreshToken = newRefreshToken.Token;
            response.RefreshTokenExpiration = newRefreshToken.ExpiredOn;
            response.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            response.Expiration = jwtToken.ValidTo;

            return response;
        }

        public async Task<AuthenticationResponse> GetTokenForLogin(ApplicationUser user)
        {
            JwtSecurityToken token = CreateJwtToken(user);

            AuthenticationResponse response = new AuthenticationResponse()
            {
                Email = user.Email,
                PersonName = user.PersonName,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                
            };
           
            if (user.RefreshTokens.Any(t => t.ExpiredOn > DateTime.UtcNow && t.RevokenOn == null))
            {
                var ActiveRefreshToken = user.RefreshTokens.FirstOrDefault(t=> t.ExpiredOn > DateTime.UtcNow && t.RevokenOn == null);
                response.RefreshToken = ActiveRefreshToken.Token;
                response.RefreshTokenExpiration = ActiveRefreshToken.ExpiredOn;

            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                response.RefreshToken = refreshToken.Token;
                response.RefreshTokenExpiration = refreshToken.ExpiredOn;
                user.RefreshTokens.Add(refreshToken);
                await _usermanager.UpdateAsync(user);           
            }
            return response;

        }

        public async Task<AuthenticationResponse> GetTokenForRegister(ApplicationUser user)
        {
            AuthenticationResponse response = new AuthenticationResponse();
            JwtSecurityToken token= CreateJwtToken(user);
            RefreshToken refreshToken = GenerateRefreshToken();
            response.Token = new JwtSecurityTokenHandler().WriteToken(token);
            response.Expiration = token.ValidTo;
            response.RefreshToken = refreshToken.Token;
            response.RefreshTokenExpiration = refreshToken.ExpiredOn;
            user.RefreshTokens = new List<RefreshToken>();
            user.RefreshTokens.Add(refreshToken);
            await _usermanager.UpdateAsync(user);
            return response;
        }

        public bool isTokenExpired(string token)
        {
            if(string.IsNullOrEmpty(token)) return true;
            var key = Encoding.UTF8.GetBytes(_cofiguration["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token,new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _cofiguration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _cofiguration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero 
                }, out SecurityToken validatedToken);
                return false;
                
            }catch(SecurityTokenExpiredException) 
            {
                return true;
            }
            catch
            {
                return true;
            }
        }

        private RefreshToken GenerateRefreshToken()
        {
            byte[] bytes = new byte[64];
            var randomNumber = RandomNumberGenerator.Create();
            randomNumber.GetBytes(bytes);
            string token = Convert.ToBase64String(bytes);
            return new RefreshToken()
            {
                Token = token,
                ExpiredOn = DateTime.UtcNow.AddDays(3),
                CreatedOn = DateTime.UtcNow,
            };
        }

        /*
         public string Token { get; set; }
        public DateTime ExpiredOn { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiredOn; 
        public DateTime? CreatedOn { get; set; }
        public DateTime? RevokenOn { get; set; }
        public bool IsActive => RevokenOn == null || !IsExpired;
         
         */
    }
}
