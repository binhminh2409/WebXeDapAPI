using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;
using WebXeDapAPI.Models.Enum;

namespace WebXeDapAPI.Helper;
public class PaymentMapper
{
    public static Payment DtoToEntity(PaymentDto dto, User user, Order order)
    {
        if (dto == null) return null;

        StatusPayment status = (StatusPayment)Enum.Parse(typeof(StatusPayment), dto.Status);
        Method method = (Method)Enum.Parse(typeof(Method), dto.Method);
        return new Payment
        {
            User = user,
            Order = order,
            TotalPrice = dto.TotalPrice,
            Status = status,
            Method = method,
            ExtraFee = dto.ExtraFee,
            CreatedTime = DateTime.UtcNow, // Set current time when creating
            UpdatedTime = DateTime.UtcNow  // Set current time when creating
        };
    }
    public static PaymentDto EntityToDto(Payment payment)
    {
        if (payment == null) throw new ArgumentNullException(nameof(payment));

        return new PaymentDto
        {
            Id = payment.Id,
            UserId = payment.User != null ? payment.User.Id : 0,
            OrderId = payment.Order != null ? payment.Order.Id : 0,
            TotalPrice = payment.TotalPrice,
            Method = payment.Method.ToString(),
            ExtraFee = payment.ExtraFee,
            Status = payment.Status.ToString(),
            CreatedTime = payment.CreatedTime,
            UpdatedTime = payment.UpdatedTime
        };
    }
}
