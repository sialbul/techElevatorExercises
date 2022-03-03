using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AuctionApp.Models;
using AuctionApp.DAO;
using System;

namespace AuctionApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuctionsController : ControllerBase
    {
        private readonly IAuctionDao dao;

        public AuctionsController(IAuctionDao auctionDao = null)
        {
            if (auctionDao == null)
            {
                dao = new AuctionMemoryDao();
            }
            else
            {
                dao = auctionDao;
            }
        }


        [HttpGet("/")]
        public List<Auction> Auctions()
        {
            return dao.List();

        }

        [HttpGet("{id}")]
        public ActionResult<Auction> GetAuction(int id)
        {
            Auction auction = dao.Get(id);

            if (auction != null)
            {
                return auction;
            }
            else
            {
                return NoContent();
            }

        }



        [HttpPost]
        public ActionResult<Auction> AddAuction(Auction newAuction)
        {
            Auction added = dao.Create(newAuction);
         
            return Created($"/auctions/{added.Id}", added);
        }



        [HttpGet("/auctions")]
        public List<Auction> FilterByTitle(double currentBid_lte = 0, string title_like = "")
        {
            List<Auction> filteredAuctions = new List<Auction>();

            List<Auction> allAuctions = dao.List();


            //Foreach through the hotels. 
            foreach (Auction auction in allAuctions)
            {

                if (auction != null)
                {
                    if (currentBid_lte > 0 && title_like != null)
                    {
                        if (auction.Title.ToLower().Contains(title_like.ToLower()) && auction.CurrentBid <= currentBid_lte)
                        {
                            filteredAuctions.Add(auction);
                        }
                    }
                    else
                    {
                      
                        if (auction.Title.ToLower().Contains(title_like.ToLower()))
                        {
                            filteredAuctions.Add(auction);
                        }

                        if (auction.CurrentBid <= currentBid_lte)
                        {
                            filteredAuctions.Add(auction);

                        }
                    }
                }
               

            }

            return filteredAuctions;
        }



        
    }
}
