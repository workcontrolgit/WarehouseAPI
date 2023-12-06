using System;
using Warehouse.Application.Interfaces;

namespace Warehouse.Infrastructure.Shared.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}