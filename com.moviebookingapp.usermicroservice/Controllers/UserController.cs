using com.moviebookingapp.usermicroservice.Collection;
using com.moviebookingapp.usermicroservice.Models;
using com.moviebookingapp.usermicroservice.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Core;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace com.moviebookingapp.usermicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _iuserRepository;
        private readonly IConfiguration _configuration;

        public UserController(IUserRepository iuserRepository, IConfiguration configuration)
        {
            _iuserRepository = iuserRepository;
            _configuration = configuration;

        }

        // Login 
        [HttpGet]
        [Route("/api/v1.0/moviebooking/login")]
        public async Task<IActionResult> Get([FromQuery] LoginModel loginModel)
        {
            var logedinUser = await _iuserRepository.ValidateLoginUser(loginModel.Email, loginModel.LoginId);
            if (logedinUser == null)
            {
                return NotFound("No user found with the given credentials");
            }
            var verifyPassword = BCrypt.Net.BCrypt.Verify(loginModel.Password, logedinUser.Password);
            if (!verifyPassword)
            {
                return Unauthorized("Invalid Password");
            }
            var claims = new List<Claim>
            {

                  new Claim(JwtRegisteredClaimNames.Email,loginModel.Email ),
                  new Claim(ClaimTypes.Email,loginModel.Email ),
                  new Claim(ClaimTypes.Role,logedinUser.Role)

            };
            var token = GenerateAccessToken(claims);
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                user_id = logedinUser.Id
            });
        }



        // register as a new user 
        [Route("/api/v1.0/moviebooking/register")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Users users)
        {
            var newUser = await _iuserRepository.ValidateUser(users.Email, users.LoginId);
            if (newUser != null)
            {
                return BadRequest("Userdetails already exists");
            }
            users.Password = BCrypt.Net.BCrypt.HashPassword(users.Password);
            await _iuserRepository.Register(users);
            return CreatedAtAction(nameof(Get), new { id = users.Id }, users);
        }

        //Forgot password
        [HttpGet]
        [Route("/api/v1.0/moviebooking/forgot-password")]
        public async Task<IActionResult> Get([FromQuery] ForgotPasswordRequest forgotPasswordModel)
        {
            var user = await _iuserRepository.ForgotPassword(forgotPasswordModel.Email);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // create reset token that expires after 1 day
            user.ResetToken = generateResetToken();
            user.ResetTokenExpires = DateTime.UtcNow.AddDays(1);

            await _iuserRepository.UpdateResetToken(user);



            return Ok(new { message = "Please check your email for password reset instructions" });
        }

        // reset user password
        [Route("/api/v1.0/moviebooking/reset-passowrd")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ResetPasswordRequest resetPassword)
        {
            var user = await _iuserRepository.ValidateResetToken(resetPassword.Token);
            if (user == null)
            {
                return NotFound("Invalid Token");
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(resetPassword.NewPassword);
            user.ResetToken = null;
            user.ResetTokenExpires = null;
            await _iuserRepository.UpdatePassword(user);
            return Ok(new { message = "Password reset successful, you can now login" });
        }

        private JwtSecurityToken GenerateAccessToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private string generateResetToken()
        {
            // token is a cryptographically strong random sequence of values
            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

            return token;
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // GET api/<UserController>/5 
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
    }
}
