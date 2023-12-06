using System.Threading.Tasks;
using Warehouse.Application.DTOs.Email;

namespace Warehouse.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}