using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ISA.Models.ManageViewModels
{
    public class IndexViewModel : UserProfileCommon
    {
        public bool HasPassword { get; set; }

        public IList<UserLoginInfo> Logins { get; set; }

        //IGNORE, LEAVE DEFAULT IMPLEMENTATION
        public string PhoneNumber { get; set; }

        public bool TwoFactor { get; set; }

        public bool BrowserRemembered { get; set; }

        /* =========== USER PROFILE STUFF =============== */
        #region USER PROFILE
        // NOW INHERITED PROPS

        public List<FriendViewModel> Friends { get; set; }

        #endregion
    }

    public class FriendViewModel : UserProfileCommon
    {
        public int Id { get; set; }
    }

    public class UserProfileCommon
    {
        public string EmailAddress { get; set; }

        public string City { get; set; }

        public string TelephoneNr { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

    }
}
