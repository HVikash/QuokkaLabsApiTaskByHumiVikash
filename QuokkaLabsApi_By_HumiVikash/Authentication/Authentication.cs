using Microsoft.AspNet.Identity.Owin;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using QuokkaLabsApi_By_HumiVikash.Models.DTOs;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using QuokkaLabsApi_By_HumiVikash.Models;

namespace QuokkaLabsApi_By_HumiVikash.Authentication
{
    public class Authentication
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public Authentication(UserManager<ApplicationUser> userManager, IConfiguration configuration, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        internal string Generate(Task<UserForJwtDto> user)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier,user.Result.UserName),
                new Claim(ClaimTypes.Email,user.Result.Email),
                new Claim(ClaimTypes.Role,user.Result.Role)


                };

                var token = new JwtSecurityToken(
                   issuer: _configuration["Jwt:Issuer"],
                   audience: _configuration["Jwt:Audience"],
                   expires: DateTime.Now.AddDays(30),

                   claims: claims,

                    signingCredentials: credentials);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }


        internal async Task<UserForJwtDto> Authenticate(LoginUserDto user)
        {
            try
            {
                var currentuser = await _signInManager.PasswordSignInAsync(user.Username, user.Password, false, false);
                //var tes = currentuser.Succeeded.ToString();
                if (currentuser.Succeeded)
                {
                    var userdata = await _userManager.FindByNameAsync(user.Username);
                    string? role = string.Empty;
                    if (userdata != null)
                    {
                        role = _userManager.GetRolesAsync(userdata).Result.FirstOrDefault();
                        return new UserForJwtDto()
                        {
                            UserName = user.Username,
                            Email = string.IsNullOrEmpty(userdata.Email) ? "" : userdata.Email,

                            Role = string.IsNullOrEmpty(role) ? "" : role,
                            isSuccess = true
                        };
                    }



                }
                else
                {
                    return new UserForJwtDto()
                    {
                        UserName = user.Username,
                        isSuccess = false
                    };

                }
            }
            catch (Exception e)
            {
                return new UserForJwtDto()
                {
                    UserName = user.Username,
                    isSuccess = false
                };
            }

            return null;
        }
    }
}
