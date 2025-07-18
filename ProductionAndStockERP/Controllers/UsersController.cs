using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionAndStockERP.Dtos.UserDtos;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;
using ProductionAndStockERP.Services;

namespace ProductionAndStockERP.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IActivityLogsService _activityLogsService;

        public UsersController(IUserService userService,IMapper mapper,IActivityLogsService activityLogsService)
        {
            _userService = userService;
            _mapper = mapper;
            _activityLogsService = activityLogsService;
        }

        //POST İŞLEMLERİ

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest data)
        {
            var result = await _userService.VerificationUser(data.Email, data.PasswordHash);
            return Ok(result);
        }

        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId = User.GetUserId();
            var result = await _userService.GetUserById(userId.Value);
            if (userId is not null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Kullanıcı kimliği token'da bulunamadı.");
            }
        }


        [Authorize(Roles ="Admin")]
        [HttpPost("createuser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto data)
        {
            var newuser =  _mapper.Map<User>(data);

            newuser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newuser.PasswordHash);

            var userId = User.GetUserId();

            if (userId is not null)
            {
                await _activityLogsService.AddLogAsync(userId.Value, $"Kullanıcı Yeni Kullanıcı Ekledi.Kullanıcı:{newuser}");
            }
            else
            {
                return BadRequest("Kullanıcı kimliği token'da bulunamadı.");
            }

            var result = await _userService.CreateUserAsync(newuser);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("updateuser/{id}")]
        public async Task<IActionResult> UpdateUser(int id,[FromBody] UpdateUserDto data)
        {

            var user = await _userService.GetUserById(id);

            if(user.Data != null) {
                _mapper.Map(data, user.Data);

                var userId = User.GetUserId();
                if (userId is not null)
                {
                    await _activityLogsService.AddLogAsync(userId.Value, $"Kullanıcı, Kullanıcı Güncelledi.Kullanıcı:{user.Data.UserId}");
                }
                else
                {
                    return BadRequest("Kullanıcı kimliği token'da bulunamadı.");
                }


                var result = await _userService.UpdateUserAsync(user.Data);
                return Ok(result);
            }
            return Ok(user);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost("deleteuser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userId = User.GetUserId();
            if (userId is not null)
            {
                await _activityLogsService.AddLogAsync(userId.Value, $"Kullanıcı, Kullanıcı Sildi.Kullanıcı:{_userService.GetUserById(id)}");
            }
            else
            {
                return BadRequest("Kullanıcı kimliği token'da bulunamadı.");
            }

            var result = await _userService.DeleteUserAsync(id);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("updateuserpassword")]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordDto  data)
        {
            var user = await _userService.GetUserById(data.UserId);
            if(user.Data != null)
            {
                var userId = User.GetUserId();
                if (userId is not null)
                {
                    await _activityLogsService.AddLogAsync(userId.Value, $"Kullanıcı, Kullanıcı Şifresini Güncelledi.Kullanıcı:{_userService.GetUserById(data.UserId)}");
                }
                else
                {
                    return BadRequest("Kullanıcı kimliği token'da bulunamadı.");
                }

                var result = await _userService.UpdateUserPassword(data.UserId,data.oldpass,data.newpass);
                return Ok(result);
            }
            return Ok(user);
        }

        //GET İŞLEMLERİ
        [Authorize]
        [HttpGet("getuserbyid/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.GetUserById(id);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getalluser")]
        public async Task<IActionResult> GetAllUser()
        {
            var result = await _userService.GetAllUsers();
            return Ok(result);
        }
    }
}
