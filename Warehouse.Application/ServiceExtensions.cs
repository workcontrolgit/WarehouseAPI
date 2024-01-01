using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Warehouse.Application.Behaviours;
using Warehouse.Application.Helpers;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Customers.Models;
using Warehouse.Domain.Employees.Models;
using Warehouse.Domain.Positions.Models;

namespace Warehouse.Application
{
    public static class ServiceExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            // Register FluentValidation validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            // Register MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            // Register MediatR pipeline behavior
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped<IDataShapeHelper<Position>, DataShapeHelper<Position>>();
            services.AddScoped<IDataShapeHelper<Employee>, DataShapeHelper<Employee>>();
            services.AddScoped<IDataShapeHelper<Customer>, DataShapeHelper<Customer>>();
            services.AddScoped<IModelHelper, ModelHelper>();
        }
    }
}