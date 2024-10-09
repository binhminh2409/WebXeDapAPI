using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Service.Interfaces
{
    public interface IPaymentIService
    {
        public Task<PaymentDto> CreateAsync(PaymentDto dto, User user, Order order);
    }
}
