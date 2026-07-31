using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tarea3CRUD.Domain.Interfaces;
using Tarea3CRUD.Domain.Models;

namespace Tarea3CRUD.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public List<string> Categories { get; set; } = new List<string>();

        [BindProperty(SupportsGet = true)]
        public string? SearchQuery { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SelectedCategory { get; set; }

        public async Task OnGetAsync()
        {
            var allProducts = await _productService.GetAllAsync();

            Categories = allProducts
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                allProducts = allProducts.Where(p => 
                    p.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) || 
                    (p.Description != null && p.Description.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))
                );
            }

            if (!string.IsNullOrEmpty(SelectedCategory))
            {
                allProducts = allProducts.Where(p => p.Category.Equals(SelectedCategory, StringComparison.OrdinalIgnoreCase));
            }

            Products = allProducts.OrderBy(p => p.Id);
        }
    }
}
