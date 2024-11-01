namespace WebXeDapAPI.Models.Enum
{
    public enum StatusPayment
    {
        Pending, // Chờ xử lý
        Processing, // Đang xử lý
        Confirmed,
        Successful, // TT thanh cong
        Failed //TT that bai
    }
}
