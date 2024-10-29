using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Repository.Interface
{
    public interface IPaymentInterface
    {
        Task<List<Payment>> GetByUserAsync(int userId);
        
        Task<Payment> GetByIdAsync(int paymentId);

        Task<Payment> CreateAsync(Payment payment);

        Task<List<Payment>> GetAll();

        Task<List<Payment>> GetInTimeRange(DateTime startTime, DateTime endTime);
    }
}
