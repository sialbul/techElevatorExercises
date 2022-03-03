using System;
using System.Collections.Generic;
using AuctionApp.Models;

namespace AuctionApp.DAO
{
    public class AuctionMemoryDao : IAuctionDao
    {
        private static List<Auction> auctions;

        public AuctionMemoryDao()
        {
            if (auctions == null)
            {
                auctions = new List<Auction>
                {
                    new Auction() { Id = 1, Title = "Bell Computer Monitor", Description = "4K LCD monitor from Bell Computers, HDMI & DisplayPort", User = "Queenie34", CurrentBid = 100.39 },
                    new Auction() { Id = 2, Title = "Pineapple Smart Watch", Description = "Pears with Pineapple ePhone", User = "Miller.Fahey", CurrentBid = 377.44 },
                    new Auction() { Id = 3, Title = "Mad-dog Sneakers", Description = "Soles check. Laces check.", User = "Cierra_Pagac", CurrentBid = 125.23 },
                    new Auction() { Id = 4, Title = "Annie Sunglasses", Description = "Keep the sun from blinding you", User = "Sallie_Kerluke4", CurrentBid = 69.67 },
                    new Auction() { Id = 5, Title = "Byson Vacuum", Description = "Clean your house with a spherical vacuum", User = "Lisette_Crist", CurrentBid = 287.73 },
                    new Auction() { Id = 6, Title = "Fony Headphones", Description = "Listen to music, movies, games and not bother people around you!", User = "Chester67", CurrentBid = 267.38 },
                    new Auction() { Id = 7, Title = "Molex Gold Watch", Description = "Definitely not fake gold watch", User = "Stuart27", CurrentBid = 188.39 }
                };
            }
        }

        public List<Auction> List()
        {
            return auctions;
        }

        public Auction Get(int id)
        {
            foreach (var auction in auctions)
            {
                if (auction.Id == id)
                {
                    return auction;
                }
            }

            return null;
        }

        public Auction Create(Auction auction)
        {
            if (auction.IsValid)
            {
                auction.Id = NextId();
                auctions.Add(auction);
                return auction;
            }
            return null;
        }

        private int NextId()
        {
            int maxId = 0;
            foreach (Auction auction in auctions)
            {
                if (auction.Id > maxId)
                {
                    maxId = auction.Id;
                }
            }
            return maxId + 1;
        }

        public List<Auction> SearchByTitle(string searchTerm)
        {
            List<Auction> matchTitles = new List<Auction>();

            if (searchTerm.Length > 0)
            {
                foreach (Auction auction in auctions)
                {
                    if (auction.Title.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase))
                    {
                        matchTitles.Add(auction);
                    }
                }
            }

            return matchTitles;
        }

        public List<Auction> SearchByPrice(double maxPrice)
        {
            List<Auction> matchPrices = new List<Auction>();
            foreach (Auction auction in auctions)
            {
                if (auction.CurrentBid <= maxPrice)
                {
                    matchPrices.Add(auction);
                }
            }

            return matchPrices;
        }

        public List<Auction> SearchByTitleAndPrice(string searchTerm, double maxPrice)
        {
            List<Auction> matches = new List<Auction>();
            foreach (Auction auction in auctions)
            {
                if (auction.Title.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) && auction.CurrentBid <= maxPrice)
                {
                    matches.Add(auction);
                }
            }

            return matches;
        }
    }
}
