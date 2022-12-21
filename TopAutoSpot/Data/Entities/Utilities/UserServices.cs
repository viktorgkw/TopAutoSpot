using Microsoft.EntityFrameworkCore;

namespace TopAutoSpot.Data.Entities.Utilities
{
    public static class UserServices
    {
        public static async Task<User> GetUserByName(ApplicationDbContext _context, string username)
        {
            return await _context.Users.FirstAsync(u => u.UserName == username);
        }
    }
}
