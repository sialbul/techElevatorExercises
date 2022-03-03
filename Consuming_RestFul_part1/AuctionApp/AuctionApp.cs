using AuctionApp.Models;
using AuctionApp.Services;
using System;
using System.Collections.Generic;

namespace AuctionApp
{
    public class AuctionApp
    {
        private readonly AuctionApiService auctionApiService;
        private readonly AuctionConsoleService console = new AuctionConsoleService();

        public AuctionApp(string apiUrl)
        {
            this.auctionApiService = new AuctionApiService(apiUrl);
        }

        public void Run()
        {
            while (true)
            {
                console.PrintMainMenu();
                int menuSelection = console.PromptForInteger("Please choose an option", 0, 4);

                if (menuSelection == 0)
                {
                    // Exit the loop
                    break;
                }

                if (menuSelection == 1)
                {
                    // List auctions
                    ShowAuctions();
                }

                if (menuSelection == 2)
                {
                    // Show a single auction
                    ShowAuction();
                }

                if (menuSelection == 3)
                {
                    // Search for auctions with a term
                    ShowAuctionsWithTerm();
                }

                if (menuSelection == 4)
                {
                    // Search for auctions below a price
                    ShowAuctionsBelowPrice();
                }
            }
        }

        private void ShowAuctions()
        {
            try
            {
                List<Auction> auctions = auctionApiService.GetAllAuctions();
                console.PrintAuctions(auctions);
            }
            catch (Exception ex)
            {
                console.PrintError(ex.Message);
            }
            console.Pause();
        }

        private void ShowAuction()
        {
            try
            {
                int auctionId = console.PromptForInteger("Please enter an auction id to get the details", 0);
                if (auctionId == 0)
                {
                    // user cancel
                    return;
                }
                Auction auction = auctionApiService.GetDetailsForAuction(auctionId);
                console.PrintAuction(auction);
            }
            catch (Exception ex)
            {
                console.PrintError(ex.Message);
            }
            console.Pause();
        }

        private void ShowAuctionsWithTerm()
        {
            try
            {
                string searchTerm = console.PromptForString("Please enter a term to search for");
                if (searchTerm.Length == 0)
                {
                    // User cancel
                    return;
                }
                List<Auction> auctions = auctionApiService.GetAuctionsSearchTitle(searchTerm);
                console.PrintAuctions(auctions);
            }
            catch (Exception ex)
            {
                console.PrintError(ex.Message);
            }
            console.Pause();
        }

        private void ShowAuctionsBelowPrice()
        {
            try
            {
                double searchPrice = console.PromptForDouble("Please enter a max price to search for");
                List<Auction> auctions = auctionApiService.GetAuctionsSearchPrice(searchPrice);
                console.PrintAuctions(auctions);
            }
            catch (Exception ex)
            {
                console.PrintError(ex.Message);
            }
            console.Pause();
        }
    }
}
