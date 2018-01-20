namespace ISA.DataAccess.Models
{
    public class BaseEntity<T> : Entity
    {
        public virtual T Id { get; set; }
    }
}
