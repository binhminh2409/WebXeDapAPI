using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebXeDapAPI.Data;
using WebXeDapAPI.Models;
using WebXeDapAPI.Repository.Interface;

namespace WebXeDapAPI.Repository
{
    public class DeliveryRepository : IDeliveryInterface
    {
        private readonly ApplicationDbContext _dbContext;

        public DeliveryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Delivery> CreateAsync(Delivery delivery)
        {
            await _dbContext.Deliveries.AddAsync(delivery);
            await _dbContext.SaveChangesAsync();
            return delivery;
        }

        public async Task<List<Delivery>> GetAll()
        {
            return await _dbContext.Deliveries
                                    .Include(d => d.Payment)
                                    .ToListAsync();
        }

        public async Task<Delivery> GetByIdAsync(int deliveryId)
        {
            return await _dbContext.Deliveries
                                    .Include(d => d.Payment)
                                    .FirstOrDefaultAsync(d => d.Id == deliveryId);
        }

        public async Task<Delivery> GetByPaymentId(int paymentId)
        {
            return await _dbContext.Deliveries
                                    .Include(d => d.Payment)
                                    .FirstOrDefaultAsync(d => d.Payment.Id == paymentId); // Assuming Delivery has a PaymentId property
        }

        public async Task<List<Delivery>> GetByUserAsync(int userId)
        {
            return await _dbContext.Deliveries
                .Where(d => d.UserId == userId)
                .Include(d => d.Payment)
                .ToListAsync();
        }

        public async Task<Delivery> UpdateAsync(Delivery delivery)
        {
            _dbContext.Deliveries.Update(delivery);
            await _dbContext.SaveChangesAsync();

            // Load the Payment related entity before returning
            return await _dbContext.Deliveries
                .Include(d => d.Payment)
                .FirstOrDefaultAsync(d => d.Id == delivery.Id);
        }
    }
}
