using Microsoft.AspNetCore.Mvc.RazorPages;
using Tarea3CRUD.Domain.Interfaces;

namespace Tarea3CRUD.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public int TotalProducts { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }
        public decimal AveragePrice { get; set; }

        public async Task OnGetAsync()
        {
            var products = await _productService.GetAllAsync();

            TotalProducts = products.Count();
            LowStockProducts = products.Count(p => p.Stock > 0 && p.Stock <= 5);
            OutOfStockProducts = products.Count(p => p.Stock == 0);
            AveragePrice = products.Any() ? products.Average(p => p.Price) : 0m;
        }
    }
}
