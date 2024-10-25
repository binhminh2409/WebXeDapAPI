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

        private readonly IOrderDetailsInterface _orderDetailsInterface;


        public PaymentService(IPaymentInterface paymentInterface,IUserInterface userInterface,  IOrderInterface orderInterface,   IOrderDetailsInterface orderDetailsInterface){
            _paymentInterface = paymentInterface;
            _userInterface = userInterface;
            _orderInterface = orderInterface;
            _orderDetailsInterface = orderDetailsInterface;
        }

        public async Task<PaymentDto> CreateAsync(PaymentDto dto) 
        {
            if (dto == null) {
                throw new ArgumentException("Request invalid");
            }
            User user = _userInterface.GetUser(dto.UserId);


            Order order = _orderInterface.GetById(dto.OrderId);
            
            // Check payment amount: 
            Order_Details order_Details = _orderDetailsInterface.GetByOrderId(order.No_);
            if (dto.TotalPrice != order_Details.TotalPrice) {
                throw new ArgumentException("The payment amount does not match the order total."); 
            }
            
            Payment payment = PaymentMapper.DtoToEntity(dto, user, order);
            Console.WriteLine(payment.ToString());
            Payment createdPayment = await _paymentInterface.CreateAsync(payment);
            

            // Update Cart and Order status 
            order.Status = Models.Enum.StatusOrder.Paid;
            _orderInterface.Update(order);

            PaymentDto createdPaymentDto = PaymentMapper.EntityToDto(createdPayment);
            return createdPaymentDto;
        }
    }
}
