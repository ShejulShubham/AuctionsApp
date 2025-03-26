using AuctionApp.Models;

namespace AuctionApp.Data.Services
{
    public interface IBidService
    {
        Task AddBid(Bid bid);

        IQueryable<Bid> GetAllBids();
    }
}
