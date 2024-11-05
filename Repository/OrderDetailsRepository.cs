using Microsoft.EntityFrameworkCore;
using WebXeDapAPI.Data;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;
using WebXeDapAPI.Repository.Interface;
using WebXeDapAPI.Service;
using static System.Net.Mime.MediaTypeNames;

namespace WebXeDapAPI.Repository
{
    public class OrderDetailsRepository : IOrderDetailsInterface
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderDetailsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        Order_Details IOrderDetailsInterface.GetByOrderId(string orderNo)
        {
            Order_Details? order_Details = _dbContext.Order_Details.FirstOrDefault(od => od.OrderID == orderNo);
            return order_Details;
        }

        public List<Order_Details> GetAllByOrderId(string orderNo)
        {
            List<Order_Details> order_Details = _dbContext.Order_Details
                                                    .Where(od => od.OrderID == orderNo)
                                                    .ToList();
            return order_Details;
        }


        public List<Order_Details> GetByUser(int userId)
        {
            // Fetch orders for the user in a single query
            var orders = _dbContext.Orders
                .Where(o => o.UserID == userId)
                .ToList();

            // Create a list to hold order details
            List<Order_Details> orderDetails = new();

            // If there are orders, fetch their details in a single query
            if (orders.Any())
            {
                // Collect all order IDs
                var orderIds = orders.Select(o => o.No_).ToList();

                // Fetch all order details that match the order IDs
                orderDetails = _dbContext.Order_Details
                    .Where(od => orderIds.Contains(od.OrderID))
                    .ToList();
            }

            return orderDetails;
        }

    }
}
