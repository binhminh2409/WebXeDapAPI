using WebXeDapAPI.Data;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Helper;
using WebXeDapAPI.Models;
using WebXeDapAPI.Models.Enum;
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


        public PaymentService(IPaymentInterface paymentInterface, IUserInterface userInterface, IOrderInterface orderInterface, IOrderDetailsInterface orderDetailsInterface)
        {
            _paymentInterface = paymentInterface;
            _userInterface = userInterface;
            _orderInterface = orderInterface;
            _orderDetailsInterface = orderDetailsInterface;
        }

        public async Task<PaymentDto> CreateAsync(PaymentDto dto)
        {
            if (dto == null) throw new ArgumentException("Request invalid");

            var user = _userInterface.GetUser(dto.UserId);
            var order = _orderInterface.GetById(dto.OrderId);
            List<Order_Details> order_Details = _orderDetailsInterface.GetAllByOrderId(order.No_);
            decimal totalPrice = 0;

            // Get total price
            foreach (var order_Detail in order_Details)
            {
                totalPrice += order_Detail.TotalPrice;
            }

            // Check amount
            if (dto.TotalPrice != totalPrice)
                throw new ArgumentException("The payment amount does not match the order total.");

            if (await _paymentInterface.GetByOrderId(dto.OrderId) != null)
            {
                var existedPayment = await _paymentInterface.GetByOrderId(dto.OrderId);
                return PaymentMapper.EntityToDto(existedPayment);
            }

            dto.Status = "Processing";

            var payment = PaymentMapper.DtoToEntity(dto, user, order);
            var createdPayment = await _paymentInterface.CreateAsync(payment);

            order.Status = Models.Enum.StatusOrder.Processing;
            _orderInterface.Update(order);

            return PaymentMapper.EntityToDto(createdPayment);
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

        public async Task<List<PaymentDto>> FindByUser(int userId)
        {
            List<Payment> payments = await _paymentInterface.GetByUserAsync(userId);
            List<PaymentDto> dtos = new List<PaymentDto>();
            foreach (var payment in payments)
            {
                dtos.Add(PaymentMapper.EntityToDto(payment));
            }

            return dtos;
        }

        public async Task<PaymentDto> ConfirmAsync(int paymentId)
        {
            Payment payment = await _paymentInterface.GetByIdAsync(paymentId) ?? throw new ArgumentException("Payment id invalid");

            Order order = payment.Order;
            order.Status = Models.Enum.StatusOrder.Paid;
            _orderInterface.Update(order);

            payment.Status = Models.Enum.StatusPayment.Successful;
            Payment updatedPayment = await _paymentInterface.UpdateAsync(payment);
            PaymentDto updatedPaymentDto = PaymentMapper.EntityToDto(updatedPayment);
            return updatedPaymentDto;
        }

        public async Task<PaymentDto> UpdateStatusAsync(PaymentDto dto)
        {
            Payment payment = await _paymentInterface.GetByIdAsync(dto.Id) ?? throw new ArgumentException("Payment id invalid");

            StatusPayment status = (StatusPayment)Enum.Parse(typeof(StatusPayment), dto.Status);

            payment.Status = status;
            Payment updatedPayment = await _paymentInterface.UpdateAsync(payment);

            PaymentDto updatedPaymentDto = PaymentMapper.EntityToDto(updatedPayment);
            return updatedPaymentDto;
        }

        public async Task<List<PaymentDto>> FindByGuid(string guid)
        {
            List<Order> orders = _orderInterface.GetByGuid(guid);
            List<Payment> payments = new();
            foreach (var order in orders)
            {
                Payment payment = await _paymentInterface.GetByOrderId(order.Id);
                payments.Add(payment);
            }
            List<PaymentDto> dtos = new List<PaymentDto>();
            foreach (var payment in payments)
            {
                dtos.Add(PaymentMapper.EntityToDto(payment));
            }

            return dtos;
        }
    }
}
