using System;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;
using WebXeDapAPI.Models.Enum;

namespace WebXeDapAPI.Helper
{
    public class DeliveryMapper
    {
        public static Delivery DtoToEntity(DeliveryDto dto, Payment payment)
        {
            if (dto == null) return null;

            return new Delivery
            {
                Id = dto.Id,
                Status = (StatusDelivery)Enum.Parse(typeof(StatusDelivery), dto.Status),
                Payment = payment,
                No_ = dto.No_,
                UserId = dto.UserId,
                CreatedTime = DateTime.UtcNow, // Set current time when creating
                UpdatedTime = DateTime.UtcNow  // Set current time when creating
            };
        }

        public static DeliveryDto EntityToDto(Delivery delivery)
        {
            if (delivery == null) throw new ArgumentNullException(nameof(delivery));

            DeliveryDto dto = new DeliveryDto
            {
                Id = delivery.Id,
                Status = delivery.Status.ToString(),
                Payment = delivery.Payment != null ? PaymentMapper.EntityToDto(delivery.Payment) : null,
                No_ = delivery.No_,
                UserId = delivery.UserId,
                CreatedTime = delivery.CreatedTime,
                UpdatedTime = delivery.UpdatedTime
            };
            return dto;
        }

    }
}
