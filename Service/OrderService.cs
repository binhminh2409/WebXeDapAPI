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
using Microsoft.EntityFrameworkCore;
using WebXeDapAPI.Helper;

namespace WebXeDapAPI.Service
{
    public class OrderService : IOrderIService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICartInterface _cartInterface;

        private readonly IStockInterface _stockInterface;
        private readonly IOrderDetailsInterface _orderDetailsInterface;

        private readonly IOrderInterface _orderInterface;
        public OrderService(ApplicationDbContext dbContext, ICartInterface cartInterface, IOrderDetailsInterface orderDetailsInterface, IOrderInterface orderInterface, IStockInterface stockInterface)
        {
            _cartInterface = cartInterface;
            _dbContext = dbContext;
            _orderDetailsInterface = orderDetailsInterface;
            _orderInterface = orderInterface;
            _stockInterface = stockInterface;
        }

        public async Task<List<ProductGetAllInfPriceDto>> ListOfBestSellingProducts()
        {
            var oneMonthAgo = DateTime.Now.AddMonths(-1);

            var bestSellingProducts = await _dbContext.Order_Details
                .Where(od => od.CreatedDate >= oneMonthAgo)
                .GroupBy(od => od.ProductID)
                .Select(group => new
                {
                    ProductId = group.Key,
                    TotalSold = group.Count()
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(8)
                .Join(_dbContext.Products,
                    bestSeller => bestSeller.ProductId,
                    product => product.Id,
                    (bestSeller, product) => new ProductGetAllInfPriceDto
                    {
                        Id = product.Id,
                        ProductName = product.ProductName,
                        Price = product.Price,
                        PriceHasDecreased = product.PriceHasDecreased,
                        Image = product.Image,
                        BrandNamer = product.brandName,
                    })
                .ToListAsync();

            return bestSellingProducts;
        }

        public (Order, List<Order_Details>) Create([FromQuery] OrderDto orderDto)
        {
            try
            {
                string cartGuid = null;
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



                if (orderDto.Cart != null && orderDto.Cart.Any() && userIDToAssign == -1)
                {
                    foreach (var productId in orderDto.Cart)
                    {
                        var cart = _cartInterface.GetProducId(productId);
                        cartGuid = cart.GuId ?? null;
                    }
                }

                var order = new Order
                {
                    No_ = AutomaticallyGenerateOrderNumbers(),
                    UserID = userIDToAssign,
                    ShipName = orderDto.ShipName,
                    ShipAddress = orderDto.ShipAddress,
                    ShipEmail = orderDto.ShipEmail,
                    ShipPhone = orderDto.ShipPhone,
                    Status = StatusOrder.Pending,
                    Guid = cartGuid
                };

                if (orderDto.Cart != null && orderDto.Cart.Any())
                {
                    var orderDetails = new List<Order_Details>();

                    foreach (var productId in orderDto.Cart)
                    {

                        var cart = _cartInterface.GetProducId(productId);
                        // Check stock
                        Stock stock = _stockInterface.GetByProductId(productId);
                        if (cart.Quantity > stock.Quantity)
                        {
                            throw new ArgumentException($"Product with ID {productId} does not have enough stock");
                        }
                        _stockInterface.DecreaseQuantity(stock.Id, cart.Quantity);

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
                throw new Exception($"There is an error when creating an Order {ex.Message}", ex);
            }
        }

        public Order_Details GetByOrderNo(string orderNo)
        {
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

        public List<OrderDto> GetByUser(int userId)
        {
            List<Order>? orders = _orderInterface.GetByUser(userId);
            List<OrderDto>? dtos = new();
            foreach (var order in orders)
            {
                OrderDto dto = new OrderDto
                {
                    Id = order.Id,
                    ShipAddress = order.ShipAddress,
                    ShipEmail = order.ShipEmail,
                    ShipName = order.ShipName,
                    ShipPhone = order.ShipPhone,
                    UserID = order.UserID,
                    Status = order.Status.ToString(),
                    No_ = order.No_
                };
                dtos.Add(dto);
            }
            return dtos;
        }

        public List<Order_Details> GetDetailsByUser(int userId)
        {
            List<Order_Details>? order_Details = _orderDetailsInterface.GetByUser(userId);
            return order_Details;
        }

        public OrderWithDetailDto GetByIdWithDetail(int orderId)
        {
            Order order = _orderInterface.GetById(orderId);
            List<Order_Details> order_Details = _orderDetailsInterface.GetAllByOrderId(order.No_);
            List<OrderDetailDto> orderDetailDtos = new();

            // Populate orderdetails
            foreach (var order_Detail in order_Details)
            {
                OrderDetailDto added = new OrderDetailDto
                {
                    Id = order_Detail.Id,
                    OrderID = order_Detail.OrderID,
                    ProductID = order_Detail.ProductID,
                    ProductName = order_Detail.ProductName,
                    PriceProduc = order_Detail.PriceProduc,
                    Quantity = order_Detail.Quantity,
                    TotalPrice = order_Detail.TotalPrice,
                    Image = order_Detail.Image,
                    Color = order_Detail.Color,
                    CreatedDate = order_Detail.CreatedDate
                };
                orderDetailDtos.Add(added);
            }

            OrderWithDetailDto orderWithDetailDto = new OrderWithDetailDto
            {
                Id = order.Id,
                UserID = order.UserID,
                ShipName = order.ShipName,
                ShipAddress = order.ShipAddress,
                ShipEmail = order.ShipEmail,
                ShipPhone = order.ShipPhone,
                No_ = order.No_,
                Status = order.Status.ToString(), // Convert enum to string if needed

                OrderDetails = orderDetailDtos
            };

            return orderWithDetailDto;
        }

        public string CancelOrder(int orderId)
        {
            try
            {
                Order order = _orderInterface.GetById(orderId);
                order.Status = StatusOrder.Cancelled;
                _orderInterface.Update(order);
                return "Order cancelled";
            }
            catch (Exception ex)
            {
                throw new Exception("There is an error when creating an Order", ex);
            }
        }

        public List<OrderWithDetailDto> GetByGuid(string Guid)
        {
            try
            {
                List<Order> orders = _orderInterface.GetByGuid(Guid);
                List<OrderWithDetailDto> orderWithDetailDtos = new();

                foreach (var order in orders)
                {
                    List<Order_Details> order_Details = _orderDetailsInterface.GetAllByOrderId(order.No_);
                    List<OrderDetailDto> orderDetailDtos = new();
                    // Populate orderdetails
                    foreach (var order_Detail in order_Details)
                    {
                        OrderDetailDto added = new OrderDetailDto
                        {
                            Id = order_Detail.Id,
                            OrderID = order_Detail.OrderID,
                            ProductID = order_Detail.ProductID,
                            ProductName = order_Detail.ProductName,
                            PriceProduc = order_Detail.PriceProduc,
                            Quantity = order_Detail.Quantity,
                            TotalPrice = order_Detail.TotalPrice,
                            Image = order_Detail.Image,
                            Color = order_Detail.Color,
                            CreatedDate = order_Detail.CreatedDate
                        };
                        orderDetailDtos.Add(added);
                    }

                    OrderWithDetailDto orderWithDetailDto = new OrderWithDetailDto
                    {
                        Id = order.Id,
                        UserID = order.UserID,
                        ShipName = order.ShipName,
                        ShipAddress = order.ShipAddress,
                        ShipEmail = order.ShipEmail,
                        ShipPhone = order.ShipPhone,
                        No_ = order.No_,
                        Status = order.Status.ToString(), // Convert enum to string if needed

                        OrderDetails = orderDetailDtos
                    };

                    orderWithDetailDtos.Add(orderWithDetailDto);
                }

                return orderWithDetailDtos;
            }
            catch (Exception e)
            {
                throw new Exception($"Error getting orders: {e.Message}");
            }
        }

        public List<OrderWithDetailDto> GetByUserWithDetail(int userId)
        {
            try
            {
                List<Order> orders = _orderInterface.GetByUser(userId);
                List<OrderWithDetailDto> orderWithDetailDtos = new();

                foreach (var order in orders)
                {
                    List<Order_Details> order_Details = _orderDetailsInterface.GetAllByOrderId(order.No_);
                    List<OrderDetailDto> orderDetailDtos = new();
                    // Populate orderdetails
                    foreach (var order_Detail in order_Details)
                    {
                        OrderDetailDto added = new OrderDetailDto
                        {
                            Id = order_Detail.Id,
                            OrderID = order_Detail.OrderID,
                            ProductID = order_Detail.ProductID,
                            ProductName = order_Detail.ProductName,
                            PriceProduc = order_Detail.PriceProduc,
                            Quantity = order_Detail.Quantity,
                            TotalPrice = order_Detail.TotalPrice,
                            Image = order_Detail.Image,
                            Color = order_Detail.Color,
                            CreatedDate = order_Detail.CreatedDate
                        };
                        orderDetailDtos.Add(added);
                    }

                    OrderWithDetailDto orderWithDetailDto = new OrderWithDetailDto
                    {
                        Id = order.Id,
                        UserID = order.UserID,
                        ShipName = order.ShipName,
                        ShipAddress = order.ShipAddress,
                        ShipEmail = order.ShipEmail,
                        ShipPhone = order.ShipPhone,
                        No_ = order.No_,
                        Status = order.Status.ToString(), // Convert enum to string if needed

                        OrderDetails = orderDetailDtos
                    };

                    orderWithDetailDtos.Add(orderWithDetailDto);
                }

                return orderWithDetailDtos;
            }
            catch (Exception e)
            {
                throw new Exception($"Error getting orders: {e.Message}");
            }
        }
    }
}
