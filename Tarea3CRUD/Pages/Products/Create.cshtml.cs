using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tarea3CRUD.Domain.Interfaces;
using Tarea3CRUD.Domain.Models;

namespace Tarea3CRUD.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;

        public CreateModel(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _productService.AddAsync(Product);
            TempData["SuccessMessage"] = $"El producto '{Product.Name}' se ha creado correctamente.";

            return RedirectToPage("./Index");
        }
    }
}
