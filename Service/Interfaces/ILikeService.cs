using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Service.Interfaces
{
    public interface ILikeService
    {
        Task<Like> Create(Like entity);
        Task<ProductLikeCountDto> GetLikeCountByProductId(int productId);
    }
}
