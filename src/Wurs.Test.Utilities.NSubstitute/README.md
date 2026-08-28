# Wurs.Test.Utilities.NSubstitute

Wurs.Test.Utilities.NSubstitute is an extension library for Wurs.Test.Utilities that integrates with NSubstitute to provide easy setup and management of HTTP message handlers in unit tests.

## Features

- **HttpMessageHandlerExtensions**: Extension methods for setting up HTTP message handlers using NSubstitute.

## Installation

To install the library, add the following package reference to your project:


## Usage

### Setting Up HttpMessageHandlerContext with NSubstitute
```
    private readonly YourSut _sut;
    private readonly HttpMessageHandler _handler;

    public YourSutTests()
    {
        _handler = Substitute.For<HttpMessageHandler>();
        var clientFactoryMock = Substitute.For<IHttpClientFactory>();

        clientFactoryMock.CreateClient(Arg.Any<string>())
            .Returns(new HttpClient(_handler)
            {
                BaseAddress = new Uri("https://stub.url.com")
            });

        _sut = new YourSut(clientFactoryMock);
    }

    [TestMethod()]
    public async Task YourTest()
    {
        _handler.SetupContext()
            .AddResponse(new YourResponse());


        var result = await _sut.YourMethod();

        Assert.IsNotNull(result);
    }  
```



