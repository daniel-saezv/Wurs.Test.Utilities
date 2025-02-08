# Wurs.Test.Utilities

Wurs.Test.Utilities is a utility library designed to facilitate testing in .NET projects. It provides helper classes and methods to simplify the creation and management of HTTP message handlers, serialization options, and response handling.

## Features

- **HttpMessageHandlerContext**: A context for managing HTTP message handlers and responses.
- **QueueResponseHelper**: A helper class for dequeuing HTTP responses.
- **Serialization Options**: Centralized configuration for JSON serialization options.

## Installation

To install the library, add the following package reference to your project:


## Usage

### Configuring Serialization Options

You can configure the default JSON serialization options for all tests by setting the `DefaultJsonSerializerOptions` property in a static constructor or a test fixture.
```
using System.Text.Json; using Wurs.Test.Utilities;
public class TestClass 
{ 
    static TestClass() 
    { 
        HttpMessageHandlerContext.DefaultJsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true };
    } 
}
```


### Using HttpMessageHandlerContext
Read specific readme file for your mocking library.


## License

This project is licensed under the MIT License.


