using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.DataAccess;

namespace Entities.Concrete
{
    public class Address : IEntity
    {
        public Address()
        {
            Orders = new HashSet<Order>();
            Customers = new HashSet<Customer>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string AddressLine { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public int CityCode { get; set; }



        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
