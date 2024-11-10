using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Service.Interfaces
{
    public interface IAdsIService
    {
        Task<Ads> Create(AdsDto adsDto);
        Task<Ads> Update(Ads ads);
        Task<Ads> Delete(int id);
        byte[] GetAdsBytesImage(string imagePath);
        ICollection<Ads> GetAll();
    }
}
