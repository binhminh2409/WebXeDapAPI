﻿using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Service.Interfaces
{
    public interface IBrandIService
    {
        public Brand Create(Brand brand);
        string Update(BrandDto brandDto);
        bool Delete(int id);

    }
}
