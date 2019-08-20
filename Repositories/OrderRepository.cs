using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Registration;
using Models.RepositoryResults;
using Models.Tables;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IOrderRepository
    {
        Task<AddOrderResult> AddOrder(OrderItem[] orderItems, string userId);
        Order[] GetOrdersForUser(string userId);
        Order[] GetAllOrders();
        Task<bool> UpdateOrder(Order order);
    }
    public class OrderRepository : IOrderRepository
    {
        private readonly MarketDBContext _context;
        private readonly UserManager<AppUser> _userManager;


        public OrderRepository(MarketDBContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<AddOrderResult> AddOrder(OrderItem[] orderItems, string userId)
        {
            AddOrderResult result = new AddOrderResult();
            AppUser user = _userManager.Users.SingleOrDefault(u => u.Id == userId);
            Customer customer = _context.Customers.SingleOrDefault(c => c.Id == user.Customer_Id);
            if (user.Customer_Id != null && orderItems.Length > 0)
            {
                Order order = new Order()
                {
                    Customer_Id = user.Customer_Id.Value,
                    Order_Date = DateTime.Now.Date,
                    Status = "Новый"
                };

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                order = _context.Orders.Last();
                int orderId = order.Id;

                foreach (var orderItem in orderItems)
                {
                    orderItem.Order_Id = orderId;
                    orderItem.Item = null;
                    var discount = customer.Discount;
                    var price = orderItem.Item_Price;
                    orderItem.Item_Price = price - (orderItem.Item_Price * discount) / 100;
                }

                await _context.OrderItems.AddRangeAsync(orderItems);
                int SaveChangesResult = await _context.SaveChangesAsync();
                if (SaveChangesResult > 0)
                    result.Success = true;
                
            }
            return result;
        }

        public Order[] GetOrdersForUser(string userId)
        {
            var customerId = _userManager.Users.SingleOrDefault(u => u.Id == userId).Customer_Id;
            var arr = _context.Orders.Where(o => o.Customer_Id == customerId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .ToArray();
            return arr;
        }

        public Order[] GetAllOrders()
        {

            var arr = _context.Orders

                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .ToArray();
            return arr;
        }

        public async Task<bool> UpdateOrder(Order order)
        {
            foreach (var orderItem in order.OrderItems)
            {
                orderItem.Id = default;
                orderItem.Item = null;
                orderItem.Order = null;
            }
            var orderItems = _context.OrderItems.Where(o => o.Order_Id == order.Id).ToList();
            foreach (var orderIt in orderItems)
            {
                orderIt.Order = null;
            }
            _context.OrderItems.RemoveRange(orderItems);
            await _context.SaveChangesAsync();
            _context.Orders.Update(order);
            int res = await _context.SaveChangesAsync();
            if (res > 0)
                return true;
            else
                return false;

        }

    }
}
