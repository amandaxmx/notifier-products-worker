using NotifierProducts.Application.Interfaces;
using System.Threading.Tasks;

namespace NotifierProducts.Application.UseCases
{
    public class ProcessProductUseCase
    {
        private readonly IMessageService _messageService;
        private readonly IEmailService _emailService;

        public ProcessProductUseCase(
            IMessageService messageService,
            IEmailService emailService)
        {
            _messageService = messageService;
            _emailService = emailService;
        }

        public async Task ExecuteAsync()
        {
            var productMessages = await _messageService.ReceiveMessagesAsync();

            foreach (var message in productMessages)
            {
                try
                {
                    await _emailService.SendEmailAsync(message.Product);
                    await _messageService.DeleteMessageAsync(message.ReceiptHandle);
                }
                catch (System.Exception ex)
                {

                }
            }
        }
    }
}
