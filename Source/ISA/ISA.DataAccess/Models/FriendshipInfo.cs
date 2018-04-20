using ISA.Common.Extensions;
using ISA.DataAccess.Models.Enumerations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.DataAccess.Models
{
    public class FriendshipInfo : BaseEntity<int>
    {
        [ForeignKey("Friend1")]
        public int Friend1Id { get; set; }
        public UserProfile Friend1 { get; set; }

        [ForeignKey("Friend2")]
        public int Friend2Id { get; set; }
        public UserProfile Friend2 { get; set; }

        [Column("Status")]
        public string FriendshipStatusText
        {
            get => Status.ToString();
            set => Status = value.ParseEnum<FriendshipStatus>();
        }

        [NotMapped]
        public FriendshipStatus Status { get; set; }

        [InverseProperty("Owner")]
        public int FriendListId { get; set; }
        public FriendList FriendList { get; set; }
        //public UserProfile ModifiedBy {get;set;} To know who last updated the status
    }

}
