using System;
using System.Collections.Generic;
using System.Text;

namespace ISA.DataAccess.Models
{
    public class BaseEntity<T> : Entity
    {
        public virtual T Id { get; set; }
    }
}
