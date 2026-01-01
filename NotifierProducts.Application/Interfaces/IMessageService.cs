using NotifierProducts.Application.Models;

namespace NotifierProducts.Application.Interfaces
{
    public interface IMessageService
    {
        Task<List<ProductMessage>> ReceiveMessagesAsync();
        Task DeleteMessageAsync(string receiptHandle);
    }
}
