using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using AuctionApp.Models;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace AuctionApp.Tests
{
    [TestClass]
    public class AuctionsControllerTests
    {
        protected HttpClient client;

        [TestInitialize]
        public void Setup()
        {
            var builder = new WebHostBuilder().UseStartup<AuctionApp.Startup>();
            var server = new TestServer(builder);
            client = server.CreateClient();
        }


        [TestMethod]
        public async Task GetAuctions_ExpectList()
        {
            var response = await client.GetAsync("auctions");

            string responseContent = await response.Content.ReadAsStringAsync();
            List<Auction> content = JsonConvert.DeserializeObject<List<Auction>>(responseContent);

            Assert.IsTrue(response.IsSuccessStatusCode, "Response should be successful.");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(responseContent), "Response content is empty but shouldn't be.");
            Assert.IsNotNull(content, "Response content can't be deserialized into a list of Auctions.");
            Assert.IsTrue(content.Count > 0, "Response content should be a list with more than zero items.");
        }

        [TestMethod]
        public async Task GetAuction_SpecificAuction_ExpectAuction()
        {
            var response = await client.GetAsync("auctions/1");

            string responseContent = await response.Content.ReadAsStringAsync();
            Auction content = JsonConvert.DeserializeObject<Auction>(responseContent);

            Assert.IsTrue(response.IsSuccessStatusCode, "Response should be successful.");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(responseContent), "Response content is empty but shouldn't be.");
            Assert.IsNotNull(content, "Response content can't be deserialized into an Auction.");
        }

        [TestMethod]
        public async Task GetAuction_NonExistentAuction_ExpectEmpty()
        {
            var response = await client.GetAsync("auctions/23");

            string responseContent = await response.Content.ReadAsStringAsync();
            Auction content = JsonConvert.DeserializeObject<Auction>(responseContent);

            Assert.IsTrue(response.IsSuccessStatusCode, "Response should be successful."); //comes back as 204 in this implementation
            Assert.IsTrue(string.IsNullOrWhiteSpace(responseContent), "Response content should be empty but isn't.");
            Assert.IsNull(content, "Response content should be null.");
        }

        [TestMethod]
        public async Task CreateAuction_ExpectAuction()
        {
            Auction input = new Auction() { Title = "Dragon Plush", Description = "Not a real dragon", User = "Bernice", CurrentBid = 219.50 };

            var response = await client.PostAsJsonAsync("auctions", input);

            string responseContent = await response.Content.ReadAsStringAsync();
            Auction content = JsonConvert.DeserializeObject<Auction>(responseContent);

            Assert.IsTrue(response.IsSuccessStatusCode, "Response should be successful.");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(responseContent), "Response content is empty but shouldn't be.");
            Assert.IsNotNull(content, "Response content can't be deserialized into an Auction.");
            Assert.IsTrue(content.Id > 0, "Expected result to have an ID greater than 0.");
        }

        [TestMethod]
        public async Task SearchByTitle_ExpectList()
        {
            var response = await client.GetAsync("auctions?title_like=watch");

            string responseContent = await response.Content.ReadAsStringAsync();
            List<Auction> content = JsonConvert.DeserializeObject<List<Auction>>(responseContent);

            Assert.IsTrue(response.IsSuccessStatusCode, "Response should be successful.");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(responseContent), "Response content is empty but shouldn't be.");
            Assert.IsNotNull(content, "Response content can't be deserialized into a list of Auctions.");
            Assert.IsTrue(content.Count == 2, "Expected response should have two auctions.");
        }

        [TestMethod]
        public async Task SearchByTitle_ExpectNone()
        {
            string gibberish = "aergergvdasc";

            var response = await client.GetAsync($"auctions?title_like={gibberish}");

            string responseContent = await response.Content.ReadAsStringAsync();
            List<Auction> content = JsonConvert.DeserializeObject<List<Auction>>(responseContent);

            Assert.IsTrue(response.IsSuccessStatusCode, "Response should be successful.");
            Assert.IsNotNull(content, "Response content can't be deserialized into a list of Auctions.");
            Assert.IsTrue(content.Count == 0, "Expected response should have zero auctions.");
        }

        [TestMethod]
        public async Task SearchByPrice_ExpectList()
        {
            var response = await client.GetAsync("auctions?currentBid_lte=200");

            string responseContent = await response.Content.ReadAsStringAsync();
            List<Auction> content = JsonConvert.DeserializeObject<List<Auction>>(responseContent);

            Assert.IsTrue(response.IsSuccessStatusCode, "Response should be successful.");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(responseContent), "Response content is empty but shouldn't be.");
            Assert.IsNotNull(content, "Response content can't be deserialized into a list of Auctions.");
            Assert.IsTrue(content.Count == 4, "Expected response should have four auctions.");
        }

        [TestMethod]
        public async Task SearchByPrice_ExpectNone()
        {
            var response = await client.GetAsync("auctions?currentBid_lte=0.01");

            string responseContent = await response.Content.ReadAsStringAsync();
            List<Auction> content = JsonConvert.DeserializeObject<List<Auction>>(responseContent);

            Assert.IsTrue(response.IsSuccessStatusCode, "Response should be successful.");
            Assert.IsNotNull(content, "Response content can't be deserialized into a list of Auctions.");
            Assert.IsTrue(content.Count == 0, "Expected response should have zero auctions.");
        }

        [TestMethod]
        public async Task SearchByTitleAndPrice_ExpectList()
        {
            var response = await client.GetAsync("auctions?title_like=watch&currentBid_lte=200");

            string responseContent = await response.Content.ReadAsStringAsync();
            List<Auction> content = JsonConvert.DeserializeObject<List<Auction>>(responseContent);

            Assert.IsTrue(response.IsSuccessStatusCode, "Response should be successful.");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(responseContent), "Response content is empty but shouldn't be.");
            Assert.IsNotNull(content, "Response content can't be deserialized into a list of Auctions.");
            Assert.IsTrue(content.Count == 1, "Expected response should have one auction.");
        }

        [TestMethod]
        public async Task SearchByTitleAndPrice_ExpectNone()
        {
            var response = await client.GetAsync($"auctions?title_like=watch&currentBid_lte=0.01");

            string responseContent = await response.Content.ReadAsStringAsync();
            List<Auction> content = JsonConvert.DeserializeObject<List<Auction>>(responseContent);

            Assert.IsTrue(response.IsSuccessStatusCode, "Response should be successful.");
            Assert.IsNotNull(content, "Response content can't be deserialized into a list of Auctions.");
            Assert.IsTrue(content.Count == 0, "Expected response should have zero auctions.");
        }
    }
}
