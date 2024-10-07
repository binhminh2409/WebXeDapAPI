﻿namespace WebXeDapAPI.Models.Enum
{
    public enum StatusOrder
    {
        Pending, // Chờ xử lý
        Processing, // Đang xử lý
        Shipped, // Đã giao hàng
        Delivered, // Đã giao hàng thành công
        Cancelled // Đã hủy
    }
}
