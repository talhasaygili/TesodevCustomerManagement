using System.Net;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using OrderMicroservice.Entities.DataToObjects;
using Shared.DataAccess.Abstract;
using Shared.Entities.DataToObjects;
using Core.Enums;

namespace OrderMicroservice.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
	{
		private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
			_orderRepository = orderRepository;
        }

        [HttpPost, Route("api/", Name = "[controller]/create")]
        public IActionResult Create(OrderDTO order)
		{
			// Validate customer
			if (!ModelState.IsValid)
			{
                var message = string.Join("\n", ModelState.Values
									.SelectMany(v => v.Errors)
									.Select(e => e.ErrorMessage));
                return BadRequest(message);
			}
			DateTime now = DateTime.UtcNow;

			order.Status = OrderStatus.Pending.ToString();
			order.CreatedAt = now;
			order.UpdatedAt = now;


			Order? result = order.ToOrder();

			try
			{
				_orderRepository.Add(result);
			}
			catch (Exception ex)
			{
				return Forbid(ex.Message);
			}
			return Ok(result.Id);
			
		}

        [HttpGet, Route("api/", Name = "[controller]/getAllOrders")]
        public IActionResult Get()
		{
            if (!ModelState.IsValid)
            {
                var message = string.Join("\n", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage));
                return BadRequest(message);
            }

            return Ok(_orderRepository.GetList(x => true));
		}

        [HttpGet, Route("api/order", Name = "[controller]/getOrderById")]
        public IActionResult GetOrderById(IdDTO requestBody)
		{
            if (!ModelState.IsValid)
            {
                var message = string.Join("\n", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage));
                return BadRequest(message);
            }

            var order = _orderRepository.Get(x => x.Id == requestBody.Id);
			if(order == null)
			{
				return NotFound();
			}
			return Ok(order);
		}

        [HttpDelete, Route("api/", Name = "[controller]/delete")]
		public IActionResult Delete(IdDTO requestBody)
		{
            if (!ModelState.IsValid)
            {
                var message = string.Join("\n", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage));
                return BadRequest(message);
            }

            var order = _orderRepository.Get(x => x.Id == requestBody.Id);
            if (order == null)
			{
				return NotFound();
			}

			_orderRepository.Delete(order);
			return Ok(true);
		}

        [HttpPut, Route("api/", Name = "[controller]/update")]
        public IActionResult Update(OrderDTO updatedOrder)
		{
            if (!ModelState.IsValid)
            {
                var message = string.Join("\n", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage));
                return BadRequest(message);
            }

            var order = _orderRepository.Get(x => x.Id == updatedOrder.Id);
			if(order == null)
			{
				return NotFound();
			}

			order.Quantity = updatedOrder.Quantity;
			order.Price = updatedOrder.Price;
			order.Status = updatedOrder.Status;
			order.AddressId = updatedOrder.AddressId;
			order.ProductId = updatedOrder.ProductId;
			order.UpdatedAt = DateTime.UtcNow;

			_orderRepository.Update(order);

			return Ok(true);
		}

        [HttpPost, Route("api/change-status", Name = "[controller]/changeStatus")]
        public IActionResult ChangeStatus(ChangeStatusDTO request)
		{
            if (!ModelState.IsValid)
            {
                var message = string.Join("\n", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage));
                return BadRequest(message);
            }

            var order = _orderRepository.Get(x => x.Id == request.Id);
			if(order == null)
			{
				return NotFound();
			}

			if (string.IsNullOrEmpty(request.Status))
			{
				return BadRequest("Status field is requied");
			}
			List<string> status = Enum.GetNames<OrderStatus>().ToList();
			if (status.Contains(request.Status.ToString()))
			{
                order.Status = request.Status;
			}
			else
			{
				return BadRequest("Please type valid status");
			}

			order.UpdatedAt = DateTime.UtcNow;

			_orderRepository.Update(order);

			return Ok(true);
		}

        [HttpGet, Route("api/customer", Name = "[controller]/getOrdersByCustomerId")]
        public IActionResult GetOrdersByCustomerId(IdDTO requestBody)
		{
            if (!ModelState.IsValid)
            {
                var message = string.Join("\n", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage));
                return BadRequest(message);
            }

            var customerOrders = _orderRepository.GetList(x => x.CustomerId == requestBody.Id);

			if(customerOrders.Count == 0)
			{
				return NotFound("Customer Order Not Found");
			}

			return Ok(customerOrders);
		}

	}
}

