using API.Extensions;
using API.ViewModels;
using Hard.Business.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly UserManager<IdentityUser> _userManager;

        private readonly JwtSettings _jwtSettings;

        public AuthController(INotifier notifier, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<JwtSettings> jwtSettings) : base(notifier)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }        

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser() 
            { 
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return CustomResponse(await GenerateJwt(user.Email));
            }

            foreach (var erro in result.Errors)
            {
                NotifyError(erro.Description);
            }
            
            return CustomResponse();
        }

        [HttpPost("signin")]
        public async Task<ActionResult> SignIn(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (result.Succeeded)
            {                
                return CustomResponse(await GenerateJwt(loginUser.Email));
            }

            if (result.IsLockedOut)
            {
                NotifyError("The user was temporarily locked out because of many failed access attempts.");
                return CustomResponse();
            }

            if (result.IsNotAllowed)
            {
                NotifyError("The user is not allowed to sign in.");
                return CustomResponse();
            }

            NotifyError("Incorrect user or password.");

            return CustomResponse();
        }

        private async Task<LoginResponseViewModel> GenerateJwt(string email) 
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor() 
            { 
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.ValidAt,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Subject = identityClaims,
                
                
            });            

            var encodedToken = tokenHandler.WriteToken(token);

            return new LoginResponseViewModel() 
            { 
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_jwtSettings.ExpirationTime).TotalSeconds,
                UserToken = new UserTokenViewModel() 
                { 
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(x => new ClaimViewModel() { Type = x.Type, Value = x.Value})
                }
            };
        }
    }
}
