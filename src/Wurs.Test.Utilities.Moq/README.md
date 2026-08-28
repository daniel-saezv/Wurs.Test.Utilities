# Wurs.Test.Utilities.Moq

Wurs.Test.Utilities.Moq is an extension library for Wurs.Test.Utilities that integrates with Moq to provide easy setup and management of HTTP message handlers in unit tests.

## Features

- **MockHttpMessageHandlerExtensions**: Extension methods for setting up HTTP message handlers using Moq.

## Installation

To install the library, add the following package reference to your project:


## Usage

### Setting Up HttpMessageHandlerContext with Moq
```
    private readonly YourSut _sut;
    private readonly Mock<HttpMessageHandler> _handlerMock;

    public YourSutTests()
    {
        _handlerMock = new Mock<HttpMessageHandler>();
        var clientFactoryMock = new Mock<IHttpClientFactory>();

        clientFactoryMock.Setup(cf => cf.CreateClient(It.IsAny<string>()))
            .Returns(new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("https://stub.url.com")
            });

        _sut = new YourSut(clientFactoryMock.Object);
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


## License

This project is licensed under the MIT License.



