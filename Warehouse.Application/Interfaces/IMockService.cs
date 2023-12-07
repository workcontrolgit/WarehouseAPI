using System.Collections.Generic;
using Warehouse.Domain.Entities;

namespace Warehouse.Application.Interfaces
{
    public interface IMockService
    {
        List<Position> GetPositions(int rowCount);

        List<Customer> GetCustomers(int rowCount);


        List<Employee> GetEmployees(int rowCount);

        List<Position> SeedPositions(int rowCount);
    }
}