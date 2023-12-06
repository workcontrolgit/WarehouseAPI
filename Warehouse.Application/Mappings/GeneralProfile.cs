using AutoMapper;
using Warehouse.Application.Features.Employees.Queries.GetEmployees;
using Warehouse.Application.Features.Positions.Commands.CreatePosition;
using Warehouse.Application.Features.Positions.Queries.GetPositions;
using Warehouse.Domain.Entities;

namespace Warehouse.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Position, GetPositionsViewModel>().ReverseMap();
            CreateMap<Employee, GetEmployeesViewModel>().ReverseMap();
            CreateMap<CreatePositionCommand, Position>();
        }
    }
}