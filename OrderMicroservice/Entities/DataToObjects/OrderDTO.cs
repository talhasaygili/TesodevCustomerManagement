using System;
using Entities.Concrete;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace OrderMicroservice.Entities.DataToObjects
{
    public class OrderDTO
	{
        [FromBody]
        public Guid Id { get; set; }

        [FromBody]
        [Required]
        public int Quantity { get; set; }

        [FromBody]
        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [FromBody]
        [Required]
        [MinLength(3, ErrorMessage = "Status needs to be longer than 3 characters")]
        public string Status { get; set; }

        [FromBody]
        [Required]
        public DateTime CreatedAt { get; set; }

        [FromBody]
        [Required]
        public DateTime UpdatedAt { get; set; }


        [FromBody]
        [Required]
        public Guid CustomerId { get; set; }

        [FromBody]
        [Required]
        public Guid AddressId { get; set; }

        [FromBody]
        [Required]
        public Guid ProductId { get; set; }


        public Order ToOrder()
        {
            return new Order()
            {
                Id = Id,
                Quantity = Quantity,
                Price = Price,
                Status = Status,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,

                CustomerId = CustomerId,
                AddressId = AddressId,
                ProductId = ProductId
            };
        }
    }


}

