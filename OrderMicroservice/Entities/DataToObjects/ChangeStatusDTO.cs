using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace OrderMicroservice.Entities.DataToObjects
{
    public class ChangeStatusDTO
    {
        [Required]
        [FromBody]
        public Guid Id { get; set; }
        
        [FromBody]
        [Required]
        [MinLength(3, ErrorMessage = "Status needs to be longer than 3 characters")]
        public string Status { get; set; }
    }
}
