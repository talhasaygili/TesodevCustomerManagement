using System;
using Entities.Concrete;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace CustomerMicroservice.Entities.DataToObjects
{
	public class CustomerDTO
	{
        [FromBody]
        public Guid Id { get; set; }

        [FromBody]
        [Required]
        public string Name { get; set; }

        [FromBody]
        [Required]
        public string Email { get; set; }

        [FromBody]
        [Required]
        public DateTime UpdatedAt { get; set; }

        [FromBody]
        [Required]
        public DateTime CreatedAt { get; set; }

        [FromBody]
        [Required]
        public Guid AddressId { get; set; }

        public Customer ToCustomer()
        {
            return new Customer()
            {
                Id = Id,
                Name = Name,
                Email = Email,
                UpdatedAt = UpdatedAt,
                CreatedAt = CreatedAt,
                AddressId = AddressId

            };
        }
    }


}

