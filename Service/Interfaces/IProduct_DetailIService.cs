﻿using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Service.Interfaces
{
    public interface IProduct_DetailIService
    {
        Task<Product_Details> Create(Product_DetailDto product_DetailDto);
        string Update(UpdateProduct_DetailsDto updateProduct_DetailsDto);
        bool Delete(int Id);
    }
}
