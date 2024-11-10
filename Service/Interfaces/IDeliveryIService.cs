using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Service.Interfaces
{
    public interface IDeliveryIService
    {
        public Task<DeliveryDto> CreateAsync(PaymentDto payment, string cityFrom, string cityTo, string districtFrom, string districtTo);

        public Task<DeliveryDto> CreateSelfAsync(PaymentDto payment);

        public Task<DeliveryDto> UpdateAsync(DeliveryDto deliveryDto);


        public Task<List<DeliveryDto>> FindAll();

        public Task<DeliveryDto> FindById(int deliveryId);

        public Task<List<DeliveryDto>> FindByUser(int userId);
        
        public Task<DeliveryDto> FindByOrderId(int orderId);

        public Task<DeliveryDto> GetFee(PaymentDto payment, string cityFrom, string cityTo, string districtFrom, string districtTo);
    }
}
