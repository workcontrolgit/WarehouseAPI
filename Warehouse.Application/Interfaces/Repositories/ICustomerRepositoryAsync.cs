using Warehouse.Application.Features.Customers.Queries.GetCustomers;
using Warehouse.Application.Parameters;
using Warehouse.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Warehouse.Domain.Customers.Models;

namespace Warehouse.Application.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for Customer entity with asynchronous methods.
    /// </summary>
    /// <param name="companyName">Customer number to check for uniqueness.</param>
    /// <returns>
    /// Task indicating whether the customer name is unique.
    /// </returns>
    /// <param name="rowCount">Number of rows to seed.</param>
    /// <returns>
    /// Task indicating the completion of seeding.
    /// </returns>
    /// <param name="requestParameters">Parameters for the query.</param>
    /// <param name="data">Data to be returned.</param>
    /// <param name="recordsCount">Number of records.</param>
    /// <returns>
    /// Task containing the paged response.
    /// </returns>    
    public interface ICustomerRepositoryAsync : IGenericRepositoryAsync<Customer>
    {
        Task<bool> IsUniqueCustomerNumberAsync(string companyName);

        Task<bool> SeedDataAsync(int rowCount);

        Task<(IEnumerable<Entity> data, RecordsCount recordsCount)> GetPagedCustomerReponseAsync(GetCustomersQuery requestParameters);
    }
}