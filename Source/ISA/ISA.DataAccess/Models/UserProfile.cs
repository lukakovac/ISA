namespace ISA.DataAccess.Models
{
    public class UserProfile : BaseEntity<int>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }
    }
}
