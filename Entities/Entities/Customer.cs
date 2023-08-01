using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.DataAccess;
using Entities.Concrete;

namespace Entities.Concrete
{
    public class Customer : IEntity
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }




        public Guid AddressId { get; set; }
        [ForeignKey("AddressId")]
        public virtual Address? Address { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}





