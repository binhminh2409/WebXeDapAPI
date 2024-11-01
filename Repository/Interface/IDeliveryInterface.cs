using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Repository.Interface
{
    public interface IDeliveryInterface
    {
        Task<List<Delivery>> GetByUserAsync(int userId);
        
        Task<Delivery> GetByIdAsync(int DeliveryId);

        Task<Delivery> CreateAsync(Delivery Delivery);

        Task<List<Delivery>> GetAll();

        Task<Delivery> GetByPaymentId(int paymentId);
    }
}
