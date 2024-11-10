using Data.Dto;
using Microsoft.AspNetCore.Http;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Service.Interfaces
{
    public interface IProductsIService
    {
        Task<Products> Create(ProductDto productsDto);
        Task<UpdateProductDto> Update(int id, UpdateProductDto updateProductDto);
        Task<bool> DeleteAsync(int Id);
        List<Object> GetBrandName(string keyword);
        List<Object> GetTypeName(string keyword, int limit = 6);
        List<Object> GetPriceHasDecreased();
        byte[] GetProductImageBytes(string imagePath);
        List<ProductGetAllInfPriceDto> GetProductsWithinPriceRangeAndBrand(string productType, decimal minPrice, decimal maxPrice, string brandsName);
        ProductDetailWithColors GetProductsByNameAndColor(string productName, string? color);
        Task<List<GetViewProductType>> GetProductType(string ProductType);
        Task<List<ProductTypeInfDto>> GetProductName(string productName);
        List<ProductGetAllInfDto> GetAllProduct();
    }
}
