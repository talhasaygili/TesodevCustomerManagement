using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.DataAccess;

namespace Entities.Concrete
{
    public class Product : IEntity
    {
        public Product()
        {
            Orders = new HashSet<Order>();
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string ImageUrl { get; set; }

        public string Name { get; set; }


        public virtual ICollection<Order> Orders { get; set; }
    }
}
