using WebXeDapAPI.Data;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;
using WebXeDapAPI.Models.Enum;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using WebXeDapAPI.Repository.Interface;
using WebXeDapAPI.Service.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

namespace WebXeDapAPI.Service
{
    public class OrderService : IOrderIService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICartInterface _cartInterface;
        private readonly IOrderDetailsInterface _orderDetailsInterface;
        public OrderService(ApplicationDbContext dbContext,ICartInterface cartInterface, IOrderDetailsInterface orderDetailsInterface)
        {
            _cartInterface = cartInterface;
            _dbContext = dbContext;
            _orderDetailsInterface = orderDetailsInterface;
        }
        public (Order, List<Order_Details>) Create([FromQuery] OrderDto orderDto)
        {
            try
            {
                if (orderDto == null)
                {
                    throw new ArgumentNullException(nameof(orderDto), "OrderDto cannot be null");
                }
                int? userId = orderDto.UserID;
                int userIDToAssign = userId ?? -1;
                if (userId.HasValue)
                {
                    var user = _dbContext.Users.FirstOrDefault(x => x.Id == userId.Value);
                }

                var order = new Order
                {
                    No_ = AutomaticallyGenerateOrderNumbers(),
                    UserID = userIDToAssign,
                    ShipName = orderDto.ShipName,
                    ShipAddress = orderDto.ShipAddress,
                    ShipEmail = orderDto.ShipEmail,
                    ShipPhone = orderDto.ShipPhone,
                    Status = StatusOrder.Pending
                };

                if (orderDto.Cart != null && orderDto.Cart.Any())
                {
                    var orderDetails = new List<Order_Details>();
                    foreach (var productId in orderDto.Cart)
                    {
                        var cart = _cartInterface.GetProducId(productId);
                        Console.WriteLine("-------------------------" + cart.Id);
                        if (cart != null)
                        {
                            var orderDetail = new Order_Details
                            {
                                OrderID = order.No_,
                                ProductID = cart.ProductId,
                                ProductName = cart.ProductName,
                                Quantity = cart.Quantity,
                                PriceProduc = cart.PriceProduct,
                                Color = cart.Color,
                                TotalPrice = cart.TotalPrice,
                                Image = cart.Image,
                                CreatedDate = DateTime.Now
                            };
                            Console.WriteLine("-------------------------" + orderDetail.ToString());
                            orderDetails.Add(orderDetail);
                        }
                        else
                        {
                            throw new Exception($"Product with ID {productId} not found.");
                        }
                    }

                    _dbContext.Orders.Add(order);
                    _dbContext.Order_Details.AddRange(orderDetails);
                    _dbContext.SaveChanges();
                    foreach (var orderDetail in orderDetails)
                    {
                        var product = _dbContext.Products.FirstOrDefault(p => p.Id == orderDetail.ProductID);
                        if (product != null)
                        {
                            product.Quantity -= orderDetail.Quantity;
                        }
                        else
                        {
                            throw new Exception($"Product with ID {orderDetail.ProductID} not found in Products table.");
                        }
                    }

                    _dbContext.SaveChanges();

                    return (order, orderDetails);
                }
                else
                {
                    throw new Exception("Cart is empty.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("There is an error when creating an Order", ex);
            }
        }

        public Order_Details GetByOrderNo(string orderNo) {
            Order_Details? order_Details = _orderDetailsInterface.GetByOrderId(orderNo);
            return order_Details;
        }

        private static string AutomaticallyGenerateOrderNumbers()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 1000);//Sinh ra số ngẫy nhiên từ 1 đến 9999
            string formattedNumber = randomNumber.ToString("D4");//Định dạng số thành chuỗi với độ dài 4 ký tư

            string dateTimeNow = DateTime.Now.ToString("yyyyMMddHHmmss"); // Format DateTime as a string
            byte[] dateTimeBytes = Encoding.UTF8.GetBytes(dateTimeNow);
            string encryptedDateTime = Convert.ToBase64String(dateTimeBytes); // "Encrypt" it by converting to base64

            return $"OR-{formattedNumber}-{encryptedDateTime}";
        }
    }
}
