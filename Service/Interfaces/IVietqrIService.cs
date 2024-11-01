namespace WebXeDapAPI.Service.Interfaces
{
    public interface IVietqrService
    {
        Task<byte[]> GenerateQrCodeAsync(string bank, string accountNumber, string amount, string ndck, string fullName);
    }
}
