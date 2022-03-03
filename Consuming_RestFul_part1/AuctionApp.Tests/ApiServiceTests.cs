using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using RestSharp;
using System.Net;
using AuctionApp.Models;
using AuctionApp.Services;

namespace AuctionApp.Tests
{
    [TestClass]
    public class ApiServiceTests
    {
        private const string baseApiUrl = "http://localhost:3000";
        private AuctionApiService apiService;

        private static readonly List<Auction> expectedAuctions = new List<Auction>
        {
            new Auction() { Id = 1, Title = "Bell Computer Monitor", Description = "4K LCD monitor from Bell Computers, HDMI & DisplayPort", User = "Queenie34", CurrentBid = 100.39 },
            new Auction() { Id = 2, Title = "Pineapple Smart Watch", Description = "Pears with Pineapple ePhone", User = "Miller.Fahey", CurrentBid = 377.44 },
            new Auction() { Id = 3, Title = "Mad-dog Sneakers", Description = "Soles check. Laces check.", User = "Cierra_Pagac", CurrentBid = 125.23 },
            new Auction() { Id = 4, Title = "Annie Sunglasses", Description = "Keep the sun from blinding you", User = "Sallie_Kerluke4", CurrentBid = 69.67 },
            new Auction() { Id = 5, Title = "Byson Vacuum", Description = "Clean your house with a spherical vacuum", User = "Lisette_Crist", CurrentBid = 287.73 },
            new Auction() { Id = 6, Title = "Fony Headphones", Description = "Listen to music, movies, games and not bother people around you!", User = "Chester67", CurrentBid = 267.38 },
            new Auction() { Id = 7, Title = "Molex Gold Watch", Description = "Definitely not fake gold watch", User = "Stuart27", CurrentBid = 188.39 }
        };
        private static readonly Auction expectedAuction = expectedAuctions[0];

        private static bool CompareUrl(string clientBase, string resource, string fullResource)
        {
            return
                fullResource == clientBase + resource ||
                fullResource == resource ||
                fullResource == clientBase + resource.Substring(1); // In case there's a starting slash
        }

        [TestInitialize]
        public void Setup()
        {
            // Create a new api service
            apiService = new AuctionApiService(baseApiUrl);
        }

        [TestMethod]
        public void GetAllAuctions_ExpectList()
        {
            // Arrange
            string fullResource = $"{baseApiUrl}/auctions";
            string clientBase = apiService.client.BaseUrl.ToString();
            Mock<IRestClient> restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.Execute<List<Auction>>(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource)), Method.GET))
                .Returns(new RestResponse<List<Auction>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = expectedAuctions,
                    ResponseStatus = ResponseStatus.Completed
                });
            apiService.client = restClient.Object;

            // Act
            List<Auction> actualAuctions = apiService.GetAllAuctions();

            // Assert
            restClient.Verify(x => x.Execute<List<Auction>>(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource)), Method.GET), Times.Once(), "The API wasn't called or called too many times.");
            actualAuctions.Should().BeEquivalentTo(expectedAuctions, "a list of all auctions is expected.");
        }

        [TestMethod]
        public void GetDetailsForAuction_ExpectSpecificItem()
        {
            // Arrange
            string fullResource = $"{baseApiUrl}/auctions/1";
            string clientBase = apiService.client.BaseUrl.ToString();
            Mock<IRestClient> restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.Execute<Auction>(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource)), Method.GET))
                .Returns(new RestResponse<Auction>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = expectedAuction,
                    ResponseStatus = ResponseStatus.Completed
                });
            apiService.client = restClient.Object;

            // Act
            Auction actualAuction = apiService.GetDetailsForAuction(1);

            // Assert
            restClient.Verify(x => x.Execute<Auction>(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource)), Method.GET), Times.Once(), "The API wasn't called or called too many times.");
            actualAuction.Should().BeEquivalentTo(expectedAuction, "the auction with the ID 1 is expected.");
        }

        [TestMethod]
        public void GetDetailsForAuction_IdNotFound()
        {
            // Arrange
            string fullResource = $"{baseApiUrl}/auctions/99";
            string clientBase = apiService.client.BaseUrl.ToString();
            Mock<IRestClient> restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.Execute<Auction>(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource)), Method.GET))
                .Returns(new RestResponse<Auction>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = null,
                    ResponseStatus = ResponseStatus.Completed
                });
            apiService.client = restClient.Object;

            // Act
            Auction actualAuction = apiService.GetDetailsForAuction(99);

            // Assert
            restClient.Verify(x => x.Execute<Auction>(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource)), Method.GET), Times.Once(), "The API wasn't called or called too many times.");
            actualAuction.Should().BeNull("An auction isn't expected for an invalid ID.");
        }

        [TestMethod]
        public void GetAuctionsSearchTitle_ExpectList()
        {
            // Arrange
            string fullResource = $"{baseApiUrl}/auctions";
            string clientBase = apiService.client.BaseUrl.ToString();
            Mock<IRestClient> restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.Execute<List<Auction>>(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource)
                && r.Parameters.Count > 0 && r.Parameters[0].Name == "title_like" && (string)r.Parameters[0].Value == "watch"), Method.GET))
                .Returns(new RestResponse<List<Auction>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = new List<Auction>() { expectedAuctions[0], expectedAuctions[6] },
                    ResponseStatus = ResponseStatus.Completed
                });
            apiService.client = restClient.Object;

            // Act
            List<Auction> actualAuctions = apiService.GetAuctionsSearchTitle("watch");

            // Assert
            restClient.Verify(x => x.Execute<List<Auction>>(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource)
                && r.Parameters.Count > 0 && r.Parameters[0].Name == "title_like" && (string)r.Parameters[0].Value == "watch"), Method.GET), Times.Once(), "The API wasn't called or called too many times.");
            actualAuctions.Should().BeEquivalentTo(new List<Auction>() { expectedAuctions[0], expectedAuctions[6] }, "Expected auctions not returned for search value.");
        }

        [TestMethod]
        public void GetAuctionsSearchTitle_ExpectNone()
        {
            // Arrange
            string fullResource = $"{baseApiUrl}/auctions";
            string clientBase = apiService.client.BaseUrl.ToString();
            Mock<IRestClient> restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.Execute<List<Auction>>(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource)
                && r.Parameters.Count > 0 && r.Parameters[0].Name == "title_like" && (string)r.Parameters[0].Value == "nosuchtitle"), Method.GET))
                .Returns(new RestResponse<List<Auction>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = new List<Auction>(),
                    ResponseStatus = ResponseStatus.Completed
                });
            apiService.client = restClient.Object;

            // Act
            List<Auction> actualAuctions = apiService.GetAuctionsSearchTitle("nosuchtitle");

            // Assert
            restClient.Verify(x => x.Execute<List<Auction>>(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource)
                && r.Parameters.Count > 0 && r.Parameters[0].Name == "title_like" && (string)r.Parameters[0].Value == "nosuchtitle"), Method.GET), Times.Once(), "The API wasn't called or called too many times.");
            actualAuctions.Should().BeEquivalentTo(new List<Auction>(), "An empty list of Auctions is expected.");
        }

        [TestMethod]
        public void GetAuctionsSearchPrice_ExpectList()
        {
            // Arrange
            string fullResource = $"{baseApiUrl}/auctions";
            string clientBase = apiService.client.BaseUrl.ToString();
            Mock<IRestClient> restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.Execute<List<Auction>>(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource)
                && r.Parameters.Count > 0 && r.Parameters[0].Name == "currentBid_lte" && (string)r.Parameters[0].Value == "200"), Method.GET))
                .Returns(new RestResponse<List<Auction>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = new List<Auction>() { expectedAuctions[0], expectedAuctions[2], expectedAuctions[3], expectedAuctions[6] },
                    ResponseStatus = ResponseStatus.Completed
                });
            apiService.client = restClient.Object;

            // Act
            List<Auction> actualAuctions = apiService.GetAuctionsSearchPrice(200);

            // Assert
            restClient.Verify(x => x.Execute<List<Auction>>(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource)
                && r.Parameters.Count > 0 && r.Parameters[0].Name == "currentBid_lte" && (string)r.Parameters[0].Value == "200"), Method.GET), Times.Once(), "The API wasn't called or called too many times.");
            actualAuctions.Should().BeEquivalentTo(new List<Auction>() { expectedAuctions[0], expectedAuctions[2], expectedAuctions[3], expectedAuctions[6] }, "Expected auctions not returned for search price.");
        }

        [TestMethod]
        public void GetAuctionsSearchPrice_ExpectNone()
        {
            // Arrange
            string fullResource = $"{baseApiUrl}/auctions";
            string clientBase = apiService.client.BaseUrl.ToString();
            Mock<IRestClient> restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.Execute<List<Auction>>(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource)
                && r.Parameters.Count > 0 && r.Parameters[0].Name == "currentBid_lte" && (string)r.Parameters[0].Value == "0"), Method.GET))
                .Returns(new RestResponse<List<Auction>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = new List<Auction>(),
                    ResponseStatus = ResponseStatus.Completed
                });
            apiService.client = restClient.Object;

            // Act
            List<Auction> actualAuctions = apiService.GetAuctionsSearchPrice(0.0);

            // Assert
            restClient.Verify(x => x.Execute<List<Auction>>(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource)
                && r.Parameters.Count > 0 && r.Parameters[0].Name == "currentBid_lte" && (string)r.Parameters[0].Value == "0"), Method.GET), Times.Once(), "The API wasn't called or called too many times.");
            actualAuctions.Should().BeEquivalentTo(new List<Auction>(), "Expected no auctions returned for search price of $0.");
        }
    }
}
