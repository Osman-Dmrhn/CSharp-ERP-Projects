// Dosya: Controllers/UsersController.cs
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionAndStockERP.Dtos.UserDtos;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;
using System.Security.Claims;

namespace ProductionAndStockERP.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize] 
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        
        [AllowAnonymous] 
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest data)
        {
            var result = await _userService.VerificationUser(data.Email, data.PasswordHash);
            if (!result.Success) return Unauthorized(result);
            return Ok(result);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.GetUserId();
            if (userId == null) return Unauthorized("Geçersiz token.");

            var result = await _userService.GetUserById(userId.Value);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.GetUserById(id);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers([FromQuery] UserFilterParameters filters)
        {
            var result = await _userService.GetAllUsers(filters);
            // Sayfalama header'ları eklenebilir.
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto data)
        {
            var performingUserId = User.GetUserId();
            if (performingUserId == null) return Unauthorized();

            var newUser = _mapper.Map<User>(data);

            var result = await _userService.CreateUserAsync(newUser, performingUserId.Value);

            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto data)
        {
            var performingUserId = User.GetUserId();
            if (performingUserId == null) return Unauthorized();

            var response = await _userService.GetUserById(id);
            if (!response.Success) return NotFound(response);

            var userToUpdate = _mapper.Map<User>(response.Data); 
            _mapper.Map(data, userToUpdate);

            var result = await _userService.UpdateUserAsync(userToUpdate, performingUserId.Value);

            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("password")] 
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordDto data)
        {
            var performingUserId = User.GetUserId();
            if (performingUserId == null) return Unauthorized();

            
            if (!User.IsInRole("Admin") && performingUserId.Value != data.UserId)
            {
                return Forbid("Bu işlemi yapma yetkiniz yok.");
            }

            var result = await _userService.UpdateUserPasswordAsync(data.UserId, data.oldpass, data.newpass, performingUserId.Value);

            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var performingUserId = User.GetUserId();
            if (performingUserId == null) return Unauthorized();

            var result = await _userService.DeleteUserAsync(id, performingUserId.Value);

            if (!result.Success) return NotFound(result);
            return Ok(result);
        }
    }
}