# SimpleBankingApi

### Description

A simple web api working on ASP.NET Core 2.2.  
The Visual Studio solution is divided into three main parts :

*  **Banking.Api**: the presentation layer
*  **Banking.Service**: the business layer
*  **Banking.Dao**: the data access layer

In addition, a few additional VS projects exist to separate interfaces and tests from the implementations mentioned above.

### Technologies

The Visual Studio solution is built with Visual Studio 2017, .NET Core 2.2, NUnit 3.11.  
In addition, a swagger is available (Swashbuckle 5.0.0 beta)

### WebApi

##### AdminController

| Method | Function name | Description                    |
| :------:  | ------------- | ------------------------------ |
| GET | `existingAccounts`      | Display all existing accounts       |

##### BankController

| Method | Function name | Description                    |
| :------:  | ------------- | ------------------------------ |
| GET |`getBalance/{accountNumber}`      | Returns the current balance of the given account       |
| GET |`getHistory/{accountNumber}`       | Returns all transactions and the related balance for the given account   |
| POST |`deposit`        | Make a transaction to deposit funds on the given account (Parameters: accountNumber, amount, comment)   |
| POST |`withdraw`       | Make a transaction to withdraw funds from the given account (Parameters: accountNumber, amount, comment)  |

Use the following link to test the webapi: [Swagger](https://simplebankingapi.azurewebsites.net)