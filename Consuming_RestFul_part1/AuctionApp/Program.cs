namespace AuctionApp
{
    public class Program
    {
        private const string apiUrl = "http://localhost:3000/";
        static void Main()
        {
            AuctionApp app = new AuctionApp(apiUrl);
            app.Run();
        }
    }
}
