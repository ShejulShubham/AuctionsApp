using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuctionApp.Data;
using AuctionApp.Models;
using AuctionApp.Data.Services;
using System.Security.Claims;

namespace AuctionApp.Controllers
{
    public class ListingsController : Controller
    {
        private readonly IListingService _listingService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IBidService _bidService;
        private readonly ICommentService _commentService;

        public ListingsController(IListingService context, IWebHostEnvironment _webEnvironment, IBidService bidService, ICommentService commentService)
        {
            _listingService = context;
            _webHostEnvironment = _webEnvironment;
            _bidService = bidService;
            _commentService = commentService;
        }

        // GET: Listings
        public async Task<IActionResult> Index(int? pageNumber, string searchString)
        {
            var applicationDbContext = _listingService.GetAllList();
            int pageSize = 3;
            if (!string.IsNullOrEmpty(searchString))
            {
                applicationDbContext = applicationDbContext.Where(a => a.Title.Contains(searchString));

                return View(await PaginatedList<Listing>.CreateAsync(applicationDbContext
                                            .Where(l => l.isSold == false)
                                            .AsNoTracking(), pageNumber ?? 1, pageSize));
            }

            return View(await PaginatedList<Listing>.CreateAsync(applicationDbContext
                                                        .Where(l => l.isSold == false)
                                                        .AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> MyListings(int? pageNumber)
        {
            var applicationDbContext = _listingService.GetAllList();
            int pageSize = 3;

            return View("Index", await PaginatedList<Listing>.CreateAsync(applicationDbContext
                                                        .Where(l => l.IdentityUserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                                                        .AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> MyBids(int? pageNumber)
        {
            var applicationDbContext = _bidService.GetAllBids();
            int pageSize = 3;

            return View(await PaginatedList<Bid>.CreateAsync(applicationDbContext
                                                        .Where(l => l.IdentityUserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                                                        .AsNoTracking(), pageNumber ?? 1, pageSize));
        }



        // GET: Listings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = await _listingService.GetListById(id);

            if (listing == null)
            {
                return NotFound();
            }

            return View(listing);
        }

        // GET: Listings/Create
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: Listings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ListingVM listingVM)
        {
            if (listingVM.Image != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");

                string fileName = listingVM.Image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    listingVM.Image.CopyTo(fileStream);
                }

                var listObj = new Listing
                {
                    Title = listingVM.Title,
                    Description = listingVM.Description,
                    Price = listingVM.Price,
                    IdentityUserId = listingVM.IdentityUserId,
                    ImagePath = fileName,
                };
                await _listingService.AddList(listObj);
                return RedirectToAction("Index");
            }

            return View(listingVM);
        }

        [HttpPost]
        public async Task<ActionResult> AddBid([Bind("Id, Price, ListingId, IdentityUserId")] Bid bid)
        {
            if (ModelState.IsValid)
            {
                await _bidService.AddBid(bid);
            }

            var listing = await _listingService.GetListById(bid.ListingId);
            listing.Price = bid.Price;
            await _listingService.SaveChanges();

            return View("Details",  listing);
        }

        public async Task<ActionResult> CloseBidding(int id)
        {
            var listing = await _listingService.GetListById(id);
            listing.isSold = true;
            await _listingService.SaveChanges();

            return View("Details", listing);
        }

        [HttpPost]
        public async Task<ActionResult> AddComment([Bind("Id, Content, ListingId, IdentityUserId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                await _commentService.AddComment(comment);
            }

            var listing = await _listingService.GetListById(comment.ListingId);

            return View("Details", listing);    
        }

            //    // GET: Listings/Edit/5
            //    public async Task<IActionResult> Edit(int? id)
            //    {
            //        if (id == null)
            //        {
            //            return NotFound();
            //        }

            //        var listing = await _context.Listings.FindAsync(id);
            //        if (listing == null)
            //        {
            //            return NotFound();
            //        }
            //        ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", listing.IdentityUserId);
            //        return View(listing);
            //    }

            //    // POST: Listings/Edit/5
            //    // To protect from overposting attacks, enable the specific properties you want to bind to.
            //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            //    [HttpPost]
            //    [ValidateAntiForgeryToken]
            //    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Price,ImagePath,isSold,IdentityUserId")] Listing listing)
            //    {
            //        if (id != listing.Id)
            //        {
            //            return NotFound();
            //        }

            //        if (ModelState.IsValid)
            //        {
            //            try
            //            {
            //                _context.Update(listing);
            //                await _context.SaveChangesAsync();
            //            }
            //            catch (DbUpdateConcurrencyException)
            //            {
            //                if (!ListingExists(listing.Id))
            //                {
            //                    return NotFound();
            //                }
            //                else
            //                {
            //                    throw;
            //                }
            //            }
            //            return RedirectToAction(nameof(Index));
            //        }
            //        ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", listing.IdentityUserId);
            //        return View(listing);
            //    }

            //    // GET: Listings/Delete/5
            //    public async Task<IActionResult> Delete(int? id)
            //    {
            //        if (id == null)
            //        {
            //            return NotFound();
            //        }

            //        var listing = await _context.Listings
            //            .Include(l => l.User)
            //            .FirstOrDefaultAsync(m => m.Id == id);
            //        if (listing == null)
            //        {
            //            return NotFound();
            //        }

            //        return View(listing);
            //    }

            //    // POST: Listings/Delete/5
            //    [HttpPost, ActionName("Delete")]
            //    [ValidateAntiForgeryToken]
            //    public async Task<IActionResult> DeleteConfirmed(int id)
            //    {
            //        var listing = await _context.Listings.FindAsync(id);
            //        if (listing != null)
            //        {
            //            _context.Listings.Remove(listing);
            //        }

            //        await _context.SaveChangesAsync();
            //        return RedirectToAction(nameof(Index));
            //    }

            //    private bool ListingExists(int id)
            //    {
            //        return _context.Listings.Any(e => e.Id == id);
            //    }
        }
}
