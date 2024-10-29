using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Service.Interfaces
{
    public interface IPaymentIService
    {
        public Task<PaymentDto> CreateAsync(PaymentDto dto);

        public Task<List<PaymentDto>> FindAll();

        public Task<List<PaymentDto>> FindByUser(int userId);
 
    }
}
