using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.Views.Utilities
{
    public static class NotificationServices
    {
        public static async Task<bool> RemoveNotification(
            ApplicationDbContext _context, string notificationId)
        {
            var foundNotification = await _context.Notifications
                .FirstAsync(n => n.Id == notificationId);

            if (foundNotification != null)
            {
                _context.Notifications.Remove(foundNotification);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public static async Task<Notification> Get(
            ApplicationDbContext _context, string notificationId, string userName)
        {
            if (ValidateProperties(new string[] { notificationId }) == false)
            {
                return null;
            }

            var foundNotification = _context.Notifications
                .FirstOrDefault(n => n.Id == notificationId);

            if (foundNotification != null)
            {
                var foundUser = _context.Users.First(u => u.UserName == userName);

                if (foundUser != null && foundUser.Id == foundNotification.To) 
                {
                    return foundNotification;
                }
            }

            return null;
        }

        public static async Task<List<Notification>> GetAll
            (ApplicationDbContext _context, string userId)
        {
            return await _context.Notifications
                .Where(n => n.To == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public static async Task<bool> Send(ApplicationDbContext _context, string from, string to, string title, string description)
        {
            if (ValidateProperties(new string[] { from, to, title, description }) == false)
            {
                return false;
            }

            var fromUser = _context.Users
                .FirstOrDefault(u => u.Id == from);

            if (fromUser == null)
            {
                return false;
            }

            var toUser = _context.Users
                .FirstOrDefault(u => u.Id == to);

            if (toUser == null)
            {
                return false;
            }

            Notification newNotification = new Notification()
            {
                Id = Guid.NewGuid().ToString(),
                From = fromUser.Id,
                To = toUser.Id,
                Title = title,
                Description = description,
                CreatedAt = DateTime.Now
            };

            _context.Notifications.Add(newNotification);
            await _context.SaveChangesAsync();

            return true;
        }

        public static async Task<string> GetFromUsername(ApplicationDbContext _context, string from)
        {
            return _context.Users.First(u => u.Id == from).UserName;
        }

        private static bool ValidateProperties(string[] props)
        {
            foreach (var prop in props)
            {
                if (string.IsNullOrEmpty(prop) || string.IsNullOrWhiteSpace(prop))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
