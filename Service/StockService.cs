using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Helper;
using WebXeDapAPI.Models;
using WebXeDapAPI.Models.Enum;
using WebXeDapAPI.Repository.Interface;
using WebXeDapAPI.Service.Implementations;
using WebXeDapAPI.Service.Interfaces;
using WebXeDapAPI.Utilities;

namespace WebXeDapAPI.Service
{
    public class StockService : IStockIService
    {
        private readonly IStockInterface _stockInterface;

        private readonly IInputStockInterface _inputStockInterface;

        private readonly IProductsInterface _productsInterface;

        public StockService(IStockInterface stockInterface,
                IInputStockInterface inputStockInterface,
                IProductsInterface productsInterface)
        {
            _stockInterface = stockInterface;
            _inputStockInterface = inputStockInterface;
            _productsInterface = productsInterface;
        }

        private string generateBatchNumber()
        {
            DateTime currentDate = DateTime.Now;

            long timestamp = ((DateTimeOffset)currentDate).ToUnixTimeSeconds();

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(timestamp.ToString()));
                StringBuilder hexString = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hexString.AppendFormat("{0:x2}", b);
                }
                string encodedDate = hexString.ToString().Substring(0, 7);
                string batchNumber = encodedDate.ToUpper();

                return batchNumber;
            }
        }


        public async Task<StockDto> CreateAsync(StockDto dto)
        {
            try
            {
                Products product = _productsInterface.GetProductsId(dto.productId);
                Stock stock = StockMapper.DtoToEntity(dto, product);
                Stock createdStock = await _stockInterface.CreateAsync(stock);

                StockDto createdDto = StockMapper.EntityToDto(stock);
                return createdDto;
            }
            catch (Exception e)
            {
                throw new Exception($"Error at Stock Service: {e.Message}");
            }
        }

        public async Task<StockDto> DecreaseQuantity(int stockId, int decreasedBy)
        {
            try
            {
                Stock stock = await _stockInterface.DecreaseQuantityAsync(stockId, decreasedBy);
                StockDto dto = StockMapper.EntityToDto(stock);
                return dto;
            }
            catch (Exception e)
            {
                throw new Exception($"Error at Stock Service: {e.Message}");
            }
        }

        public async Task<List<StockDto>> GetAllAsync()
        {
            try
            {
                List<Stock> stocks = await _stockInterface.GetAllAsync();
                List<StockDto> dtos = new();
                foreach (var stock in stocks)
                {
                    StockDto dto = StockMapper.EntityToDto(stock);
                    dtos.Add(dto);

                }

                return dtos;
            }
            catch (Exception e)
            {
                throw new Exception($"Error at Stock Service: {e.Message}");
            }
        }

        public async Task<StockDto> GetByIdAsync(int stockId)
        {
            try
            {
                Stock stock = await _stockInterface.GetByIdAsync(stockId);
                StockDto dto = StockMapper.EntityToDto(stock);
                return dto;
            }
            catch (Exception e)
            {
                throw new Exception($"Error at Stock Service: {e.Message}");
            }
        }

        public async Task<StockDto> GetByProductId(int productId)
        {
            try
            {
                Stock stock = await _stockInterface.GetByProductIdAsync(productId);
                StockDto dto = StockMapper.EntityToDto(stock);
                return dto;
            }
            catch (Exception e)
            {
                throw new Exception($"Error at Stock Service: {e.Message}");
            }
        }

        public async Task<StockDto> IncreaseQuantity(int stockId, int increasedBy)
        {
            try
            {

                Stock stock = await _stockInterface.IncreaseQuantity(stockId, increasedBy);
                StockDto dto = StockMapper.EntityToDto(stock);
                return dto;
            }
            catch (Exception e)
            {
                throw new Exception($"Error at Stock Service: {e.Message}");
            }
        }

        public async Task<List<StockDto>> DecreaseQuantityByOrderWithDetail(OrderWithDetailDto orderWithDetailDto)
        {
            List<Stock> updatedStocks = new();
            List<StockDto> updatedStocksDto = new();
            foreach (var orderDetail in orderWithDetailDto.OrderDetails)
            {
                Stock stock = _stockInterface.GetByProductId(orderDetail.ProductID);
                Stock added = await _stockInterface.DecreaseQuantityAsync(stock.Id, orderDetail.Quantity);
                updatedStocks.Add(added);
            }

            foreach (var updatedStock in updatedStocks)
            {
                updatedStocksDto.Add(StockMapper.EntityToDto(updatedStock));
            }

            return updatedStocksDto;
        }

        public async Task<List<InputStockDto>> RestockOrder(List<InputStockDto> inputStockDtos)
        {
            try
            {
                List<InputStock> inputStocks = new();
                List<InputStock> createdInputStocks = new();
                List<InputStockDto> createdInputStockDtos = new();

                // Create batch number
                string batchNo_ = generateBatchNumber();

                foreach (var inputStockDto in inputStockDtos)
                {
                    // Get product
                    Products product = _productsInterface.GetProductsId(
                        inputStockDto.ProductId == null || inputStockDto.ProductId == 0
                            ? throw new Exception("Error restocking: Product invalid")
                            : inputStockDto.ProductId.Value
                    );

                    // Assign batch no
                    inputStockDto.BatchNo_ = batchNo_;

                    // Calculate total price
                    decimal totalPrice = inputStockDto.Price * inputStockDto.Quantity;
                    inputStockDto.TotalPrice = totalPrice;

                    // With Order status
                    inputStockDto.Status = "ORDERED";

                    // Map dto to entity
                    InputStock inputStock = StockMapper.DtoToEntity(inputStockDto, product);
                    inputStocks.Add(inputStock);
                }


                // For each entity, create inputStock and store into db
                foreach (var inputStock in inputStocks)
                {
                    InputStock createdInputStock = await _inputStockInterface.CreateAsync(inputStock);
                    createdInputStocks.Add(createdInputStock);
                }

                // Map created inputsStocks back into Dto
                foreach (var createdInputStock in createdInputStocks)
                {
                    InputStockDto createdInputStockDto = StockMapper.EntityToDto(createdInputStock);
                    createdInputStockDtos.Add(createdInputStockDto);

                    // Increase stock number
                    Stock stock = await _stockInterface.GetByProductIdAsync(createdInputStock.Product.Id);
                    await _stockInterface.IncreaseQuantity(stock.Id, createdInputStock.Quantity);
                }

                return createdInputStockDtos;
            }
            catch (Exception e)
            {
                throw new Exception($"Error at Stock Service: {e.Message}");
            }
        }

        public async Task<List<InputStockDto>> RestockOrderUpdateStatus(string batchNo_, string status)
        {
            try
            {
                InputStockStatus updatedStatus = StockEnumHelper.StringToEnum(status.ToUpper());
                List<InputStock> inputStocks = await _inputStockInterface.GetByBatchNo(batchNo_);
                List<InputStockDto> updatedInputStockDtos = new();

                // Update status
                foreach (var inputStock in inputStocks)
                {
                    inputStock.Status = updatedStatus;
                    if (updatedStatus == InputStockStatus.SUCCESSFUL)
                    {
                        inputStock.Paid = true;
                    }
                }
                foreach (var inputStock in inputStocks)
                {
                    await _inputStockInterface.UpdateAsync(inputStock);

                    if (updatedStatus == InputStockStatus.SUCCESSFUL)
                    {                    
                        // Increase stock number
                        Stock stock = await _stockInterface.GetByProductIdAsync(inputStock.Product.Id);
                        await _stockInterface.IncreaseQuantity(stock.Id, inputStock.Quantity);
                    }
                }

                foreach (var inputStock in inputStocks)
                {
                    InputStockDto updatedInputStockDto = StockMapper.EntityToDto(inputStock);
                    updatedInputStockDtos.Add(updatedInputStockDto);
                }

                return updatedInputStockDtos;

            }
            catch (Exception e)
            {
                throw new Exception($"Error at Stock Service: {e.Message}");
            }
        }

        public async Task<List<InputStockDto>> RestockHistory()
        {
            List<InputStock> inputStocks = await _inputStockInterface.GetAllAsync();
            List<InputStockDto> inputStockDtos = new();
            foreach (var entity in inputStocks)
            {
                inputStockDtos.Add(StockMapper.EntityToDto(entity));
            }

            return inputStockDtos;
        }
    }
}
