﻿using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.Application.Interfaces;
using Warehouse.Application.Interfaces.Repositories;
using Warehouse.Application.Parameters;
using Warehouse.Application.Wrappers;
using Warehouse.Domain.Employees.Models;
using Warehouse.Domain.Entities;

namespace Warehouse.Application.Features.Employees.Queries.GetEmployees
{
    /// <summary>
    /// GetAllEmployeesQuery - handles media IRequest
    /// BaseRequestParameter - contains paging parameters
    /// To add filter/search parameters, add search properties to the body of this class
    /// </summary>
    public class GetEmployeesQuery : QueryParameter, IRequest<PagedResponse<IEnumerable<Entity>>>
    {
        //examples:
        public string EmployeeNumber { get; set; }
        public string EmployeeTitle { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }

    }

    public class GetAllEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, PagedResponse<IEnumerable<Entity>>>
    {
        private readonly IEmployeeRepositoryAsync _employeeRepository;
        private readonly IModelHelper _modelHelper;



        /// <summary>
        /// Constructor for GetAllEmployeesQueryHandler class.
        /// </summary>
        /// <param name="employeeRepository">IEmployeeRepositoryAsync object.</param>
        /// <param name="modelHelper">IModelHelper object.</param>
        /// <returns>
        /// GetAllEmployeesQueryHandler object.
        /// </returns>
        public GetAllEmployeesQueryHandler(IEmployeeRepositoryAsync employeeRepository, IModelHelper modelHelper)
        {
            _employeeRepository = employeeRepository;
            _modelHelper = modelHelper;
        }



        /// <summary>
        /// Handles the GetEmployeesQuery request and returns a PagedResponse containing the requested data.
        /// </summary>
        /// <param name="request">The GetEmployeesQuery request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A PagedResponse containing the requested data.</returns>
        public async Task<PagedResponse<IEnumerable<Entity>>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            var validFilter = request;
            //filtered fields security
            if (!string.IsNullOrEmpty(validFilter.Fields))
            {
                //limit to fields in view model
                validFilter.Fields = _modelHelper.ValidateModelFields<GetEmployeesViewModel>(validFilter.Fields);
            }
            if (string.IsNullOrEmpty(validFilter.Fields))
            {
                //default fields from view model
                validFilter.Fields = _modelHelper.GetModelFields<GetEmployeesViewModel>();
            }
            // query based on filter
            var qryResult = await _employeeRepository.GetPagedEmployeeResponseAsync(validFilter);
            var data = qryResult.data;
            RecordsCount recordCount = qryResult.recordsCount;

            // response wrapper
            return new PagedResponse<IEnumerable<Entity>>(data, validFilter.PageNumber, validFilter.PageSize, recordCount);
        }
    }
}