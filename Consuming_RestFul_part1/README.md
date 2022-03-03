# Web Services GET exercise (C#)

In this exercise, you'll work on a command-line application that displays online auction info. The command-line application provides the user interface, processes console input, and calls the methods that you'll write code for.

You'll add web API calls using RestSharp to retrieve a list of auctions, details for a single auction, and filter the list of auctions by title and current bid.

These are the endpoints you'll work on for this exercise:

- GET: `http://localhost:3000/auctions`
- GET: `http://localhost:3000/auctions/{id}`
- GET: `http://localhost:3000/auctions?title_like=<*value*>`
- GET: `http://localhost:3000/auctions?currentBid_lte=<*value*>`

## Step One: Start the server

Before starting, make sure the web API is up and running. Open the command line and navigate to the `./server/` folder in this exercise.

First, run the command `npm install` to install any dependencies. You won't need to do this on any subsequent run.

To start the server, run the command `npm start`. If there aren't any errors, you'll see the following, which means that you've successfully set up your web API:

```
  \{^_^}/ hi!

  Loading data-generation.js
  Done

  Resources
  http://localhost:3000/auctions

  Home
  http://localhost:3000

  Type s + enter at any time to create a snapshot of the database
  Watching...
```

> Note: The server needs to be running for `AuctionApp` to run successfully, but you don't need to start the server just to run the tests in the `AuctionApp.Tests` project.

## Step Two: Explore the API

Before moving on to the next step, explore the web API using Postman. You can access the following endpoints:

- GET: `http://localhost:3000/auctions`
- GET: `http://localhost:3000/auctions/{id}` (use a number between 1 and 7 in place of `{id}`)

## Step Three: Review the starting code

### Data model

There's a class provided in `Models/Auction.cs` that represents the data model for an auction object. If you've looked at the JSON results from the API, the properties for the class might look familiar to you.

### Provided code

`AuctionApp.cs` has code that runs a menu loop which displays user options. It calls code in `Services/AuctionApiService.cs` to retrieve data from an API, and it calls code in `Services/AuctionConsoleService.cs` to get user input and display output to the user.

Take a moment to review `AuctionConsoleService.PrintAuctions()` and `AuctionConsoleService.PrintAuction()`. Note how the methods access and display the properties of an `Auction` object.

Also, take a look at `Program.cs` and notice the constant `apiUrl` declared and that's it's passed to the `AuctionApp` constructor. Take a look at `AuctionApp.cs` and you'll see the same value passed to the `AuctionApiService` constructor. The `AuctionApiService` constructor uses that value when instantiating a new `RestClient`, this sets the "base URL" for the `RestClient` you'll use to complete this exercise:

```csharp
// Program.cs
private const string apiUrl = "http://localhost:3000/";
static void Main()
{
    AuctionApp app = new AuctionApp(apiUrl);
    app.Run();
}

// AuctionApp.cs
public AuctionApp(string apiUrl)
{
    this.auctionApiService = new AuctionApiService(apiUrl);
}

// AuctionApiService.cs
public IRestClient client;
public AuctionApiService(string apiUrl)
{
    client = new RestClient(apiUrl);
}
```

### Your code

In `Services/AuctionApiService.cs`, you'll find four methods where you'll add code to call the API methods:

- `GetAllAuctions()`
- `GetDetailsForAuction()`
- `GetAuctionsSearchTitle()`
- `GetAuctionsSearchPrice()`

In `AuctionApp.Run()`, you'll see how each menu option calls these methods and passes their return values to one of the `Print` methods described in the previous section.

### Unit tests

In `AuctionApp.Tests`, you'll find the unit tests for the methods you'll write today. After you complete each step, more tests pass.

> Note: The unit tests use two third-party libraries called FluentAssertions and Moq. You can install them through NuGet, like RestSharp. The FluentAssertions library allows you to test object comparison, a task that can be difficult even for experienced programmers. Moq is a "mocking" library which allows you to run tests even if the server isn't running.

## Step Four: Write the console application

Use the `IRestClient client` declared in `AuctionApiService()` to complete these methods.

### List all auctions

In the `GetAllAuctions()` method, remove `throw new System.NotImplementedException();` and add code here to:

- Create a new `RestRequest` for the appropriate endpoint.
- Make a `GET` request and save the response in an `IRestResponse` variable using the type parameter so RestSharp can automatically deserialize it. Hint: it'll be a collection of `Auction` items.
- `return` the deserialized object.

Once you've done this, run the unit tests. After `GetAllAuctions_ExpectList` passes, you can run the application. If you select option 1 on the menu, you'll see the ID, title, and current bid for each auction.

### List details for specific auction

In the `GetDetailsForAuction()` method, remove `throw new System.NotImplementedException();` and add code here to:

- Create a new `RestRequest` for the appropriate endpoint which includes the `auctionId` variable appended to it. Hint: look at the second URL in Step Two.
- Make a `GET` request and save the response in an `IRestResponse` variable using the type parameter so RestSharp can automatically deserialize it. This method only retrieves one `Auction` item.
- `return` the deserialized object.

Once you've done this, run the unit tests. After `GetDetailsForAuction_ExpectSpecificItems` and `GetDetailsForAuction_IdNotFound()` pass, run the application. If you select option 2 on the menu, and enter an ID of one of the auctions, you'll see the full details for that auction.

### Find auctions with a specified term in the title

This endpoint uses a query string. If you don't remember what a query string is, refer back to your reading material.

Instead of adding a slash `/`, you'll use a question mark `?` and `title_like=` before appending the `searchTitle` variable to the URL. The `title_like` parameter allows you to search for auctions that have a title containing the string you pass to it.

In the `GetAuctionsSearchTitle()` method, remove `throw new System.NotImplementedException();` and add code here to:

- Create a new `RestRequest` for the appropriate endpoint with the question mark `?` and query string to appended to it.
- Make a `GET` request and save the response in an `IRestResponse` variable using the type parameter so RestSharp can automatically deserialize it. This one is a collection of `Auction` items.
- `return` the deserialized object.

Once you've done this, run the unit tests. After `GetAuctionsSearchTitle_ExpectList` and `GetAuctionsSearchTitle_ExpectNone` pass, you can run the application. If you select option 3 on the menu, and enter a string, like `watch`, you'll see the ID, title, and current bid for each auction that matches.

### Find auctions less than or equal to a specified price

This endpoint also uses a query string, but the parameter key is `currentBid_lte`. This parameter looks at the `currentBid` field and returns auctions that are **L**ess **T**han or **E**qual to the value you supply.

In the `GetAuctionsSearchPrice()` method, remove `throw new System.NotImplementedException();` and add code here to:

- Create a new `RestRequest` for the appropriate endpoint with the question mark `?` and query string to appended to it.
- Make a `GET` request and save the response in an `IRestResponse` variable using the type parameter so RestSharp can automatically deserialize it. This one is a collection of `Auction` items.
- `return` the deserialized object.

Once you've done this, run the unit tests. After `GetAuctionsSearchPrice_ExpectList` and `GetAuctionsSearchPrice_ExpectNone` pass, you can run the application. If you select option 4 on the menu, and enter a number, like `150`, you'll see the ID, title, and current bid for each auction that matches.

Since the value is a `double`, you can enter a decimal value, too. Try entering `125.25`, and then `125.20`, and observe the differences between the two result sets. The "Mad-dog Sneakers" don't appear in the second list because the current bid for them is `125.23`.
