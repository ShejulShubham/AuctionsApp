# **Project Details: AuctionApp**  

## **Project Overview**  
**Project Name:** AuctionApp  
**Tech Stack:**  
- **Backend:** .NET (ASP.NET Core MVC or Web API)  
- **Frontend:** Bootstrap CSS, Razor Views (or JavaScript for API)  
- **Database:** MS SQL Server  

## **Key Features**  
1. **Responsive UI**  
   - Built using **Bootstrap 5** for mobile-first design.  
   - Works seamlessly on desktop, tablet, and mobile devices.  
   - Custom styling with Bootstrap utilities and components.  

2. **Bootstrap CSS Integration**  
   - Used Bootstrap’s grid system, cards, modals, and forms.  
   - Custom themes with Bootstrap variables for consistent styling.  
   - Responsive navigation bar with dropdown menus.  

3. **Auction & Bidding Functionality**  
   - **Post Auctions:** Users can create new auction listings with title, description, starting bid, and end date.  
   - **Post Bids:** Authenticated users can place bids on active auctions.  
   - **Add Images:** Upload and display multiple images per auction.  
   - **Comments Section:** Users can leave comments on auction listings.  

4. **Database Management (MS SQL Server)**  
   - **Tables & Relations:**  
     - **Users** (Id, Username, Email, PasswordHash)  
     - **Auctions** (Id, Title, Description, StartBid, CurrentBid, StartTime, EndTime, SellerId)  
     - **Bids** (Id, Amount, BidTime, AuctionId, BidderId)  
     - **Comments** (Id, Text, PostedAt, UserId, AuctionId)  
