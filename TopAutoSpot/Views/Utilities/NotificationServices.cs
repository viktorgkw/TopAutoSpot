using TopAutoSpot.Data;
using TopAutoSpot.Models;

namespace TopAutoSpot.Views.Utilities
{
    public static class NotificationServices
    {
        public static bool RemoveNotification(
            ApplicationDbContext _context, string notificationId, string userName)
        {
            if (ValidateProperties(new string[] { notificationId, userName }) == false)
            {
                return false;
            }

            Notification foundNotification = _context.Notifications
                .First(n => n.Id == notificationId);

            User foundUser = _context.Users.First(u => u.UserName == userName);

            if (foundNotification != null && foundUser != null && foundUser.Id == foundNotification.To)
            {
                _context.Notifications.Remove(foundNotification);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public static Notification Get(
            ApplicationDbContext _context, string notificationId, string userName)
        {
            if (ValidateProperties(new string[] { notificationId }) == false)
            {
                return null;
            }

            Notification? foundNotification = _context.Notifications
                .FirstOrDefault(n => n.Id == notificationId);

            if (foundNotification != null)
            {
                User foundUser = _context.Users.First(u => u.UserName == userName);

                if (foundUser != null && foundUser.Id == foundNotification.To)
                {
                    return foundNotification;
                }
            }

            return null;
        }

        public static List<Notification> GetAll
            (ApplicationDbContext _context, string userId)
        {
            return _context.Notifications
                .Where(n => n.To == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToList();
        }

        public static bool Send(ApplicationDbContext _context, string from, string to, string title, string description)
        {
            if (ValidateProperties(new string[] { from, to, title, description }) == false)
            {
                return false;
            }

            User? fromUser = _context.Users
                .FirstOrDefault(u => u.Id == from);

            if (fromUser == null)
            {
                return false;
            }

            User? toUser = _context.Users
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
            _context.SaveChanges();

            return true;
        }

        public static string GetFromUsername(ApplicationDbContext _context, string from)
        {
            return _context.Users.First(u => u.Id == from).UserName;
        }

        private static bool ValidateProperties(string[] props)
        {
            foreach (string prop in props)
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
