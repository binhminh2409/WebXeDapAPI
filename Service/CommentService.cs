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
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _DBContext;
        public CommentService(ApplicationDbContext dbContext)
        {
            _DBContext = dbContext;
        }
        public async Task<CommentDto> Create(CommentDto commentDto)
        {
            try
            {
                var user = await _DBContext.Users.FirstOrDefaultAsync(x => x.Id == commentDto.UserId);
                if (user == null)
                {
                    throw new Exception("UserId not found");
                }
                var product = await _DBContext.Products.FirstOrDefaultAsync(x => x.Id == commentDto.ProductId);
                if (product == null)
                {
                    throw new Exception("ProductId not found");
                }
                if (string.IsNullOrWhiteSpace(commentDto.Description))
                {
                    throw new Exception("Description cannot be empty");
                }
                var comment = new Comment
                {
                    UserId = user.Id,
                    ProductId = product.Id,
                    Description = commentDto.Description,
                    DateTime = DateTime.Now,
                };
                await _DBContext.Comments.AddAsync(comment);
                await _DBContext.SaveChangesAsync();
                return commentDto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while creating comment: {ex.Message}");
            }
        }

        public Task<Comment> Delete(Comment comment)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Comment>> GetCommentsByUserId(int userId, int productId)
        {
            try
            {
                // Querying comments by both userId and productId
                var comments = await _DBContext.Comments
                    .Where(x => x.UserId == userId && x.ProductId == productId)
                    .ToListAsync();

                // Check if comments list is empty
                if (comments == null || !comments.Any())
                {
                    throw new Exception("No comments found for this user and product.");
                }

                return comments;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        public async Task<List<Comment>> GetAll( int productId)
        {
            try
            {
                // Querying comments by both userId and productId
                var comments = await _DBContext.Comments
                    .Where(x => x.ProductId == productId)
                    .ToListAsync();

                // Check if comments list is empty
                if (comments == null || !comments.Any())
                {
                    throw new Exception("No comments found for this user and product.");
                }

                return comments;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        public Task<Comment> Update(Comment comment)
        {
            throw new NotImplementedException();
        }
    }
    
}
