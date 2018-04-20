using ISA.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public int? UserProfileId { get; set; }

        [NotMapped]
        public UserProfile Profile { get; set; }
    }
}
