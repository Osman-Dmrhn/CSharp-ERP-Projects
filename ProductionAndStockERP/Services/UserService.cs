using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ProductionAndStockERP.Data;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;

namespace ProductionAndStockERP.Services
{
    public class UserService:IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseHelper<IEnumerable<User>>> GetAllUsers()
        {
            var result = await _context.Users.ToListAsync();
            return ResponseHelper<IEnumerable<User>>.Ok(result);
        }
        public async Task<ResponseHelper<User>> GetUserById(int id)
        {
            var result = await _context.Users.FindAsync(id);
            if(result == null)
            {
                return ResponseHelper<User>.Fail("Kullanıcı Bulunamadı");
            }
            return ResponseHelper<User>.Ok(result);
        }

        public async Task<ResponseHelper<string>> VerificationUser(string userName, string passwordhash)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                return ResponseHelper<string>.Fail("Kullanıcı Adı veya Şifre Hatalı");
            }
            else if (user.PasswordHash!=passwordhash)
            {
                return ResponseHelper<string>.Fail("Kullanıcı Adı veya Şifre Hatalı");
            }
            var result = JwtHelper.GenerateJwtToken(userName, user.UserId);
            return ResponseHelper<string>.Ok(result);
        }

        public async Task<ResponseHelper<bool>> CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return ResponseHelper<bool>.Ok(true);
        }
        public async Task<ResponseHelper<bool>> UpdateUserAsync(User user)
        {
            var result = await _context.Users.FindAsync(user.UserId);
            if(result is not null)
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return ResponseHelper<bool>.Ok(true);
            }
            return ResponseHelper<bool>.Fail("Kullanıcı Bulunamadı");
        }
        public async Task<ResponseHelper<bool>> DeleteUserAsync(int id)
        {
            var result = await _context.Users.FindAsync(id);
            if (result is not null)
            {
                _context.Users.Remove(result);
                await _context.SaveChangesAsync();
                return ResponseHelper<bool>.Ok(true);
            }
            return ResponseHelper<bool>.Fail("Kullanıcı Bulunamadı");
        }
    }
}
