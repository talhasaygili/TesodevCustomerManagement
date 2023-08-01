using System;
using Microsoft.AspNetCore.Mvc;
using Entities.Concrete;
using Shared.DataAccess.Abstract;
using Shared.DataAccess.Concrete.EntityFramework;
using CustomerMicroservice.Entities.DataToObjects;
using Shared.Entities.DataToObjects;

namespace CustomerMicroservice.Controllers
{
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpPost, Route("api/", Name = "api/Create")]
        public IActionResult Create(CustomerDTO customer)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join("\n", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage));
                return BadRequest(message);
            }
            DateTime now = DateTime.UtcNow;

            customer.CreatedAt = now;
            customer.UpdatedAt = now;

            Customer? result = customer.ToCustomer();

            try
            {
                _customerRepository.Add(result);
            }
            catch (Exception ex)
            {
                return Forbid(ex.Message);
            }
            return Ok(result.Id);

        }


        [HttpGet, Route("api/", Name = "api/Get")]
        public IActionResult Get()
        {

            return Ok(_customerRepository.GetList(x => true));
        }


        [HttpGet, Route("api/customer", Name = "[controller]/GetCustomerById")]
        public IActionResult GetCustomerById(IdDTO requestBody)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join("\n", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage));
                return BadRequest(message);
            }

            var customer = _customerRepository.Get(x => x.Id == requestBody.Id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpGet, Route("api/validate", Name = "[controller]/Validate")]
        public IActionResult Validate(IdDTO requestBody)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join("\n", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage));
                return BadRequest(message);
            }

            var customer = _customerRepository.Get(x => x.Id == requestBody.Id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(true);
        }

        [HttpPut, Route("api/", Name = "[controller]/Update")]
        public IActionResult Update(CustomerDTO updatedCustomer)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join("\n", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage));
                return BadRequest(message);
            }

            var customer = _customerRepository.Get(x => x.Id == updatedCustomer.Id);
            if (customer == null)
            {
                return NotFound();
            }

            customer.Name = updatedCustomer.Name;
            customer.Email = updatedCustomer.Email;
            customer.AddressId = updatedCustomer.AddressId;
            customer.UpdatedAt = DateTime.UtcNow;

            _customerRepository.Update(customer);

            return Ok(true);
        }


        [HttpDelete, Route("api/", Name = "[controller]/Delete")]
        public IActionResult Delete(IdDTO requestBody)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join("\n", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage));
                return BadRequest(message);
            }

            var customer = _customerRepository.Get(x => x.Id == requestBody.Id);
            if (customer == null)
            {
                return NotFound();
            }

            _customerRepository.Delete(customer);
            return Ok(true);
        }

    }
}
