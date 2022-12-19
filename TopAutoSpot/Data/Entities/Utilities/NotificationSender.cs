namespace TopAutoSpot.Data.Entities.Utilities
{
    public static class NotificationSender
    {
        public static bool Send(ApplicationDbContext _context, string from, string to, string title, string description)
        {
            if (ValidateProperties(new string[] { from, to, title, description }) == false)
            {
                return false;
            }

            var fromUser = _context.Users.FirstOrDefault(u => u.Id == from);

            if (fromUser == null)
            {
                return false;
            }

            var toUser = _context.Users.FirstOrDefault(u => u.Id == to);

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
                Description = description
            };

            _context.Notifications.Add(newNotification);

            toUser.Notifications.Add(newNotification);

            return true;
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
