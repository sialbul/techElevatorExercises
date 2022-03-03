using RestSharp;
using System.Collections.Generic;
using AuctionApp.Models;
using System.Net.Http;

namespace AuctionApp.Services
{
    public class AuctionApiService
    {
        public IRestClient client;

        public AuctionApiService(string apiUrl)
        {
            client = new RestClient(apiUrl);
        }

        public List<Auction> GetAllAuctions()
        {
            RestRequest request = new RestRequest("auctions");

            IRestResponse<List<Auction>> response;
            response = client.Get<List<Auction>>(request);

            //Check for error
            ErrorMessage(response);


            //return "response data"; 
            return response.Data;

        }

        public Auction GetDetailsForAuction(int auctionId)
        {
            RestRequest request = new RestRequest($"auctions/{auctionId}");

            IRestResponse<Auction> response;
            response = client.Get<Auction>(request);

            //Check for error
            ErrorMessage(response);


            //return "response data"; 
            return response.Data;
        }

        public List<Auction> GetAuctionsSearchTitle(string searchTerm)
        {
            RestRequest request = new RestRequest($"auctions?title_like={searchTerm}");

            IRestResponse<List<Auction>> response;
            response = client.Get<List<Auction>>(request);

            //Check for error
            ErrorMessage(response);


            //return "response data"; 
            return response.Data;
        }

        public List<Auction> GetAuctionsSearchPrice(double searchPrice)
        {
            RestRequest request = new RestRequest($"auctions?currentBid_lte={searchPrice}&currentBid_lte<{searchPrice}");

            IRestResponse<List<Auction>> response;
            response = client.Get<List<Auction>>(request);

            //Check for error
            ErrorMessage(response);


            //return "response data"; 
            return response.Data;
        }

        private void ErrorMessage(IRestResponse response)
        {
            if (!response.IsSuccessful)
            {
                //TODO: Write a message into a log file for future. 

                throw new HttpRequestException($"There was an error in the call to the server {response.StatusCode}");
            }
        }
    }
}
