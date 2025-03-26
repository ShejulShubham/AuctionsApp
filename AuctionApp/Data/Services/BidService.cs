using AuctionApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionApp.Data.Services
{
    public class BidService : IBidService
    {
        private readonly ApplicationDbContext _context;

        public BidService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddBid(Bid bid)
        {
            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Bid> GetAllBids()
        {
            var applicationDbContext = from a in _context.Bids.Include(l => l.Listing).ThenInclude(l => l.User)
                                       select a;

            return applicationDbContext;
        }
    }
}
