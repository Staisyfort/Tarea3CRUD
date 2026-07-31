using System.Text.Json;
using Tarea3CRUD.Domain.Interfaces;
using Tarea3CRUD.Domain.Models;

namespace Tarea3CRUD.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly string _filePath;
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public ProductService()
        {
            var folderPath = Path.Combine(AppContext.BaseDirectory, "Data");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            _filePath = Path.Combine(folderPath, "products.json");
            SeedData();
        }

        private void SeedData()
        {
            if (!File.Exists(_filePath))
            {
                var initialProducts = new List<Product>
                {
                    new Product { Id = 1, Name = "Laptop Dell Inspiron", Category = "Electrónica", Price = 799.99m, Stock = 15, Description = "Laptop con procesador Intel i7, 16GB de RAM y 512GB SSD." },
                    new Product { Id = 2, Name = "Smartphone Samsung Galaxy S23", Category = "Electrónica", Price = 899.99m, Stock = 20, Description = "Smartphone con pantalla Dynamic AMOLED 2X y cámara de 50MP." },
                    new Product { Id = 3, Name = "Silla de Oficina Ergonómica", Category = "Muebles", Price = 149.99m, Stock = 30, Description = "Silla giratoria ergonómica con soporte lumbar y reposabrazos ajustable." },
                    new Product { Id = 4, Name = "Cafetera Express Nespresso", Category = "Hogar", Price = 199.99m, Stock = 10, Description = "Cafetera de cápsulas con sistema de alta presión y espumador de leche." },
                    new Product { Id = 5, Name = "Auriculares Inalámbricos Sony WH-1000XM4", Category = "Electrónica", Price = 279.99m, Stock = 25, Description = "Auriculares de diadema con cancelación de ruido activa." }
                };

                var json = JsonSerializer.Serialize(initialProducts, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
        }

        private async Task<List<Product>> ReadFromFileAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                if (!File.Exists(_filePath))
                {
                    return new List<Product>();
                }
                var json = await File.ReadAllTextAsync(_filePath);
                return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task SaveToFileAsync(List<Product> products)
        {
            await _semaphore.WaitAsync();
            try
            {
                var json = JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_filePath, json);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await ReadFromFileAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var products = await ReadFromFileAsync();
            return products.FirstOrDefault(p => p.Id == id);
        }

        public async Task AddAsync(Product product)
        {
            var products = await ReadFromFileAsync();
            product.Id = products.Any() ? products.Max(p => p.Id) + 1 : 1;
            products.Add(product);
            await SaveToFileAsync(products);
        }

        public async Task UpdateAsync(Product product)
        {
            var products = await ReadFromFileAsync();
            var index = products.FindIndex(p => p.Id == product.Id);
            if (index != -1)
            {
                products[index] = product;
                await SaveToFileAsync(products);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var products = await ReadFromFileAsync();
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                products.Remove(product);
                await SaveToFileAsync(products);
            }
        }
    }
}
