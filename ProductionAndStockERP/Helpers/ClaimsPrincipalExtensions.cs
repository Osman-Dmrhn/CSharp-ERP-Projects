using System.Security.Claims;

namespace ProductionAndStockERP.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static int? GetUserId(this ClaimsPrincipal user)
        {
            if (user == null)
            {
                return null;
            }

            var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            return null;
        }
    }
}
