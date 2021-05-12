using cw8.Contexts;
using cw8.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static cw8.Models.User;

namespace cw8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private const int ACCESS_TOKEN_LIFETIME = 20; //minutes
        private const int REFRESH_TOKEN_LIFETIME = 1; //days

        private const int SALT_LENGTH = 32; //bytes

        private readonly HospitalContext _context;
        private readonly IConfiguration _config;

        public AccountsController(HospitalContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginRequest loginRequest)
        {
            User user = _context.Users.Where(user => user.Login == loginRequest.Login).FirstOrDefault();

            if(user == default) return StatusCode(401, "No such user was found");
            if(!user.PasswordHashed.SequenceEqual(loginRequest.Password.Encrypt(user.Salt))) return StatusCode(401, "Wrong password");

            var creds = _config["SecretKey"].ToSigningCredentials();
            var accessToken = GetNewJwtToken(creds, user);

            var refreshToken = user.RenewRefreshToken(REFRESH_TOKEN_LIFETIME);
            _context.Update(user);
            await _context.SaveChangesAsync();
            
            return Ok(new 
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                refreshToken = refreshToken
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterRequest registerRequest)
        {
            User user = _context.Users.Where(user => user.Login == registerRequest.Login).FirstOrDefault();
            if (user != default) return StatusCode(409, "Such user already exists");
            try
            {
                user = CreateUser(registerRequest);
            }
            catch (UndefinedUserRoleException)
            {
                return StatusCode(422, "Undefined user role occured");
            }


            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("refresh")]
        public ActionResult RefreshAccessToken([FromHeader] string login, [FromHeader] byte[] refreshToken)
        {
            User user = _context.Users.Where(user => user.Login == login).FirstOrDefault();
            if (user == default) return StatusCode(401, "No such user was found");
            if (!user.RefreshToken.SequenceEqual(refreshToken)) return StatusCode(401, "Wrong refresh token");
            if (user.RefreshTokenExpirationDate.CompareTo(DateTime.Now) < 0) return StatusCode(401, "Refresh token has expired");

            var creds = _config["SecretKey"].ToSigningCredentials();
            var accessToken = GetNewJwtToken(creds, user);

            return Ok(new JwtSecurityTokenHandler().WriteToken(accessToken));
        }

        #region Utility

        private static User CreateUser(RegisterRequest registerRequest)
        {
            string userRole;
            if (Enum.IsDefined(typeof(UserRoles), registerRequest.UserRole)) userRole = registerRequest.UserRole;
            else throw new UndefinedUserRoleException();

            var salt = GetSalt();
            return new User()
            {
                Login = registerRequest.Login,
                PasswordHashed = registerRequest.Password.Encrypt(salt),
                UserRole = userRole,
                Salt = salt,
                RefreshToken = Guid.NewGuid().ToByteArray(),
                RefreshTokenExpirationDate = DateTime.Now.AddDays(REFRESH_TOKEN_LIFETIME)
            };
        }

        private static byte[] GetSalt()
        {
            var salt = new byte[SALT_LENGTH];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }

        private static JwtSecurityToken GetNewJwtToken(SigningCredentials creds, User user)
        {
            var userClaims = new[]
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.UserRole)
            };

            return new
            (
                issuer: "http://localhost:62361",
                audience: "http://localhost:62361",
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(ACCESS_TOKEN_LIFETIME),
                signingCredentials: creds
            );
        }

        #endregion
    }
}
