namespace SimpleProjectWebAPIwithDIandEF.Filters
{
    public class Class
    {
        /*
           [HttpPost("Generate-New-Token")]
        public async Task<IActionResult> GenerateNewToken(TokenModel Tokenmodel)
        {
            if (Tokenmodel == null)
            {
                return BadRequest("invalid Request");
            }
            string? jwtToken = Tokenmodel.Token;
            string? refreshToken = Tokenmodel.RefreshToken;
            ClaimsPrincipal? claims = _jwtService.GetPrincipalFromJwtToken(jwtToken);
            if (claims == null) {
                return BadRequest("invalid jwt access token");
            }
            string? email = claims.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser? user = await _usermanager.FindByEmailAsync(email);
            if (user == null|| user.RefreshToken != Tokenmodel.RefreshToken || user.RefreshTokenExpirationDateTime<= DateTime.Now) {
                return BadRequest("invalid refresh token ");
            }

            AuthenticationResponse response = _jwtService.CreateJwtToken(user);
            user.RefreshToken = response.RefreshToken;
            user.RefreshTokenExpirationDateTime = response.RefreshTokenExpirationDateTime;
            await _usermanager.UpdateAsync(user);
            return Ok(response);

        }
         
         
         public ClaimsPrincipal? GetPrincipalFromJwtToken(string token)
        {
            // token validation 
            var tokenValidationParameter = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudience = _cofiguration["Jwt:Audience"],
                ValidateIssuer = true,
                ValidIssuer = _cofiguration["Jwt:Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cofiguration["Jwt:Key"])),
                ValidateLifetime = false, // because we call this method only when token is expired
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal claims = jwtSecurityTokenHandler.ValidateToken(
                token
                ,tokenValidationParameter
                ,out SecurityToken securityToken
                );
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return claims;

        }

         */
    }
}
