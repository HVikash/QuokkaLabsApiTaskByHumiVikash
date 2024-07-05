//using Azure;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuokkaLabsApi_By_HumiVikash.DatabaseContext;
//using QuokkaLabsApi_By_HumiVikash.Migrations;
using QuokkaLabsApi_By_HumiVikash.Models;
using QuokkaLabsApi_By_HumiVikash.Models.DTOs;
using QuokkaLabsApi_By_HumiVikash.Models.DTOs.Response;


using System.Net;

namespace QuokkaLabsApi_By_HumiVikash.Controllers
{
   
    //token routing
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        protected readonly Response _response;
        private readonly QuokkaLabsApi_By_HumiVikash.Authentication.Authentication _auth;

        public AccountController(ApplicationDBContext db, QuokkaLabsApi_By_HumiVikash.Authentication.Authentication auth, UserManager<ApplicationUser> userManager, IConfiguration config,
            RoleManager<IdentityRole> roleManager, Response response)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _response = response;
            _auth = auth;

        }
      
        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<Response> Register(ApplicationUserDto user)
        {

            ApplicationUser registerUser = new ApplicationUser()
            {
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password,
                PhoneNumber = user.Phone,
                address = user.address,
                NormalizedEmail = user.UserName.ToUpper(),
                Name = user.Name

            };
            try
            {

                var isExists = await _userManager.FindByNameAsync(registerUser.UserName);
                IdentityResult? result;
                if (isExists == null)
                {

                    result = await _userManager.CreateAsync(registerUser, user.Password);
                    await _userManager.AddToRoleAsync(registerUser, user.Role);
                    if (result.Succeeded)
                    {

                        var userToreturn = _db.ApplicationUsers.FirstOrDefault(x => x.UserName == user.UserName);

                        _response.Status = (System.Net.HttpStatusCode)StatusCodes.Status201Created;
                        _response.IsSuccess = true;

                        _response.Result = user;
                    }
                    else
                    {
                        _response.Status = (System.Net.HttpStatusCode)StatusCodes.Status424FailedDependency;
                        _response.IsSuccess = false;
                        _response.ErrorMessage = "Something went wrong.Unable to create user.Please talk to admin!";
                        _response.Result = user;
                    }


                }
                else
                {
                    _response.Status = (System.Net.HttpStatusCode)StatusCodes.Status409Conflict;
                    _response.IsSuccess = false;
                    _response.ErrorMessage = "User with same name already exists!";

                    _response.Result = user;
                }
            }
            catch (Exception ex)
            {
                _response.Status = (System.Net.HttpStatusCode)StatusCodes.Status400BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                _response.Result = user;

            }
            return _response;
        
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public ActionResult<Response> Login(LoginUserDto user)
        {
            try
            {
                var loginuser = _auth.Authenticate(user);
                if (loginuser.Result.isSuccess == true)
                {
                    var token = _auth.Generate(loginuser);
                    _response.Status = System.Net.HttpStatusCode.OK;
                    _response.IsSuccess = true;
                    _response.Result = new LoginResponse()
                    {
                        UserName = user.Username,
                        Token = token
                    }; ;

                }
                else
                {
                    _response.Status = System.Net.HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessage = "Authentication failed!Please enter correct credentials!";
                    _response.Result = user;
                }
            }

            catch (Exception ex)
            {
                _response.Status = System.Net.HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                _response.Result = user;
            }
            return Ok(_response);

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("{user}")]
        public  ActionResult<UserInfo> GetProfile(string user)
        {
            try
            {
                var isExists =  _userManager.FindByNameAsync(user);
                if (isExists==null)
                {
                    return StatusCode(404);
                }
                return new UserInfo()
                {
                    Name = string.IsNullOrEmpty(isExists.Result.Name) ? "": isExists.Result.Name,
                    UserName = string.IsNullOrEmpty(isExists.Result.UserName) ? "" : isExists.Result.UserName,
                    Email = string.IsNullOrEmpty(isExists.Result.Email) ? "" : isExists.Result.Email,
                    Phone = string.IsNullOrEmpty(isExists.Result.PhoneNumber) ? "" : isExists.Result.PhoneNumber,
                    address = string.IsNullOrEmpty(isExists.Result.address) ? "" : isExists.Result.address,
                };
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("AddRole")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<Response> AddRole(AddRolesDto role)
        {
            try
            {
                var IsRoleExists = await _roleManager.RoleExistsAsync(role.RoleName);
                //var test =  _roleManager.Roles.ToList();

                if (IsRoleExists)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = "Role already Exists!";
                    _response.Status = (HttpStatusCode)StatusCodes.Status409Conflict;


                }
                else
                {
                    await _roleManager.CreateAsync(new IdentityRole(role.RoleName));

                    _response.IsSuccess = true;

                    _response.Status = (HttpStatusCode)StatusCodes.Status201Created;
                    _response.Result = role;
                }

            }
            catch (Exception exp)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = exp.Message;
                _response.Status = (HttpStatusCode)StatusCodes.Status400BadRequest;

            }
            return _response;
        }
    }
}
