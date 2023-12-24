using Bogus;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Address = Warehouse.Domain.Entities.Address;

namespace Warehouse.Infrastructure.Shared.Services
{
    public class DatabaseSeeder
    {
        public IReadOnlyCollection<Product> Products { get; }
        public IReadOnlyCollection<ProductCategory> ProductCategories { get; }
        public IReadOnlyCollection<ProductProductCategory> ProductProductCategories { get; }

        public IReadOnlyCollection<Address> Addresses { get; }
        public IReadOnlyCollection<Customer> Customers { get; }

        public IReadOnlyCollection<Order> Orders { get; }

        public IReadOnlyCollection<OrderItem> OrderItems { get; }


        public DatabaseSeeder(int rowCount=100, int seedValue = 1969)
        {
            Products = GenerateProducts(rowCount, seedValue);
            ProductCategories = GenerateProductCategories(rowCount, seedValue);
            ProductProductCategories = GenerateProductProductCategories(rowCount, seedValue, Products, ProductCategories);

            Addresses = GenerateAddresses(rowCount, seedValue);
            Customers = GenerateCustomers(rowCount, seedValue, Addresses);

            Orders = GenerateOrders(rowCount, seedValue, Customers);
            OrderItems = GenerateOrderItems(rowCount, seedValue, Orders, Products);


        }

        private static IReadOnlyCollection<Order> GenerateOrders(int rowCount, int seedValue, IEnumerable<Customer> customers)
        {
            var faker = new Faker<Order>()
                .UseSeed(seedValue)
                .RuleFor(r => r.Id, f => Guid.NewGuid())
                .RuleFor(r => r.OrderDate, f => f.Date.Recent())
                .RuleFor(r => r.Terms, f => f.PickRandom<Terms>())
                .RuleFor(r => r.CustomerId, f => f.PickRandom(customers).Id)
                .RuleFor(r => r.Created, f => f.Date.Recent())
                .RuleFor(r => r.CreatedBy, f => f.Internet.UserName())
                ;

            return faker.Generate(rowCount);
        }

        private static IReadOnlyCollection<OrderItem> GenerateOrderItems(int rowCount, int seedValue, IEnumerable<Order> orders, IEnumerable<Product> products)
        {
            var faker = new Faker<OrderItem>()
                .UseSeed(seedValue) // Use any number
                .RuleFor(r => r.Id, f => Guid.NewGuid())
                .RuleFor(r => r.OrderId, f => f.PickRandom(orders).Id)
                .RuleFor(r => r.ProductId, f => f.PickRandom(products).Id)
                .RuleFor(r => r.UnitPrice, f => f.Commerce.Price(1).First())
                .RuleFor(r => r.Quantity, f => f.Random.Number(1, 100))
                .RuleFor(r => r.Discount, f => f.Random.Number(1, 100))
                .RuleFor(r => r.Created, f => f.Date.Recent())
                .RuleFor(r => r.CreatedBy, f => f.Internet.UserName())
                ;

            return faker.Generate(rowCount);

        }


        private static IReadOnlyCollection<Address> GenerateAddresses(int rowCount, int seedValue)
        {
            var faker = new Faker<Address>()
                  .UseSeed(seedValue) // Use any number
                  .RuleFor(r => r.Id, f => Guid.NewGuid())
                  .RuleFor(r => r.Address1, f => f.Address.StreetAddress())
                  .RuleFor(r => r.Address2, f => f.Address.SecondaryAddress().OrNull(f, .5f))
                  .RuleFor(r => r.City, f => f.Address.City())
                  .RuleFor(r => r.StateProvince, f => f.Address.State())
                  .RuleFor(r => r.PostalCode, f => f.Address.ZipCode())
                  .RuleFor(r => r.Country, f => f.Address.Country())
                  .RuleFor(r => r.Created, f => f.Date.Recent())
                  .RuleFor(r => r.CreatedBy, f => f.Internet.UserName())
                  ;

            return faker.Generate(rowCount);

        }


        private static IReadOnlyCollection<Customer> GenerateCustomers(int rowCount, int seedValue, IEnumerable<Address> addresses)
        {
            var faker = new Faker<Customer>()
                  .UseSeed(seedValue) // Use any number
                  .RuleFor(r => r.Id, f => Guid.NewGuid())
                  .RuleFor(r => r.CompanyName, f => f.Company.CompanyName())
                  .RuleFor(r => r.ContactName, f => f.Name.FullName())
                  .RuleFor(r => r.AddressId, f => f.PickRandom(addresses).Id)
                  .RuleFor(r => r.Phone, f => f.Phone.PhoneNumberFormat().OrNull(f, .15f))
                  .RuleFor(r => r.Created, f => f.Date.Recent())
                  .RuleFor(r => r.CreatedBy, f => f.Internet.UserName())
                  ;

            return faker.Generate(rowCount);

        }


        private static IReadOnlyCollection<Product> GenerateProducts(int rowCount, int seedValue)
        {
            var faker = new Faker<Product>()
                .UseSeed(seedValue) // Use any number
                .RuleFor(r => r.Id, f => Guid.NewGuid()) // Each product will have a guid.
                .RuleFor(r => r.Name, f => f.Commerce.ProductName())
                .RuleFor(r => r.CreationDate, f => f.Date.RecentOffset())
                .RuleFor(r => r.Description, f => f.Commerce.ProductDescription())
                  .RuleFor(r => r.Created, f => f.Date.Recent())
                  .RuleFor(r => r.CreatedBy, f => f.Internet.UserName())
                ;

            return faker.Generate(rowCount);
        }

        private static IReadOnlyCollection<ProductCategory> GenerateProductCategories(int rowCount, int seedValue)
        {
            var faker = new Faker<ProductCategory>()
                .UseSeed(seedValue) // Use any number
                .RuleFor(r => r.Id, f => Guid.NewGuid()) // Each category will have a guid.
                .RuleFor(r => r.Name, f => f.Commerce.Categories(1).FirstOrDefault())
                  .RuleFor(r => r.Created, f => f.Date.Recent())
                  .RuleFor(r => r.CreatedBy, f => f.Internet.UserName())
                ;

            return faker.Generate(rowCount);
        }

        private static IReadOnlyCollection<ProductProductCategory> GenerateProductProductCategories(
            int rowCount, int seedValue,
            IEnumerable<Product> products,
            IEnumerable<ProductCategory> productCategories)
        {
            // Now we set up the faker for our join table.
            // We do this by grabbing a random product and category that were generated.
            var faker = new Faker<ProductProductCategory>()
                .UseSeed(seedValue) // Use any number
                .RuleFor(r => r.ProductId, f => f.PickRandom(products).Id)
                .RuleFor(r => r.CategoryId, f => f.PickRandom(productCategories).Id);

            return faker.Generate(rowCount)
                .GroupBy(r => new { r.ProductId, r.CategoryId })
                .Select(r => r.First())
                .ToList();


        }
    }
}
