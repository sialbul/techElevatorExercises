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
                int menuSelection = console.PromptForInteger("Please choose an option", 0, 7);

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

                if (menuSelection == 5)
                {
                    // Create a new auction
                    AddAuction();
                }

                if (menuSelection == 6)
                {
                    // Modify an auction
                    UpdateAuction();
                }

                if (menuSelection == 7)
                {
                    // Delete an auction
                    DeleteAuction();
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

        private void AddAuction()
        {
            Auction auctionToAdd = console.PromptForAuctionData();
            if (auctionToAdd.IsValid)
            {


                try
                {
                    Auction addedAuction = auctionApiService.AddAuction(auctionToAdd);
                    if (addedAuction != null)
                    {
                        console.PrintSuccess($"Auction {addedAuction.Id} successfully added.");
                    }
                    else
                    {
                        console.PrintError("Auction not added.");
                    }
                }
                catch (Exception ex)
                {
                    console.PrintError(ex.Message);
                }
            }
            else
            {
                console.PrintError("Invalid data");
            }
            console.Pause();
        }

        private void UpdateAuction()
        {
            // Update an existing auction
            try
            {
                List<Auction> auctions = auctionApiService.GetAllAuctions();
                if (auctions != null)
                {
                    int auctionId = console.PromptForAuctionId(auctions, "update");
                    if (auctionId == 0)
                    {
                        // User cancel
                        return;
                    }

                    Auction oldAuction = auctionApiService.GetDetailsForAuction(auctionId);
                    if (oldAuction != null)
                    {
                        Auction auctionToUpdate = console.PromptForAuctionData(oldAuction);
                        if (auctionToUpdate.IsValid)
                        {
                            Auction updatedAuction = auctionApiService.UpdateAuction(auctionToUpdate);
                            if (updatedAuction != null)
                            {
                                console.PrintSuccess("Auction successfully updated.");
                            }
                            else
                            {
                                console.PrintError("Auction not updated.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                console.PrintError(ex.Message);
            }
            console.Pause();
        }

        private void DeleteAuction()
        {
            // Delete auction
            try
            {
                List<Auction> auctions = auctionApiService.GetAllAuctions();
                if (auctions != null)
                {
                    int auctionId = console.PromptForAuctionId(auctions, "delete");
                    if (auctionId == 0)
                    {
                        // User cancel
                        return;
                    }

                    bool deleteSuccess = auctionApiService.DeleteAuction(auctionId);
                    if (deleteSuccess)
                    {
                        console.PrintSuccess("Auction successfully deleted.");
                    }
                    else
                    {
                        console.PrintError("Auction not deleted.");
                    }
                }
            }
            catch (Exception ex)
            {
                console.PrintError(ex.Message);
            }
            console.Pause();
        }
    }
}
