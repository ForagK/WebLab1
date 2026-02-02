using Microsoft.AspNetCore.Mvc;
using WebLab1.Models;

namespace WebLab1.Services
{
    public class ProductService
    {
        private static readonly List<Product> _products = new();
        private static int _nextId = 1;

        public Product CreateProduct(Product product)
        {
            product.Id = _nextId++;

            _products.Add(product);

            return product;
        }

        public bool DeleteProduct(int id)
        {
            var product = GetProduct(id);
            if (product == null)
                return false;

            _products.Remove(product);

            return true;
        }

        public Product? GetProduct(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public bool UpdateProduct(int id, Product product)
        {
            int index = _products.FindIndex(p => p.Id == id);

            if (index == -1)
                return false;

            product.Id = id;
            _products[index] = product;
            return true;
        }

        public bool PatchProduct(int id, Dictionary<string, object> newProduct)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return false;

            if (newProduct.ContainsKey("Name"))
                product.Name = newProduct["Name"].ToString()!;

            if (newProduct.ContainsKey("Price"))
                if (float.TryParse(newProduct["Price"].ToString(), out var price))
                    product.Price = price;

            if (newProduct.ContainsKey("Description"))
                product.Description = newProduct["Description"].ToString()!;

            return true;
        }

        public string GenerateETag(Product product)
        {
            return $"{product.Name}{product.Price}".GetHashCode().ToString();
        }
    }
}
