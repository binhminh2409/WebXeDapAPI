using WebXeDapAPI.Data;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Helper;
using WebXeDapAPI.Models;
using WebXeDapAPI.Repository;
using WebXeDapAPI.Repository.Interface;
using WebXeDapAPI.Service.Interfaces;

namespace WebXeDapAPI.Service
{
    public class PaymentService : IPaymentIService
    {
        private readonly IPaymentInterface _paymentInterface;

        public PaymentService(IPaymentInterface paymentInterface){
            _paymentInterface = paymentInterface;
        }

        public async Task<PaymentDto> CreateAsync(PaymentDto dto, User user, Order order)
        {
            Payment payment = PaymentMapper.DtoToEntity(dto, user, order);
            Payment createdPayment = await _paymentInterface.CreateAsync(payment);
            return dto;
        }
    }
}
