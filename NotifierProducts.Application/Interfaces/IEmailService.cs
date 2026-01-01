using NotifierProducts.Domain.Model;

namespace NotifierProducts.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(Product product);
    }
}
