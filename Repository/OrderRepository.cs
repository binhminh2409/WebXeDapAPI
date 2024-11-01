using Microsoft.EntityFrameworkCore;
using WebXeDapAPI.Data;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;
using WebXeDapAPI.Repository.Interface;
using WebXeDapAPI.Service;
using static System.Net.Mime.MediaTypeNames;

namespace WebXeDapAPI.Repository
{
    public class OrderRepository : IOrderInterface
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Order GetById(int orderId)
        {
            Order? order = _dbContext.Orders.FirstOrDefault(o => o.Id == orderId);
            return order;
        }

        public List<Order> GetByUser(int userId)
        {
            List<Order>? orders = _dbContext.Orders.Where(o => o.UserID == userId).ToList();
            return orders;
        }

        public Order Update(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order), "Order cannot be null.");
            }

            // Retrieve the existing order from the database
            Order? existingOrder = _dbContext.Orders.FirstOrDefault(o => o.Id == order.Id);
            if (existingOrder == null)
            {
                throw new KeyNotFoundException($"Order with Id {order.Id} not found.");
            }

            // Update the fields of the existing order with the new data
            existingOrder.No_ = order.No_;
            existingOrder.UserID = order.UserID;
            existingOrder.ShipName = order.ShipName;
            existingOrder.ShipAddress = order.ShipAddress;
            existingOrder.ShipEmail = order.ShipEmail;
            existingOrder.ShipPhone = order.ShipPhone;
            existingOrder.Status = order.Status;

            // Mark entity as modified and save changes
            _dbContext.Orders.Update(existingOrder);
            _dbContext.SaveChanges(); 

            // Return the updated order
            return existingOrder;
        }
    }
}
