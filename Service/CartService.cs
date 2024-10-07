using WebXeDapAPI.Data;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;
using WebXeDapAPI.Models.Enum;
using System.Dynamic;
using WebXeDapAPI.Repository.Interface;
using WebXeDapAPI.Service.Interfaces;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static System.Net.Mime.MediaTypeNames;
using Data.Dto;

namespace WebXeDapAPI.Service
{
    public class CartService : ICartIService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICartInterface _cartInterface;
        public CartService(ApplicationDbContext dbContext, ICartInterface cartInterface)
        {
            _cartInterface = cartInterface;
            _dbContext = dbContext;
        }
        public List<Cart> CrateBicycle(CartDto cartDto)
        {
            List<Cart> cartList = new List<Cart>();
            
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == cartDto.UserId);
            if (user == null)
            {
                throw new Exception("UserId not found");
            }
            foreach (var productId in cartDto.ProductIDs)
            {
                var product = _dbContext.Products.FirstOrDefault(x => x.Id == productId);
                if (product == null)
                {
                    throw new Exception("Product ID not found");
                }
                decimal priceToUse = product.PriceHasDecreased > 0 ? product.PriceHasDecreased : product.Price;
                var cart = _dbContext.Carts.FirstOrDefault(x => x.ProductId == productId && x.UserId == cartDto.UserId);
                if (cart != null)
                {
                    cart.Quantity += 1;
                    cart.TotalPrice = priceToUse * cart.Quantity;
                }
                else
                {
                    Cart newCart = new Cart
                    {
                        UserId = user.Id,
                        ProductId = productId,
                        ProductName = product.ProductName,
                        PriceProduct = priceToUse,
                        TotalPrice = priceToUse,
                        Quantity = 1,
                        Image = product.Image,
                        Create = DateTime.Now,
                        Status = StatusCart.Pending
                    };
                    _dbContext.Carts.Add(newCart);
                    cartList.Add(newCart);
                }
            }
            _dbContext.SaveChanges();
            return cartList;
        }

        public bool Delete(int Id)
        {
            try
            {
                var query = _dbContext.Carts.FirstOrDefault(x => x.Id == Id);
                if (query == null)
                {
                    throw new Exception("Id not found");
                }
                _dbContext.Carts.Remove(query);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while Delete the Cart quantity :{ex.Message}");
            }
        }

        public bool DeleteCart(int userid, List<int> productIds)
        {
            try
            {
                var cartItems = _dbContext.Carts.Where(x => x.UserId == userid && productIds.Contains(x.ProductId)).ToList();
                if (cartItems == null || !cartItems.Any())
                {
                    throw new Exception("No cart items found for the specified userId and productIds.");
                }

                _dbContext.Carts.RemoveRange(cartItems);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the cart items: {ex.Message}");
            }
        }


        public List<object> GetCart(int userId)
        {
            List<object> result = new List<object>();
            var cart = _dbContext.Carts.FirstOrDefault(x => x.UserId == userId);
            if (cart == null)
            {
                throw new Exception("cartId not found");
            }
            List<GetCartInfDto> cart1 = _cartInterface.GetCartItemByUser(userId);
            foreach (var item in cart1)
            {
                var caerInfo = new GetCartInfDto
                {
                    CartId = item.CartId,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    PriceProduct = item.PriceProduct,
                    TotalPrice = item.TotalPrice,
                    Quantity = item.Quantity,
                    Image = item.Image,
                };
                result.Add(caerInfo);
            }
            return result;
        }

        //Tăng số lượng trong giỏ hàng
        public string IncreaseQuantityShoppingCart(int UserId, int createProductId)
        {
            try
            {
                var cart = _dbContext.Carts.FirstOrDefault(x => x.UserId == UserId && x.ProductId == createProductId);
                if (cart == null) 
                {
                    throw new Exception("UserId & ProducId not found");
                }
                var product = _dbContext.Products.FirstOrDefault(x => x.Id == createProductId);
                if(product == null)
                {
                    throw new Exception("ProducId not found");
                }
                cart.Quantity += 1;
                cart.TotalPrice = product.Price * cart.Quantity;
                _dbContext.SaveChanges();
                return "Update Successfully";
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the Cart quantity :{ex.Message}");
            }
        }

        //giảm số lượng trong giỏ hàng
        public object ReduceShoppingCart(int UserId, int createProductId)
        {
            try
            {
                var cart = _dbContext.Carts.FirstOrDefault(x => x.UserId == UserId && x.ProductId == createProductId);
                if(cart == null)
                {
                    throw new Exception("UserId & ProducId not found");
                }
                var product = _dbContext.Products.FirstOrDefault(x => x.Id == createProductId);
                if (product == null)
                {
                    throw new Exception("ProducId not found");
                }
                cart.Quantity -= 1;
                cart.TotalPrice = product.Price * cart.Quantity;
                if(cart.Quantity <= 0)
                {
                    _dbContext.Remove(cart);
                    return "Shopping cart item removed successfully";
                }
                _dbContext.SaveChanges();
                return "ReduceShoppingCart Successfully";
            }
            catch(Exception ex)
            {
                throw new Exception($"An error occurred while updating the Cart quantity :{ex.Message}");
            }
        }
    }
}
