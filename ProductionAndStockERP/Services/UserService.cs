// Dosya: Services/UserService.cs
using Microsoft.EntityFrameworkCore;
using ProductionAndStockERP.Data;
using ProductionAndStockERP.Dtos.UserDtos;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;
using System.Text.Json;

namespace ProductionAndStockERP.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IActivityLogsService _activityLogsService;

        public UserService(ApplicationDbContext context, IActivityLogsService activityLogsService)
        {
            _context = context;
            _activityLogsService = activityLogsService;
        }

        // --- EKSİK METOT 1 EKLENDİ ---
        public async Task<ResponseHelper<string>> VerificationUser(string email, string passwordhash)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return ResponseHelper<string>.Fail("Kullanıcı Adı veya Şifre Hatalı");
            }

            if (!BCrypt.Net.BCrypt.Verify(passwordhash, user.PasswordHash))
            {
                return ResponseHelper<string>.Fail("Kullanıcı Adı veya Şifre Hatalı");
            }
            var result = JwtHelper.GenerateJwtToken(user.UserName, user.UserId, user.Role);
            return ResponseHelper<string>.Ok(result);
        }

        // --- EKSİK METOT 2 EKLENDİ (DTO Dönecek Şekilde) ---
        public async Task<ResponseHelper<UserDto>> GetUserById(int id)
        {
            var user = await _context.Users
                .Where(u => u.UserId == id)
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    Email = u.Email,
                    Role = u.Role
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return ResponseHelper<UserDto>.Fail("Kullanıcı Bulunamadı");
            }
            return ResponseHelper<UserDto>.Ok(user);
        }

        // --- 'CreateAsync' HATASI DÜZELTİLDİ ---
        public async Task<ResponseHelper<PagedResponse<UserDto>>> GetAllUsers(UserFilterParameters filters)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(filters.Role))
                query = query.Where(u => u.Role == filters.Role);
            if (!string.IsNullOrEmpty(filters.SearchTerm))
                query = query.Where(u => u.UserName.Contains(filters.SearchTerm) || u.Email.Contains(filters.SearchTerm));

            var dtoQuery = query.OrderBy(u => u.UserName).Select(u => new UserDto
            {
                UserId = u.UserId,
                UserName = u.UserName,
                Email = u.Email,
                Role = u.Role
            });

            // DÜZELTME: Sayfalama mantığı constructor kullanacak şekilde yenilendi.
            var totalRecords = await dtoQuery.CountAsync();
            var items = await dtoQuery.Skip((filters.PageNumber - 1) * filters.PageSize).Take(filters.PageSize).ToListAsync();
            var pagedResponse = new PagedResponse<UserDto>(items, filters.PageNumber, filters.PageSize, totalRecords);

            return ResponseHelper<PagedResponse<UserDto>>.Ok(pagedResponse);
        }


        public async Task<ResponseHelper<User>> CreateUserAsync(User user, int performingUserId)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            await _activityLogsService.AddLogAsync(performingUserId, "Yeni kullanıcı oluşturuldu.", "Başarılı", "User", user.UserId.ToString());
            return ResponseHelper<User>.Ok(user);
        }

        public async Task<ResponseHelper<User>> UpdateUserAsync(User updatedUser, int performingUserId)
        {
            var existingUser = await _context.Users.FindAsync(updatedUser.UserId);
            if (existingUser == null) return ResponseHelper<User>.Fail("Güncellenecek kullanıcı bulunamadı.");

            var changes = new Dictionary<string, object>();
            if (existingUser.UserName != updatedUser.UserName) changes["UserName"] = new { Old = existingUser.UserName, New = updatedUser.UserName };
            if (existingUser.Email != updatedUser.Email) changes["Email"] = new { Old = existingUser.Email, New = updatedUser.Email };
            if (existingUser.Role != updatedUser.Role) changes["Role"] = new { Old = existingUser.Role, New = updatedUser.Role };

            existingUser.UserName = updatedUser.UserName;
            existingUser.Email = updatedUser.Email;
            existingUser.Role = updatedUser.Role;
            await _context.SaveChangesAsync();

            string changesJson = changes.Count > 0 ? JsonSerializer.Serialize(changes) : null;
            await _activityLogsService.AddLogAsync(performingUserId, "Kullanıcı bilgileri güncellendi.", "Başarılı", "User", existingUser.UserId.ToString(), changesJson);

            return ResponseHelper<User>.Ok(existingUser);
        }

        public async Task<ResponseHelper<bool>> DeleteUserAsync(int id, int performingUserId)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                await _activityLogsService.AddLogAsync(performingUserId, $"ID'si {id} olan kullanıcıyı silme denemesi başarısız.", "Başarısız", "User", id.ToString());
                return ResponseHelper<bool>.Fail("Kullanıcı bulunamadı.");
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            await _activityLogsService.AddLogAsync(performingUserId, $"'{user.UserName}' adlı kullanıcı silindi.", "Başarılı", "User", id.ToString());
            return ResponseHelper<bool>.Ok(true);
        }

        public async Task<ResponseHelper<string>> UpdateUserPasswordAsync(int userIdToUpdate, string oldPasswordPlain, string newPasswordPlain, int performingUserId)
        {
            var user = await _context.Users.FindAsync(userIdToUpdate);
            if (user == null) return ResponseHelper<string>.Fail("Kullanıcı bulunamadı.");

            if (!BCrypt.Net.BCrypt.Verify(oldPasswordPlain, user.PasswordHash))
            {
                await _activityLogsService.AddLogAsync(performingUserId, "Şifre değiştirme denemesi (yanlış eski şifre).", "Başarısız", "User", userIdToUpdate.ToString());
                return ResponseHelper<string>.Fail("Mevcut şifre yanlış.");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPasswordPlain);
            await _context.SaveChangesAsync();
            await _activityLogsService.AddLogAsync(performingUserId, "Kullanıcı şifresini güncelledi.", "Başarılı", "User", userIdToUpdate.ToString());
            return ResponseHelper<string>.Ok("Şifre başarıyla değiştirildi.");
        }
    }
}