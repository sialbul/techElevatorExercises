using System;

namespace AuctionApp.Models
{
    public class Auction
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string User { get; set; }
        public double CurrentBid { get; set; }

        public Auction()
        {
            //must have parameterless constructor to use as a type parameter (i.e., client.Get<Auction>())
        }

        public Auction(int id, string title, string description, string user, double currentBid)
        {
            Id = id;
            Title = title;
            Description = description;
            User = user;
            CurrentBid = currentBid;
        }

        public bool IsValid
        {
            get
            {
                return Title != null && Description != null && User != null && CurrentBid != 0;
            }
        }
    }
}
