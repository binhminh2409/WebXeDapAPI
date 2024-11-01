using System.Net.Http;
using System.Threading.Tasks;
using WebXeDapAPI.Service.Interfaces;


namespace WebXeDapAPI.Service
{
    public class VietqrService : IVietqrService
    {
        private readonly HttpClient _httpClient;

        public VietqrService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<byte[]> GenerateQrCodeAsync(string bank, string accountNumber, string amount, string ndck, string fullName)
        {
            var url = $"https://api.vieqr.com/vietqr/{bank}/{accountNumber}/{amount}/full.jpg?NDck={ndck}&FullName={fullName}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }

            throw new HttpRequestException("Unable to generate QR code.");
        }
    }

}
