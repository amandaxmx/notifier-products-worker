using NotifierProducts.Domain.Model;

namespace NotifierProducts.Application.Models
{
    public class ProductMessage
    {
        public Product Product { get; set; }
        public string ReceiptHandle { get; set; }
    }
}
