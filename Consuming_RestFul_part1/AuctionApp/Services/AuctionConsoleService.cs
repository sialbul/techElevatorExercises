using AuctionApp.Models;
using System;
using System.Collections.Generic;

namespace AuctionApp.Services
{
    public class AuctionConsoleService : ConsoleService
    {
        /************************************************************
            Print methods
        ************************************************************/
        public void PrintMainMenu()
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine("Menu:");
            Console.WriteLine("1: List all auctions");
            Console.WriteLine("2: List details for specific auction");
            Console.WriteLine("3: Find auctions with a specified term in the title");
            Console.WriteLine("4: Find auctions below a specified price");
            Console.WriteLine("0: Exit");
            Console.WriteLine("---------");
        }

        public void PrintAuctions(List<Auction> auctions)
        {
            Console.WriteLine("");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Auctions");
            Console.WriteLine("--------------------------------------------");
            foreach (Auction auction in auctions)
            {
                Console.WriteLine($"{auction.Id}: {auction.Title} | Current Bid: {auction.CurrentBid:C}");
            }
            Console.WriteLine("");
        }

        public void PrintAuction(Auction auction)
        {
            Console.WriteLine("");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Auction Details");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Id: " + auction.Id);
            Console.WriteLine("Title: " + auction.Title);
            Console.WriteLine("Description: " + auction.Description);
            Console.WriteLine("User: " + auction.User);
            Console.WriteLine("Current Bid: " + auction.CurrentBid.ToString("C"));
            Console.WriteLine("");
        }
    }
}
