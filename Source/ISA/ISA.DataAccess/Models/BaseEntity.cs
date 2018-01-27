using System.ComponentModel.DataAnnotations;

namespace ISA.DataAccess.Models
{
    public class BaseEntity<T> : Entity
    {
        [Key]
        public virtual T Id { get; set; }
    }
}
