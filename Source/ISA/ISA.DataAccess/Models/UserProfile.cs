using ISA.Common.Extensions;
using ISA.DataAccess.Models.Enumerations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.DataAccess.Models
{
    public class UserProfile : BaseEntity<int>
    {
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string TelephoneNr { get; set; }

        //[InverseProperty("Owner")]
        //public int? FriendListId { get; set; }
        //public FriendList FriendList { get; set; }

        //[InverseProperty("Friends")]
        //public virtual ICollection<FriendList> FriendOf { get; set; }

        [InverseProperty("Receiver")]
        public virtual ICollection<FriendRequest> ReceivedRequests { get; set; }

        [InverseProperty("Sender")]
        public virtual ICollection<FriendRequest> SentRequests { get; set; }
    }

    public class FriendRequest : BaseEntity<int>
    {
        public int SenderId { get; set; }
        public UserProfile Sender { get; set; }

        public int ReceiverId { get; set; }
        public UserProfile Receiver { get; set; }

        [Column("Status")]
        public string StatusString
        {
            get => Status.ToString();
            set => Status = value.ParseEnum<FriendshipStatus>();
        }

        [NotMapped]
        public FriendshipStatus Status { get; set; }
    }
}
