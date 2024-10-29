using WebXeDapAPI.Data;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Helper;
using WebXeDapAPI.Models;
using WebXeDapAPI.Repository;
using WebXeDapAPI.Repository.Interface;
using WebXeDapAPI.Service.Interfaces;

namespace WebXeDapAPI.Service
{
    public class AccountingService : IAccountingIService
    {
        private readonly IPaymentInterface _paymentInterface;

        private readonly IUserInterface _userInterface;

        private readonly IOrderInterface _orderInterface; 

        private readonly IOrderDetailsInterface _orderDetailsInterface;

        public AccountingService(IPaymentInterface paymentInterface,IUserInterface userInterface,  IOrderInterface orderInterface,   IOrderDetailsInterface orderDetailsInterface){
            _paymentInterface = paymentInterface;
            _userInterface = userInterface;
            _orderInterface = orderInterface;
            _orderDetailsInterface = orderDetailsInterface;
        }

        public async Task<List<PaymentDto>> FindAll()
        {
            List<Payment>? payments = await _paymentInterface.GetAll();
            List<PaymentDto>? dtos = new(); 
            foreach (var payment in payments)
            {
                var dto = PaymentMapper.EntityToDto(payment);
                dtos.Add(dto);
            }
            return dtos;
        }

        public async Task<List<PaymentDto>> FindInTimeRange(DateTime startTime, DateTime endTime)
        {
            // Ensure the endTime is greater than startTime
            if (endTime <= startTime)
            {
                throw new ArgumentException("endTime must be greater than startTime.");
            }

            // Retrieve payments in the specified time range
            List<Payment> payments = await _paymentInterface.GetInTimeRange(startTime, endTime);

            // Map to PaymentDto
            List<PaymentDto> dtos = payments.Select(payment => PaymentMapper.EntityToDto(payment)).ToList();

            return dtos;
        }

    }
}
