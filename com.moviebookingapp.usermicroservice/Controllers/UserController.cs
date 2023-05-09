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
        private readonly IEmailService _emailService;
        public UserController(IUserRepository iuserRepository, IConfiguration configuration, IEmailService emailService)
        {
            _iuserRepository = iuserRepository;
            _configuration = configuration;
            _emailService = emailService;
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
        [Route("/api/v1.0/moviebooking/{username}/forgot")]
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

            await _iuserRepository.UpdatePassword(user);

            // send email
            sendPasswordResetEmail(user, Request.Headers["origin"]);

            return Ok(new { message = "Please check your email for password reset instructions" });
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

        private  string generateResetToken()
        {
            // token is a cryptographically strong random sequence of values
            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

            return token;
        }

        private void sendPasswordResetEmail(Users user, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                var resetUrl = $"{origin}/account/reset-password?token={user.ResetToken}";
                message = $@"<p>Please click the below link to reset your password, the link will be valid for 1 day:</p>
                            <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Please use the below token to reset your password with the <code>/accounts/reset-password</code> api route:</p>
                            <p><code>{user.ResetToken}</code></p>";
            }

            _emailService.Send(
                to: user.Email,
                subject: "Sign-up Verification API - Reset Password",
                html: $@"<h4>Reset Password Email</h4>
                        {message}"
            );
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
