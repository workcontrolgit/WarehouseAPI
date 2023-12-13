using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Warehouse.Application.Features.Customers.Queries.GetCustomers;
using Warehouse.Application.Interfaces;
using Warehouse.Application.Interfaces.Repositories;
using Warehouse.Application.Parameters;
using Warehouse.Domain.Entities;
using Warehouse.Infrastructure.Persistence.Contexts;
using Warehouse.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Warehouse.Infrastructure.Persistence.Repositories
{
    public class CustomerRepositoryAsync : GenericRepositoryAsync<Customer>, ICustomerRepositoryAsync
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Customer> _repository;
        private IDataShapeHelper<Customer> _dataShaper;
        private readonly IMockService _mockData;
        private readonly ILogger<CustomerRepositoryAsync> _logger;


        public CustomerRepositoryAsync(ApplicationDbContext dbContext,
            IDataShapeHelper<Customer> dataShaper, 
            IMockService mockData, 
            ILogger<CustomerRepositoryAsync> logger) : base(dbContext)
        {
            _dbContext = dbContext;
            _repository = dbContext.Set<Customer>();
            _dataShaper = dataShaper;
            _mockData = mockData;
        }

        public async Task<bool> IsUniqueCustomerNumberAsync(string companyName)
        {
            return await _repository
                .AllAsync(p => p.CompanyName != companyName);
        }

        public async Task<bool> SeedDataAsync(int rowCount)
        {
            await this.BulkInsertAsync(_mockData.GetCustomers(rowCount));
            return true;
        }

        public async Task<(IEnumerable<Entity> data, RecordsCount recordsCount)> GetPagedCustomerReponseAsync(GetCustomersQuery requestParameter)
        {
            var companyName = requestParameter.CompanyName;
            var contactName = requestParameter.ContactName;

            var pageNumber = requestParameter.PageNumber;
            var pageSize = requestParameter.PageSize;
            var orderBy = requestParameter.OrderBy;
            var fields = requestParameter.Fields;

            int recordsTotal, recordsFiltered;

            // Setup IQueryable
            var result = _repository
                .AsNoTracking()
                .AsExpandable();

            // Count records total
            recordsTotal = await result.CountAsync();

            // filter data
            FilterByColumn(ref result, companyName, contactName);

            // Count records after filter
            recordsFiltered = await result.CountAsync();

            //set Record counts
            var recordsCount = new RecordsCount
            {
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            // set order by
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                result = result.OrderBy(orderBy);
            }
            //Including multiple levels
            result = result.Include(customer => customer.Orders).ThenInclude(order => order.OrderItems).ThenInclude(product => product.Product);


            // select columns
            if (!string.IsNullOrWhiteSpace(fields))
            {
                result = result.Select<Customer>("new(" + fields + ")");
            }


            // paging
            result = result
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            // retrieve data to list
            var resultData = await result.ToListAsync();


            // shape data
            var shapeData = _dataShaper.ShapeData(resultData, fields);

            return (shapeData, recordsCount);

        }

        private void FilterByColumn(ref IQueryable<Customer> query, string companyName, string contactName)
        {
            if (!query.Any())
                return;

            if (string.IsNullOrEmpty(contactName) && string.IsNullOrEmpty(companyName))
                return;

            var predicate = PredicateBuilder.New<Customer>();

            if (!string.IsNullOrEmpty(companyName))
                predicate = predicate.Or(p => p.CompanyName.Contains(companyName.Trim()));

            if (!string.IsNullOrEmpty(contactName))
                predicate = predicate.Or(p => p.ContactName.Contains(contactName.Trim()));

            query = query.Where(predicate);
        }
    }
}