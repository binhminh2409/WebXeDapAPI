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

        private readonly IUserInterface _userInterface;

        private readonly IOrderInterface _orderInterface; 

        public PaymentService(IPaymentInterface paymentInterface,IUserInterface userInterface,  IOrderInterface orderInterface){
            _paymentInterface = paymentInterface;
            _userInterface = userInterface;
            _orderInterface = orderInterface;
        }

        public async Task<PaymentDto> CreateAsync(PaymentDto dto)
        {
            if (dto == null) {
                return null;
            }
            User user = _userInterface.GetUser(dto.UserId);
            Order order = _orderInterface.GetById(dto.OrderId);
            Payment payment = PaymentMapper.DtoToEntity(dto, user, order);
            Payment createdPayment = await _paymentInterface.CreateAsync(payment);
            PaymentDto createdPaymentDto = PaymentMapper.EntityToDto(createdPayment);
            return createdPaymentDto;
        }
    }
}
