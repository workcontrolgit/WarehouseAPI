using Warehouse.Application.Features.Customers.Queries.GetCustomers;
using Warehouse.Application.Parameters;
using Warehouse.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Warehouse.Application.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for Customer entity with asynchronous methods.
    /// </summary>
    /// <param name="positionNumber">Customer number to check for uniqueness.</param>
    /// <returns>
    /// Task indicating whether the position number is unique.
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
        Task<bool> IsUniqueCustomerNumberAsync(string positionNumber);

        Task<bool> SeedDataAsync(int rowCount);

        Task<(IEnumerable<Entity> data, RecordsCount recordsCount)> GetPagedCustomerReponseAsync(GetCustomersQuery requestParameters);
    }
}