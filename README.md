# ProductManagerAPI

How to run:
1)Make sure you use .net core 7.0
2)Install following NuGet packages:
  1. Microsoft.AspNetCore.OpenApi 7.0.13
  2. Microsoft.EntityFrameworkCore 7.0.14
  3. Microsoft.EntityFrameworkCore.SqlServer 7.0.14
  4. Microsoft.EntityFrameworkCore.Tools 7.0.14
  5. Microsoft.VisualStudio.Web.CodeGeneration.Design 7.0.10
3)Add connection string to appsettings.json
4)Remove Migrations if present
5)Package Manage Console: Add-Migration InitialCreate
                           Update-Database
6)Run project
Test data:
{
  "productName": "Viineripirukas",
  "productPrice": 2,
  "productVAT": 20,
  "productPriceWithVAT": 2.50,
  "productAdded": "2023-11-20T18:45:16.967Z",
  "productGroupID": 10,
  "stores": [
    {
      "storeID": 1,
      "quantity": 12
    },
{
      "storeID": 2,
      "quantity": 16
    }
  ]
}
Test data for VAT(will calculate the missing value):
{
  "productName": "Viineripirukas",
  "productPrice": 20,
  "productVAT": 20,
  "productPriceWithVAT": 0,
  "productAdded": "2023-11-20T18:45:16.967Z",
  "productGroupID": 10,
  "stores": [
    {
      "storeID": 1,
      "quantity": 12
    },
{
      "storeID": 2,
      "quantity": 16
    }
  ]
}

(Will return error when 2 or more Price values missing)

{
  "productName": "Viineripirukas",
  "productPrice": 2,
  "productVAT": 0,
  "productPriceWithVAT": 0,
  "productAdded": "2023-11-20T18:45:16.967Z",
  "productGroupID": 10,
  "stores": [
    {
      "storeID": 1,
      "quantity": 12
    },
{
      "storeID": 2,
      "quantity": 16
    }
  ]
}

(Will return error when product group missing)

  "productName": "Viineripirukas",
  "productPrice": 2,
  "productVAT": 20,
  "productPriceWithVAT": 2.40,
  "productAdded": "2023-11-20T18:45:16.967Z",
  "productGroupID": ,
  "stores": [
    {
      "storeID": 1,
      "quantity": 12
    },
{
      "storeID": 2,
      "quantity": 16
    }
  ]
}
