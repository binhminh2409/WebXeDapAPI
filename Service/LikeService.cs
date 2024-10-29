using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebXeDapAPI.Data;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;
using WebXeDapAPI.Service.Interfaces;

namespace WebXeDapAPI.Service
{
    public class LikeService : ILikeService
    {
        private readonly ApplicationDbContext _DbContext;
        public LikeService(ApplicationDbContext context)
        {
            _DbContext = context;
        }
        public async Task<Like> Create(Like entity)
        {
            try
            {
                var product = await _DbContext.Products.FirstOrDefaultAsync(x => x.Id == entity.ProductId);
                if (product == null)
                {
                    throw new Exception("ProductId not found");
                }
                var like = new Like
                {
                    UserId = entity.UserId,
                    ProductId = product.Id,
                    CreatedAt = DateTime.Now
                };
                await _DbContext.Likes.AddAsync(like);
                await _DbContext.SaveChangesAsync();

                return like;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while creating like: {ex.Message}");
            }
        }


        public async Task<ProductLikeCountDto> GetLikeCountByProductId(int productId)
        {
            try
            {
                var likeCount = await _DbContext.Likes.CountAsync(x => x.ProductId == productId);
                return new ProductLikeCountDto
                {
                    ProductId = productId,
                    LikeCount = likeCount
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while getting like count: {ex.Message}");
            }
        }
    }
}
