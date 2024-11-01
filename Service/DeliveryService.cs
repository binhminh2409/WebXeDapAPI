using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Helper;
using WebXeDapAPI.Models;
using WebXeDapAPI.Models.Enum;
using WebXeDapAPI.Repository;
using WebXeDapAPI.Repository.Interface;
using WebXeDapAPI.Service.Interfaces;


public class DeliveryService : IDeliveryIService
{
    private readonly string? _goshipToken;
    private readonly HttpClient _httpClient;
    private readonly IDeliveryInterface _deliveryInterface;
    private readonly IPaymentInterface _paymentInterface;
    private readonly IOrderInterface _orderInterface;

    private readonly string _requestUrl = "http://sandbox.goship.io/api/v2";

    public DeliveryService(IOptions<GoshipSettings> goshipSettings,
    HttpClient httpClient,
    IDeliveryInterface deliveryInterface,
    IPaymentInterface paymentInterface,
    IOrderInterface orderInterface)
    {
        _goshipToken = goshipSettings.Value.Token;

        if (string.IsNullOrEmpty(_goshipToken))
        {
            throw new Exception("Goship token is empty.");
        }

        _httpClient = httpClient;
        _deliveryInterface = deliveryInterface;
        _paymentInterface = paymentInterface;
        _orderInterface = orderInterface;
    }

    public async Task<DeliveryDto> CreateAsync(PaymentDto paymentDto, string cityFrom, string cityTo, string districtFrom, string districtTo)
    {
        // Initialize shipment: 
        DeliveryDto dto = await GetFee(paymentDto, cityFrom, cityTo, districtFrom, districtTo);
        Payment payment = await _paymentInterface.GetByIdAsync(paymentDto.Id);

        // Construct the request payload
        var requestUrl = $"{_requestUrl}/shipments";
        var shipmentData = new
        {
            shipment = new
            {
                rate = dto.No_,
                address_from = new
                {
                    name = "Mint Bike",
                    phone = "012 345 6789",
                    street = "8 Hai Bà Trưng",
                    ward = "113",
                    district = cityFrom,
                    city = cityTo
                },
                address_to = new
                {
                    name = payment.Order.ShipName,
                    phone = payment.Order.ShipPhone,
                    street = payment.Order.ShipAddress,
                    ward = "79",
                    district = districtTo,
                    city = cityTo
                },
                parcel = new
                {
                    cod = paymentDto.TotalPrice.ToString(),
                    weight = "220",
                    width = "15",
                    height = "15",
                    length = "15",
                    metadata = "Hàng dễ vỡ, vui lòng nhẹ tay."
                }
            }
        };

        // Serialize the shipment data to JSON
        var jsonContent = JsonSerializer.Serialize(shipmentData);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // Prepare the HTTP request
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUrl)
        {
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", _goshipToken) },
            Content = content
        };
        // Send the request
        var response = await _httpClient.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to create shipment. Please check the input data and try again.");
        }

        // Parse the response
        using var jsonDoc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        if (jsonDoc.RootElement.TryGetProperty("code", out var codeElement) && codeElement.GetInt32() == 200)
        {
            var deliveryId = jsonDoc.RootElement.GetProperty("id").GetString();
            var fee = jsonDoc.RootElement.GetProperty("fee").GetDecimal();

            // Create a new Delivery entity
            var delivery = new Delivery
            {
                UserId = paymentDto.UserId, // Set the UserId from the dto
                No_ = deliveryId, // Map the API id to No_
                Status = StatusDelivery.NewOrder,
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow,
                Payment = payment
            };

            // Save the delivery record to the database
            Delivery createdDelivery = await _deliveryInterface.CreateAsync(delivery);

            payment.Order.Status = StatusOrder.Shipped;
            _orderInterface.Update(payment.Order);

            // Create and return the DeliveryDto response
            return DeliveryMapper.EntityToDto(delivery);
        }

        // Handle cases where the response code is not 200
        throw new Exception("Failed to create shipment. Unexpected response format.");
    }

    public async Task<List<DeliveryDto>> FindAll()
    {
        List<Delivery> deliveries = await _deliveryInterface.GetAll();
        List<DeliveryDto> dtos = new();
        foreach (var item in deliveries)
        {
            dtos.Add(DeliveryMapper.EntityToDto(item));
        }
        return dtos;
    }

    public async Task<List<DeliveryDto>> FindByUser(int userId)
    {
        List<Delivery> deliveries = await _deliveryInterface.GetByUserAsync(userId);
        List<DeliveryDto> dtos = new();
        foreach (var item in deliveries)
        {
            dtos.Add(DeliveryMapper.EntityToDto(item));
        }
        return dtos;
    }


    public async Task<DeliveryDto> FindById(int deliveryId)
    {
        Delivery delivery = await _deliveryInterface.GetByIdAsync(deliveryId);
        DeliveryDto dto = DeliveryMapper.EntityToDto(delivery);
        return dto;
    }

    public async Task<DeliveryDto> FindByOrderId(int orderId)
    {
        Payment payment = await _paymentInterface.GetByOrderId(orderId);
        Delivery delivery = await _deliveryInterface.GetByPaymentId(payment.Id);
        DeliveryDto dto = DeliveryMapper.EntityToDto(delivery);
        return dto;
    }

    public async Task<DeliveryDto> GetFee(PaymentDto paymentDto, string cityFrom, string cityTo, string districtFrom, string districtTo)
    {
        // Define the request URL for getting rates
        var requestUrl = $"{_requestUrl}/rates";
        Console.WriteLine($"Request URL: {requestUrl}");

        decimal codFee = paymentDto.Method == "COD" ? paymentDto.TotalPrice : 0;

        // Construct the shipment data payload
        var shipmentData = new
        {
            shipment = new
            {
                address_from = new
                {
                    city = cityFrom, // City code for the origin
                    district = districtFrom // District code for the origin
                },
                address_to = new
                {
                    city = cityTo, // City code for the destination
                    district = districtTo // District code for the destination
                },
                parcel = new
                {
                    cod = codFee, // Cash on Delivery amount
                    weight = "15000", // Weight of the parcel
                    width = "60", // Width of the parcel
                    height = "110", // Height of the parcel
                    length = "180" // Length of the parcel
                }
            }
        };

        // Serialize the shipment data to JSON
        var jsonContent = JsonSerializer.Serialize(shipmentData);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUrl)
        {
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", _goshipToken) },
            Content = content
        };

        // Send the request
        var response = await _httpClient.SendAsync(requestMessage);

        // Check if the response is successful
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to retrieve remote delivery information. Status code: {response.StatusCode}");
        }

        // Parse the response content
        using var jsonDoc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());

        // Extract fee information from the JSON response
        if (jsonDoc.RootElement.TryGetProperty("data", out JsonElement dataElement) && dataElement.ValueKind == JsonValueKind.Array)
        {
            if (dataElement.GetArrayLength() > 0)
            {
                var firstDeliveryOption = dataElement[0];

                var deliveryDto = new DeliveryDto
                {
                    No_ = firstDeliveryOption.GetProperty("id").ToString()
                };

                return deliveryDto;
            }
            else
            {
                throw new Exception("No delivery options found in the response.");
            }
        }
        else
        {
            throw new Exception("Data property not found in the response.");
        }
    }


}
