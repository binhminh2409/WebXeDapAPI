using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Repository.Interface
{
    public interface IPaymentInterface
    {
        Task<List<Payment>> GetByUserAsync(int userId);
        Task<Payment> GetByIdAsync(int paymentId);

        Task<Payment> CreateAsync(Payment payment);
    }
}
