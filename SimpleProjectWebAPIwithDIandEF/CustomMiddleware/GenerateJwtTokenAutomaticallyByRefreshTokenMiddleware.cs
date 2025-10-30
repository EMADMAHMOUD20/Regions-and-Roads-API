using App.Core.ServicesContract;

namespace SimpleProjectWebAPIwithDIandEF.CustomMiddleware
{
    public class GenerateJwtTokenAutomaticallyByRefreshTokenMiddleware
    {
        private readonly RequestDelegate _next;
        public GenerateJwtTokenAutomaticallyByRefreshTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, IJwtService jwtService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null && jwtService.isTokenExpired(token))
            {
                var refreshToken = context.Request.Cookies["Refresh-Token"];
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    var newToken = await jwtService.GetTokenBasedOnRefreshToken(refreshToken);
                    context.Request.Headers["Authorization"] = "Bearer " + newToken.Token;
                    context.Response.OnStarting(() =>
                    {
                        // Add new JWT to headers (optional)
                        context.Response.Headers["X-New-JWT"] = newToken.Token;

                        // Add new refresh token to cookies
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = newToken.RefreshTokenExpiration.ToLocalTime()
                        };

                        context.Response.Cookies.Append("Refresh-Token", newToken.RefreshToken, cookieOptions);
                        return Task.CompletedTask;
                    });
                }
            }
            await _next(context);
           
        }

    }
}
