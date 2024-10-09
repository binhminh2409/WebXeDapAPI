using Microsoft.EntityFrameworkCore;
using WebXeDapAPI.Data;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;
using WebXeDapAPI.Repository.Interface;

namespace WebXeDapAPI.Repository
{
    public class PaymentRepository : IPaymentInterface
    {
        private readonly ApplicationDbContext _dbContext;
        
        public PaymentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Payment> CreateAsync(Payment payment)
        {
            await _dbContext.Payments.AddAsync(payment);
            await _dbContext.SaveChangesAsync();
            return payment;
        }


        public async Task<Payment> GetByIdAsync(int paymentId)
        {
            Payment? payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);
            return payment;        
        }

        public async Task<List<Payment>> GetByUserAsync(int userId)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null){
                return null;
            }
                List<Payment> payments = await _dbContext.Payments
                .Where(p => p.User.Id == userId)
                .ToListAsync();

            return payments;   
        }
    }
}
