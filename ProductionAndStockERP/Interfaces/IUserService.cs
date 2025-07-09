using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Models;

namespace ProductionAndStockERP.Interfaces
{
    public interface IUserService
    {
        Task<ResponseHelper<IEnumerable<User>>> GetAllUsers();
        Task<ResponseHelper<User>> GetUserById(int id);

        Task<ResponseHelper<string>> VerificationUser(string userName,string passwordhash);

        Task<ResponseHelper<bool>> CreateUserAsync(User user);
        Task<ResponseHelper<bool>> UpdateUserAsync(User user);
        Task<ResponseHelper<bool>> DeleteUserAsync(int id);
    }
}
