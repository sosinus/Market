using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.RepositoryResults;
using Models.Tables;
using System.IO;
using System.Linq;
using UnitsOfWork;

namespace Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _marketUoW;
        public OrderController(IUnitOfWork marketUoW)
        {
            _marketUoW = marketUoW;
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult AddOrder(OrderItem[] orderItems)
        {
            string userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
            AddOrderResult result = _marketUoW.UseOrderRepository().AddOrder(orderItems, userId).Result;
            return Ok(result);

        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult GetOrdersForUser()
        {
            if (User.Claims.SingleOrDefault(c => c.Type == "UserID") != null)
            {
                string userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;

                Order[] orders = _marketUoW.UseOrderRepository().GetOrdersForUser(userId);

                foreach (var order in orders)
                {
                    foreach (var orderItem in order.OrderItems)
                    {
                        string folderName = Path.Combine("Resources", "Images", orderItem.Item.Id.ToString());
                        orderItem.Item.Image = Directory.GetFiles(folderName)[0];
                    }

                }
                return Ok(orders);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("OrderList")]
        [Authorize(Roles = "Manager")]
        public IActionResult GetOrdersForManager()
        {
            Order[] orders = _marketUoW.UseOrderRepository().GetAllOrders();

            foreach (var order in orders)
            {
                foreach (var orderItem in order.OrderItems)
                {
                    string folderName = Path.Combine("Resources", "Images", orderItem.Item.Id.ToString());
                    orderItem.Item.Image = Directory.GetFiles(folderName)[0];
                }

            }
            return Ok(orders);

        }

        [HttpPut]
        [Authorize(Roles = "Manager")]
        public IActionResult UpdateOrder(Order order)
        {
            var success = _marketUoW.UseOrderRepository().UpdateOrder(order).Result;
            if (success)
                return Ok();
            else
                return BadRequest();
        }

        [HttpPost]
        [Route("Cancel")]
        [Authorize(Roles = "User")]
        public IActionResult CancelOrder(Order order)
        {
            order.Status = "Отменен";
            var success = _marketUoW.UseOrderRepository().UpdateOrder(order).Result;
            if (success)
                return Ok();
            else
                return BadRequest();
        }

    }
}