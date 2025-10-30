using App.Core.Domain.IdentityModels;
using App.Core.DTOs.IdentityDTO_s;
using App.Core.Exceptions.JwtTokenExceptions;
using App.Core.Filters;
using App.Core.ServicesContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace SimpleProjectWebAPIwithDIandEF.Controllers
{
    [TypeFilter(typeof(GlobalLoggerFilter))]
    [TypeFilter(typeof(WrapperFilter))]
    //[AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _rolemanager;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
         
        public AccountController(SignInManager<ApplicationUser> signInManager, 
            RoleManager<ApplicationRole> rolemanager,
            UserManager<ApplicationUser> userManager,
            IJwtService jwtService, IConfiguration configuration, IEmailService emailService)
        {
            _signInManager = signInManager;
            _rolemanager = rolemanager;
            _usermanager = userManager;
            _jwtService = jwtService;
            _configuration = configuration;
            _emailService = emailService;

        }

        [HttpPost("Register")]
        [SwaggerOperation(summary:"Register a new user ",description:"Register a new user and get your Tokens")]
        public async Task<ActionResult<ApplicationUser>> Register(RegisterDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(v=>v.Errors).SelectMany(e=>e.ErrorMessage).ToList());
            }
            var CheckFound = await _usermanager.FindByEmailAsync(userDTO.Email);
            if(CheckFound != null)
            {
                return BadRequest($"this email {userDTO.Email} already taken");
            }
            ApplicationUser user = new ApplicationUser()
            {
                UserName = userDTO.Email,
                PersonName = userDTO.PersonName,
                Email = userDTO.Email,
                PhoneNumber = userDTO.PhoneNumber,
            };

            IdentityResult result = await _usermanager.CreateAsync(user,userDTO.Password);
            if (result.Succeeded)
            {
                //sign-in 
                await _signInManager.SignInAsync(user, isPersistent: false);
                AuthenticationResponse authenticatedUser = await _jwtService.GetTokenForRegister(user);
                SetRefreshTokenInCookie(authenticatedUser.RefreshToken, authenticatedUser.RefreshTokenExpiration);
                return Ok(authenticatedUser);
            }
            else
            {
                string errorMessage = string.Join("|", result.Errors.Select(e => e.Description));
                return Problem(errorMessage);
            }
        }

        [HttpPost("Login")]
        [SwaggerOperation(description:"Login with email and password and get the OTP in Your Email Then Put it in Verify OTP Endpoint")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(v=>v.Errors).SelectMany(e=>e.ErrorMessage));
            }
            var result = await _signInManager.PasswordSignInAsync(
                loginDTO.email, loginDTO.password, 
                isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded) { 
                
                ApplicationUser? user = await _usermanager.FindByEmailAsync(loginDTO.email);
                if (user == null) {
                    return NoContent();
                }
                var otp = new Random().Next(100000, 999999).ToString();
                user.OtpCode = otp;
                user.IsOtpVerified = false;
                user.OtpExpirationTime = DateTime.UtcNow.AddMinutes(5);
                await _usermanager.UpdateAsync(user);
                await _emailService.SendEmailAsync(user.Email, "Your OTP Code", $"Your OTP Code is {otp}");
                //AuthenticationResponse authenticatedUser = await _jwtService.GetTokenForLogin(user);
                return Ok("Otp Sent to your email, Please Verify in [Verify-OTP-endpoint] To Continue");
                //SetRefreshTokenInCookie(authenticatedUser.RefreshToken, authenticatedUser.RefreshTokenExpiration);
                //return Ok(authenticatedUser);

            }
            else
            {
                return Unauthorized("invalid Email or password");
            }
        }
        [HttpPost("Verify-OTP-Endpoint")]
        [SwaggerOperation(description: "This Endpoint take your OTP and Return User Tokens")]
        public async Task<IActionResult> VerifyOTP(VerifyOtpDto model)
        {
            if (!ModelState.IsValid) { 
                return BadRequest(ModelState.Values.SelectMany(v=> v.Errors).SelectMany(e=> e.ErrorMessage));
            }
            var user = await _usermanager.FindByEmailAsync(model.Email);
            if (user == null) {
                return NotFound("user not found");
            }
            if(user.OtpCode != model.OTP)
            {
                return BadRequest("something went wrong please try again");
            }
            if (user.OtpExpirationTime <= DateTime.UtcNow) {
                return BadRequest("expired otp");
            }
            AuthenticationResponse authenticationResponse = await _jwtService.GetTokenForLogin(user);
            user.OtpExpirationTime = null;
            user.OtpCode = null;
            user.IsOtpVerified = true;
            await _usermanager.UpdateAsync(user);
            return Ok(authenticationResponse);

        }


        [HttpGet("Create-New-Jwt-BaseOn-Refresh-Token")]
        [SwaggerOperation(description:"This endpoint Generates a new JWT token, Use it when your token expired")]
        public async Task<IActionResult> CheckRefreshToken()
        {
            var refreshToken = Request.Cookies["Refresh-Token"];
            try
            {
                var result = await _jwtService.GetTokenBasedOnRefreshToken(refreshToken);
                SetRefreshTokenInCookie(result.RefreshToken,result.RefreshTokenExpiration);
                return Ok(result);
            }
            catch (InvalidRefreshTokenException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
            
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return NoContent();
        }

        
        private void SetRefreshTokenInCookie(string refreshToken,DateTime expiredon)        
        {
            CookieOptions cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expiredon.ToLocalTime(),
            };
            Response.Cookies.Append("Refresh-Token",refreshToken, cookieOptions);
        }

      
    }
}
