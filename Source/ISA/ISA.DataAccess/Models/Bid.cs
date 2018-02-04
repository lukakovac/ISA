namespace ISA.DataAccess.Models
{
    public class Bid : BaseEntity<int>
    {
        public UserProfile User { get; set; }

        public ThematicProps ThematicProp { get; set; }

        public double Price { get; set; }
    }
}
