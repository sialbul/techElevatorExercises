using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Moq;
using RestSharp;
using System.Net;
using AuctionApp.Models;
using AuctionApp.Services;
using System.Net.Http;

namespace AuctionApp.Tests
{
    [TestClass]
    public class ApiServiceTests
    {
        private const string baseApiUrl = "http://localhost:3000";
        private AuctionApiService apiService;

        private static bool CompareUrl(string clientBase, string resource, string fullResource)
        {
            return
                fullResource == clientBase + resource ||
                fullResource == resource ||
                fullResource == clientBase + resource.Substring(1); // In case there's a starting slash
        }

        private bool IsRequestBodyValid(IRestRequest r, bool shouldContainData)
        {
            if (shouldContainData)
            {
                return r.Parameters.Count > 0 && r.Parameters[0].Type == ParameterType.RequestBody && r.Parameters[0].DataFormat == DataFormat.Json;
            }
            else
            {
                return r.Parameters.Count == 0;
            }
        }

        [TestInitialize]
        public void Setup()
        {
            // Create a new api service
            apiService = new AuctionApiService(baseApiUrl);
        }

        [TestMethod]
        public void AddAuction_ExpectSuccess()
        {
            // Arrange
            string fullResource = $"{baseApiUrl}/auctions";
            string clientBase = apiService.client.BaseUrl.ToString();

            Auction auction = new Auction() { Title = "Dragon Plush Toy", Description = "Not a real dragon", User = "Bernice", CurrentBid = 19.99 };
            Mock<IRestClient> restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.Execute<Auction>(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource) && IsRequestBodyValid(r, true)), Method.POST))
                .Returns(new RestResponse<Auction>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = new Auction { Id = 9, CurrentBid = 19.99, Description = "Not a real dragon", Title = "Dragon Plush Toy", User = "Bernice" },
                    ResponseStatus = ResponseStatus.Completed
                });
            apiService.client = restClient.Object;

            // Act
            Auction newAuction = null;
            try
            {
                newAuction = apiService.AddAuction(auction);
            }
            catch (System.NullReferenceException)
            {
                Assert.Fail("Request not completed. Check your URL and ensure you're sending data in the request body.");
            }
            catch (System.NotImplementedException)
            {
                Assert.Fail("Method not yet implemented.");
            }

            // Assert
            restClient.Verify(x => x.Execute<Auction>(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource) && IsRequestBodyValid(r, true)), Method.POST), Times.Once(), "The API wasn't called or called too many times.");
            newAuction.Id.Should().Be(9);
            newAuction.Title.Should().Be(auction.Title);
            newAuction.Description.Should().Be(auction.Description);
            newAuction.User.Should().Be(auction.User);
            newAuction.CurrentBid.Should().Be(auction.CurrentBid);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void AddAuction_ExpectFailureResponse()
        {
            // Arrange
            string fullResource = $"{baseApiUrl}/auctions";
            string clientBase = apiService.client.BaseUrl.ToString();

            Auction auction = new Auction() { Title = "\\", Description = "Bad data" };
            Mock<IRestClient> restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.Execute<Auction>(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource) && IsRequestBodyValid(r, true)), Method.POST))
                .Returns(new RestResponse<Auction>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ResponseStatus = ResponseStatus.Completed
                });
            apiService.client = restClient.Object;

            // Act
            Auction newAuction = null;
            try
            {
                newAuction = apiService.AddAuction(auction);
            }
            catch (System.NullReferenceException)
            {
                Assert.Fail("Request not completed. Check your URL and ensure you're sending data in the request body.");
            }
            catch (System.NotImplementedException)
            {
                Assert.Fail("Method not yet implemented.");
            }

            // Assert
            // no assertions, because exception is expected - see [ExpectedException] attribute on this test method
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void AddAuction_ExpectNoResponse()
        {
            // Arrange
            string fullResource = $"{baseApiUrl}/auctions";
            string clientBase = apiService.client.BaseUrl.ToString();

            Auction auction = new Auction() { Title = "Dragon Plush Toy", Description = "Not a real dragon", User = "Bernice", CurrentBid = 19.99 };
            Mock<IRestClient> restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.Execute<Auction>(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource) && IsRequestBodyValid(r, true)), Method.POST))
                .Returns(new RestResponse<Auction>
                {
                    ResponseStatus = ResponseStatus.Error
                });
            apiService.client = restClient.Object;

            // Act
            Auction newAuction = null;
            try
            {
                newAuction = apiService.AddAuction(auction);
            }
            catch (System.NullReferenceException)
            {
                Assert.Fail("Request not completed. Check your URL and ensure you're sending data in the request body.");
            }
            catch (System.NotImplementedException)
            {
                Assert.Fail("Method not yet implemented.");
            }

            // Assert
            // no assertions, because exception is expected - see [ExpectedException] attribute on this test method
        }

        [TestMethod]
        public void UpdateAuction_ExpectSuccess()
        {
            // Arrange
            string fullResource = $"{baseApiUrl}/auctions/4";
            string clientBase = apiService.client.BaseUrl.ToString();

            Auction auction = new Auction() { Id = 4, Title = "Dragon Plush Toy", Description = "Not a real dragon", User = "Bernice", CurrentBid = 19.99 };
            Mock<IRestClient> restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.Execute<Auction>(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource) && IsRequestBodyValid(r, true)), Method.PUT))
                .Returns(new RestResponse<Auction>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = new Auction { Id = 4, CurrentBid = 19.99, Description = "I lied. It is a real dragon", Title = "Dragon Plush Toy", User = "Bernice" },
                    ResponseStatus = ResponseStatus.Completed
                });
            apiService.client = restClient.Object;

            // Act
            Auction updatedAuction = null;
            try
            {
                updatedAuction = apiService.UpdateAuction(auction);
            }
            catch (System.NullReferenceException)
            {
                Assert.Fail("Request not completed. Check your URL and ensure you're sending data in the request body.");
            }
            catch (System.NotImplementedException)
            {
                Assert.Fail("Method not yet implemented.");
            }

            // Assert
            restClient.Verify(x => x.Execute<Auction>(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource) && IsRequestBodyValid(r, true)), Method.PUT), Times.Once(), "The API wasn't called or called too many times.");
            updatedAuction.Description.Should().Equals("I lied. It is a real dragon");
            updatedAuction.CurrentBid.Should().Be(19.99);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void UpdateAuction_ExpectFailureResponse()
        {
            // Arrange
            string fullResource = $"{baseApiUrl}/auctions/4";
            string clientBase = apiService.client.BaseUrl.ToString();

            Auction auction = new Auction() { Id = 4, Title = "Dragon Plush Toy", Description = "Not a real dragon", User = "Bernice", CurrentBid = 19.99 };
            Mock<IRestClient> restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.Execute<Auction>(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource) && IsRequestBodyValid(r, true)), Method.PUT))
                .Returns(new RestResponse<Auction>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ResponseStatus = ResponseStatus.Completed
                });
            apiService.client = restClient.Object;

            // Act
            Auction updatedAuction = null;
            try
            {
                updatedAuction = apiService.UpdateAuction(auction);
            }
            catch (System.NullReferenceException)
            {
                Assert.Fail("Request not completed. Check your URL and ensure you're sending data in the request body.");
            }
            catch (System.NotImplementedException)
            {
                Assert.Fail("Method not yet implemented.");
            }

            // Assert
            // no assertions, because exception is expected - see [ExpectedException] attribute on this test method
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void UpdateAuction_ExpectNoResponse()
        {
            // Arrange
            string fullResource = $"{baseApiUrl}/auctions/4";
            string clientBase = apiService.client.BaseUrl.ToString();

            Auction auction = new Auction() { Id = 4, Title = "Dragon Plush Toy", Description = "Not a real dragon", User = "Bernice", CurrentBid = 19.99 };
            Mock<IRestClient> restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.Execute<Auction>(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource) && IsRequestBodyValid(r, true)), Method.PUT))
                .Returns(new RestResponse<Auction>
                {
                    ResponseStatus = ResponseStatus.Error
                });
            apiService.client = restClient.Object;

            // Act
            Auction updatedAuction = null;
            try
            {
                updatedAuction = apiService.UpdateAuction(auction);
            }
            catch (System.NullReferenceException)
            {
                Assert.Fail("Request not completed. Check your URL and ensure you're sending data in the request body.");
            }
            catch (System.NotImplementedException)
            {
                Assert.Fail("Method not yet implemented.");
            }

            // Assert
            // no assertions, because exception is expected - see [ExpectedException] attribute on this test method
        }

        [TestMethod]
        public void DeleteAuction_ExpectSuccess()
        {
            // Arrange
            string fullResource = $"{baseApiUrl}/auctions/1";
            string clientBase = apiService.client.BaseUrl.ToString();

            Mock<IRestClient> restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.Execute(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource) && IsRequestBodyValid(r, false)), Method.DELETE))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    ResponseStatus = ResponseStatus.Completed
                });
            apiService.client = restClient.Object;

            // Act
            bool deleteSuccess = false;
            try
            {
                deleteSuccess = apiService.DeleteAuction(1);
            }
            catch (System.NullReferenceException)
            {
                Assert.Fail("Request not completed. Check your URL and ensure you aren't sending a request body.");
            }
            catch (System.NotImplementedException)
            {
                Assert.Fail("Method not yet implemented.");
            }

            // Assert
            restClient.Verify(x => x.Execute(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource) && IsRequestBodyValid(r, false)), Method.DELETE), Times.Once(), "The API wasn't called or called too many times.");
            deleteSuccess.Should().BeTrue("expected delete to be successful.");
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void DeleteAuction_ExpectFailureResponse()
        {
            // Arrange
            string fullResource = $"{baseApiUrl}/auctions/99";
            string clientBase = apiService.client.BaseUrl.ToString();

            Mock<IRestClient> restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.Execute(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource) && IsRequestBodyValid(r, false)), Method.DELETE))
                .Returns(new RestResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ResponseStatus = ResponseStatus.Completed
                });
            apiService.client = restClient.Object;

            // Act
            bool deleteSuccess = false;
            try
            {
                deleteSuccess = apiService.DeleteAuction(99);
            }
            catch (System.NullReferenceException)
            {
                Assert.Fail("Request not completed. Check your URL and ensure you aren't sending a request body.");
            }
            catch (System.NotImplementedException)
            {
                Assert.Fail("Method not yet implemented.");
            }

            // Assert
            // no assertions, because exception is expected - see [ExpectedException] attribute on this test method
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void DeleteAuction_ExpectNoResponse()
        {
            // Arrange
            string fullResource = $"{baseApiUrl}/auctions/99";
            string clientBase = apiService.client.BaseUrl.ToString();

            Mock<IRestClient> restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.Execute(It.Is<IRestRequest>(r => CompareUrl(clientBase, r.Resource, fullResource) && IsRequestBodyValid(r, false)), Method.DELETE))
                .Returns(new RestResponse
                {
                    ResponseStatus = ResponseStatus.Error
                });
            apiService.client = restClient.Object;

            // Act
            bool deleteSuccess = false;
            try
            {
                deleteSuccess = apiService.DeleteAuction(99);
            }
            catch (System.NullReferenceException)
            {
                Assert.Fail("Request not completed. Check your URL and ensure you aren't sending a request body.");
            }
            catch (System.NotImplementedException)
            {
                Assert.Fail("Method not yet implemented.");
            }

            // Assert
            // no assertions, because exception is expected - see [ExpectedException] attribute on this test method
        }
    }
}
