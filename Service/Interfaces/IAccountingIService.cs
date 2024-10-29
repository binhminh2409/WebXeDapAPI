using WebXeDapAPI.Dto;

namespace WebXeDapAPI.Service.Interfaces
{
    public interface IAccountingIService
    {
        public Task<List<PaymentDto>> FindAll();

        public Task<List<PaymentDto>> FindInTimeRange(DateTime startTime, DateTime endTime);
    }
}
