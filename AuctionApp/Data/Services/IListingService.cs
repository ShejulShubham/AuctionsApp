using AuctionApp.Models;

namespace AuctionApp.Data.Services
{
    public interface IListingService
    {
        IQueryable<Listing> GetAllList();
        Task AddList(Listing listing);
        Task <Listing> GetListById(int? id);
        Task SaveChanges();
    }
}
