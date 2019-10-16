using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using dataStore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace secAPI.Controllers
{
    public class AuthController : Controller
    {
        private readonly Context context;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IConfiguration configurationRoot;

        public AuthController(Context context, UserManager<User> userManager, SignInManager<User> signInManager, IPasswordHasher<User> passwordHasher, IConfiguration configurationRoot)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.passwordHasher = passwordHasher;
            this.configurationRoot = configurationRoot;
        }

        [HttpPost("api/auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await signInManager.PasswordSignInAsync(loginModel.username, loginModel.password, false, false);
            if (result.Succeeded)
                return Ok();

            return BadRequest();
        }

        [HttpPost("api/auth/token")]
        public async Task<IActionResult> CreateToken([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.FindByNameAsync(loginModel.username);
            if(user != null)
            {
                if(passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginModel.password) == PasswordVerificationResult.Success)
                {
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurationRoot["tokens:key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: configurationRoot["tokens:issuer"],
                        audience: configurationRoot["tokens:audience"],
                        claims:new[] {
                            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        },
                        expires:DateTime.Now.AddMinutes(5),
                        signingCredentials:creds);

                return Ok(new {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                });
                }
            }

            ModelState.AddModelError("usernameOrPasswordError","Username or password was wrong");
            return BadRequest(ModelState);
        }
    }
}
