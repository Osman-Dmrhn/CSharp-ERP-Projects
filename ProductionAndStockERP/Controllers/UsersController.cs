using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionAndStockERP.Dtos.UserDtos;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;

namespace ProductionAndStockERP.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService,IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        //POST İŞLEMLERİ

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest data)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.VerificationUser(data.Email, data.PasswordHash);
            return Ok(result);
        }


        [Authorize(Roles ="Admin")]
        [HttpPost("createuser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto data)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newuser =  _mapper.Map<User>(data);
            
            var result = await _userService.CreateUserAsync(newuser);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("updateuser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto data)
        {
            if(!ModelState.IsValid) { return BadRequest(ModelState); }

            var user = await _userService.GetUserById(data.UserId);
            if(user.Data != null) {
                _mapper.Map(data, user.Data);

                var result = await _userService.UpdateUserAsync(user.Data);
                return Ok(result);
            }
            return Ok(user);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost("deleteuser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
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
                var result = await _userService.UpdateUserPassword(data.UserId,data.oldpass,data.newpass);
                return Ok(result);
            }
            return Ok(user);
        }

        //GET İŞLEMLERİ
        [Authorize]
        [HttpGet("getuserbyid")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.GetUserById(id);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getalluser")]
        public async Task<IActionResult> GetAllUser(int id)
        {
            var result = await _userService.GetAllUsers();
            return Ok(result);
        }
    }
}
