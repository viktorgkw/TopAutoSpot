namespace TopAutoSpot.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Represents a user in the application.
    /// Inherits from the IdentityUser class.
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// The first name of the user.
        /// Can be null.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// The last name of the user.
        /// Can be null.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// The role of the user.
        /// Cannot be null.
        /// </summary>
        public string Role { get; set; } = null!;
    }
}
