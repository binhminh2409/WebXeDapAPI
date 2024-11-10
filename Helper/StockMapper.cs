// ProductName
// Price
// BrandId
// TypeId
// Colors

using System;
using System.Xml;
using Data.Dto;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;
using WebXeDapAPI.Models.Enum;
using WebXeDapAPI.Utilities;

namespace WebXeDapAPI.Helper
{
    public class StockMapper
    {
        public static Stock DtoToEntity(StockDto dto,  Products product)
        {
            if (dto == null) return null;

            return new Stock
            {
                Id = dto.Id,
                Product = product,
                Quantity = dto.Quantity,
                UpdatedTime = dto.UpdatedTime
            };
        }

        public static StockDto EntityToDto(Stock stock)
        {
            if (stock == null) throw new ArgumentNullException(nameof(stock));

            StockDto dto = new StockDto
            {
                Id = stock.Id,
                Product = new ProductDto {
                    Id = stock.Product.Id,
                    ProductName = stock.Product.ProductName,
                    Price = stock.Product.Price,
                    BrandId = stock.Product.BrandId,
                    TypeId = stock.Product.TypeId,
                    Colors = stock.Product.Colors 
                },
                Quantity = stock.Quantity,
                UpdatedTime = stock.UpdatedTime
            };
            return dto;
        }

        public static InputStock DtoToEntity(InputStockDto dto,  Products product)
        {
            if (dto == null) return null;

            return new InputStock
            {
                Id = dto.Id ?? 0,
                Product = product,
                Quantity = dto.Quantity,
                CreatedTime = dto.CreatedTime,
                Price = dto.Price,
                TotalPrice = dto.TotalPrice,
                BatchNo_ = dto.BatchNo_,
                Paid = dto.Paid,
                ReturnReason = "",
                Status = StockEnumHelper.StringToEnum(dto.Status),
                Type = dto.Type,
                UserId = dto.UserId
            };
        }

        public static InputStockDto EntityToDto(InputStock inputStock)
        {
            if (inputStock == null) throw new ArgumentNullException(nameof(inputStock));

            InputStockDto dto = new InputStockDto
            {
                Id = inputStock.Id,
                Product = new ProductDto {
                    ProductName = inputStock.Product.ProductName,
                    Price = inputStock.Product.Price,
                    BrandId = inputStock.Product.BrandId,
                    TypeId = inputStock.Product.TypeId,
                    Colors = inputStock.Product.Colors 
                },
                Quantity = inputStock.Quantity,
                CreatedTime = inputStock.CreatedTime,
                Price = inputStock.Price,
                Paid = inputStock.Paid,
                ReturnReason = inputStock.ReturnReason,
                Status = inputStock.Status.ToString(),
                TotalPrice = inputStock.TotalPrice,
                BatchNo_ = inputStock.BatchNo_,
                Type = inputStock.Type,
                UserId = inputStock.UserId
            };
            return dto;
        }

    }
}
