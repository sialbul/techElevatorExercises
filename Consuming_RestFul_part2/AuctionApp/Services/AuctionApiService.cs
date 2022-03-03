using RestSharp;
using System.Collections.Generic;
using AuctionApp.Models;
using System.Net.Http;
using System;

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
            IRestResponse<List<Auction>> response = client.Get<List<Auction>>(request);
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new HttpRequestException("Error occurred - unable to reach server.", response.ErrorException);
            }
            else if (!response.IsSuccessful)
            {
                throw new HttpRequestException("Error occurred - received non-success response: " + (int)response.StatusCode);
            }
            return response.Data;
        }

        public Auction GetDetailsForAuction(int auctionId)
        {
            RestRequest requestOne = new RestRequest($"auctions/{auctionId}");
            IRestResponse<Auction> response = client.Get<Auction>(requestOne);
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new HttpRequestException("Error occurred - unable to reach server.", response.ErrorException);
            }
            else if (!response.IsSuccessful)
            {
                throw new HttpRequestException("Error occurred - received non-success response: " + (int)response.StatusCode);
            }
            return response.Data;
        }

        public List<Auction> GetAuctionsSearchTitle(string searchTerm)
        {
            RestRequest request = new RestRequest($"auctions?title_like={searchTerm}");
            IRestResponse<List<Auction>> response = client.Get<List<Auction>>(request);
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new HttpRequestException("Error occurred - unable to reach server.", response.ErrorException);
            }
            else if (!response.IsSuccessful)
            {
                throw new HttpRequestException("Error occurred - received non-success response: " + (int)response.StatusCode);
            }
            return response.Data;
        }

        public List<Auction> GetAuctionsSearchPrice(double searchPrice)
        {
            RestRequest request = new RestRequest($"auctions?currentBid_lte={searchPrice}");
            IRestResponse<List<Auction>> response = client.Get<List<Auction>>(request);
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new HttpRequestException("Error occurred - unable to reach server.", response.ErrorException);
            }
            else if (!response.IsSuccessful)
            {
                throw new HttpRequestException("Error occurred - received non-success response: " + (int)response.StatusCode);
            }
            return response.Data;
        }

        public Auction AddAuction(Auction newAuction)
        {

            RestRequest request = new RestRequest($"auctions");

            request.AddJsonBody(newAuction);

            IRestResponse<Auction> response = client.Post<Auction>(request);

            CheckForError(response, "Add new auction");
            return response.Data;
        }

        public Auction UpdateAuction(Auction auctionToUpdate)
        {
            RestRequest request = new RestRequest($"auctions/{auctionToUpdate.Id}");

            request.AddJsonBody(auctionToUpdate);

            IRestResponse<Auction> response = client.Put<Auction>(request);

            CheckForError(response, $"Update auction{auctionToUpdate.Id}");
            return response.Data;
        }

        public bool DeleteAuction(int auctionId)
        {
            RestRequest request = new RestRequest($"auctions/{auctionId}");

            IRestResponse response = client.Delete(request);

            CheckForError(response, $"Delete auction{auctionId}");

            return true;

        }

        private void CheckForError(IRestResponse response, string action)
        {
            string message = string.Empty;

            string messageDetails = "";
            //Cannot connect to the server
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                message = $"Error occured in '{action}'- unable to reach server";
                messageDetails = $"Action:{action}{Environment.NewLine}" + $"\tResponse status was '{response.ResponseStatus}'.";

                if (response.ErrorException != null)
                {
                    messageDetails += $"{Environment.NewLine}\t{response.ErrorException.Message}";
                }
            }
            //Connected, got a response, but something went wrong
            else if (!response.IsSuccessful)
            {
                // TODO: Write a log message for future reference
                message = "An http error occured.";
                messageDetails = $"Action '{action}'{Environment.NewLine} " + $"\tResponse : {(int)response.StatusCode} {response.StatusDescription}";
            }
            if (message.Length > 0)
            {
                throw new HttpRequestException(message, response.ErrorException);
            }
        }
    }
}
