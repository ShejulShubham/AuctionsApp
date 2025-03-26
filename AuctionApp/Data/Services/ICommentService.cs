using AuctionApp.Models;

namespace AuctionApp.Data.Services
{
    public interface ICommentService
    {
        Task AddComment(Comment comment);
    }
}
