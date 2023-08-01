using System;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Shared.Entities.DataToObjects
{
	public class IdDTO
	{
        [Required(ErrorMessage ="Id Required")]
        [FromBody]
        public Guid Id { get; set; }
	}
}
