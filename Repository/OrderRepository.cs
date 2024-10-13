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
            throw new NotImplementedException();
        }
    }
}
