using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.DataAccess.Models
{
    public class BaseEntity<T> : Entity
    {
        [Key]
        public virtual T Id { get; set; }
    }
}
