using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.DataAccess;

namespace Entities.Concrete
{
    public class Order : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        public Guid CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        public Guid AddressId { get; set; }
        [ForeignKey("AddressId")]
        public virtual Address? Address { get; set; }

        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}

