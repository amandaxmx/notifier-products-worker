using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotifierProducts.Domain.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Url { get; set; }
        public decimal Price { get; set; }
        public string Active { get; set; }

        public Product(string name, string? url, decimal price, string active)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("O nome do produto não pode ser nulo ou vazio.", nameof(name));

            if (price < 0)
                throw new ArgumentException("O valor do produto não pode ser negativo.", nameof(price));

            Name = name;
            Url = url;
            Price = price;
            Active = active;
        }

        public Product()
        {
        }
    }
}
