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

        List<Order_Details> IOrderDetailsInterface.GetByUser(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
