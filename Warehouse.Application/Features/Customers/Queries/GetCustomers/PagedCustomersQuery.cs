using AutoMapper;
using MediatR;
using Warehouse.Application.Interfaces;
using Warehouse.Application.Interfaces.Repositories;
using Warehouse.Application.Parameters;
using Warehouse.Application.Wrappers;
using Warehouse.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Warehouse.Application.Features.Customers.Queries.GetCustomers
{
    public partial class PagedCustomersQuery : IRequest<PagedDataTableResponse<IEnumerable<Entity>>>
    {
        //strong type input parameters
        public int Draw { get; set; } //page number
        public int Start { get; set; } //Paging first record indicator. This is the start point in the current data set (0 index based - i.e. 0 is the first record).
        public int Length { get; set; } //page size
        public IList<SortOrder> Order { get; set; } //Order by
        public Search Search { get; set; } //search criteria
        public IList<Column> Columns { get; set; } //select fields
    }

    public class PageCustomerQueryHandler : IRequestHandler<PagedCustomersQuery, PagedDataTableResponse<IEnumerable<Entity>>>
    {
        private readonly ICustomerRepositoryAsync _positionRepository;
        private readonly IMapper _mapper;
        private readonly IModelHelper _modelHelper;

        public PageCustomerQueryHandler(ICustomerRepositoryAsync positionRepository, IMapper mapper, IModelHelper modelHelper)
        {
            _positionRepository = positionRepository;
            _mapper = mapper;
            _modelHelper = modelHelper;
        }

        public async Task<PagedDataTableResponse<IEnumerable<Entity>>> Handle(PagedCustomersQuery request, CancellationToken cancellationToken)
        {
            var validFilter = new GetCustomersQuery();

            // Draw map to PageNumber
            validFilter.PageNumber = (request.Start / request.Length) + 1;
            // Length map to PageSize
            validFilter.PageSize = request.Length;

            // Map order > OrderBy
            var colOrder = request.Order[0];
            switch (colOrder.Column)
            {
                case 0:
                    validFilter.OrderBy = colOrder.Dir == "asc" ? "CompanyName" : "CompanyName DESC";
                    break;

                case 1:
                    validFilter.OrderBy = colOrder.Dir == "asc" ? "ContactName" : "ContactName DESC";
                    break;

                case 2:
                    validFilter.OrderBy = colOrder.Dir == "asc" ? "Phone" : "Phone DESC";
                    break;
            }

            // Map Search > searchable columns
            if (!string.IsNullOrEmpty(request.Search.Value))
            {
                //limit to fields in view model
                validFilter.CompanyName = request.Search.Value;
                validFilter.ContactName = request.Search.Value;
            }
            if (string.IsNullOrEmpty(validFilter.Fields))
            {
                //default fields from view model
                validFilter.Fields = _modelHelper.GetModelFields<GetCustomersViewModel>();
            }
            // query based on filter
            var entityCustomers = await _positionRepository.GetPagedCustomerReponseAsync(validFilter);
            var data = entityCustomers.data;
            RecordsCount recordCount = entityCustomers.recordsCount;

            // response wrapper
            return new PagedDataTableResponse<IEnumerable<Entity>>(data, request.Draw, recordCount);
        }
    }
}