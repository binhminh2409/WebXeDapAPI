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

        public async Task<List<Payment>> GetAll()
        {
            List<Payment> payments = await _dbContext.Payments
                .Include(p => p.User)
                .Include(p => p.Order)
                .ToListAsync();
            return payments;
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
                    .Include(p => p.User)
                    .Include(p => p.Order)
                    .Where(p => p.User.Id == userId)
                    .ToListAsync();

            return payments;   
        }

        public async Task<List<Payment>> GetInTimeRange(DateTime startTime, DateTime endTime)
        {
            List<Payment> payments = await _dbContext.Payments
                .Where(p => p.CreatedTime >= startTime && p.CreatedTime <= endTime)
                .Include(p => p.User)  
                .Include(p => p.Order) 
                .ToListAsync();

            return payments;
        }
    }
}
