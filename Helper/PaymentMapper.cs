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
        return new Payment
        {
            User = user,  // Assuming `user` and `order` are fetched from database or passed in
            Order = order,
            TotalPrice = dto.TotalPrice,
            Status = status
        };
    }
}
