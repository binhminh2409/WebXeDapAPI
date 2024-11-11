using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Service.Interfaces
{
    public interface ICommentService
    {
        Task<CommentDto> Create(CommentDto commentDto);
        Task<Comment> Update(Comment comment);
        Task<Comment> Delete(Comment comment);
        Task<List<Comment>> GetCommentsByUserId(int productId);

        Task<List<Comment>> GetAll(int productId);

    }
}
