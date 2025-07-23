using ProductionAndStockERP.Dtos.UserDtos;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Models;

namespace ProductionAndStockERP.Interfaces
{
    public interface IUserService
    {
        Task<ResponseHelper<PagedResponse<UserDto>>> GetAllUsers(UserFilterParameters filters);
        Task<ResponseHelper<UserDto>> GetUserById(int id);
        Task<ResponseHelper<string>> VerificationUser(string email, string passwordhash);

        Task<ResponseHelper<string>> UpdateUserPasswordAsync(int userIdToUpdate, string oldPassword, string newPassword, int performingUserId);
        Task<ResponseHelper<User>> CreateUserAsync(User user, int performingUserId);
        Task<ResponseHelper<User>> UpdateUserAsync(User user, int performingUserId);
        Task<ResponseHelper<bool>> DeleteUserAsync(int id, int performingUserId);
    }
}
