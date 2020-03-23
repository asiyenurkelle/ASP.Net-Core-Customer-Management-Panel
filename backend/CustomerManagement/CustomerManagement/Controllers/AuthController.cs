using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using CustomerManagement.Domain;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
namespace CustomerManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]

    public class AuthController : Controller
    {

        private ILogger<AuthController> _logger;
        private IPasswordHasher<AppUser> _hasher;
        private IConfigurationRoot _config;
        private UserManager<AppUser> _userManager;

        public AuthController(UserManager<AppUser> userManager, IPasswordHasher<AppUser> hasher, ILogger<AuthController> logger, IConfigurationRoot config)
        {
            _logger = logger;
            _hasher = hasher;
            _config = config;
            _userManager = userManager;
        }


        [HttpPost("token")]
        public async Task<IActionResult> CreateToken([FromBody] CredentialModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    if (_hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) ==
                        PasswordVerificationResult.Success)
                    {
                        return Ok(CreateToken(user));
                    }
                }
            }

            catch (Exception ex)
            {
                _logger.LogError($"JWT yaratırken Exception fırlatıldı: {ex}");
            }
            return null;
        }



        private async Task<JwtPacket> CreateToken(AppUser appUser)
        {
            var userClaims = await _userManager.GetClaimsAsync(appUser);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, appUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            }.Union(userClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config["Tokens:Issuer"],
                audience: _config["Tokens:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds

                );
            return new JwtPacket
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo.ToString(),
                UserName = appUser.UserName
            };

        }


    }
    public class JwtPacket
    {
        public string Token;
        public string UserName;
        public string Expiration;
    }
}