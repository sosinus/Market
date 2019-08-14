using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Market.Models;
using Market.Models.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult AddOrder(OrderItem[] orderItems)
        {
            string userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
            int result = _marketUoW.UseOrderRepository().AddOrder(orderItems, userId).Result;
            if (result > 0)
                return Ok();
            else
                return BadRequest();
        }

        [HttpGet]
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
        public IActionResult UpdateOrder(Order order)
        {
          var success =  _marketUoW.UseOrderRepository().UpdateOrder(order).Result;
            if (success)
                return Ok();
            else
                return BadRequest();
        }

    }
}