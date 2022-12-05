using Microsoft.AspNetCore.Identity;

namespace TopAutoSpot.Data.Entities
{
    public class User : IdentityUser
    {
        // Id, Email, Password, Username and PhoneNumber come from IdentityUser
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public List<Listing> Listings { get; set; }
    }
}
