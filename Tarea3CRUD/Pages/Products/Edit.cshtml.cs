using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tarea3CRUD.Domain.Interfaces;
using Tarea3CRUD.Domain.Models;

namespace Tarea3CRUD.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;

        public EditModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public Product Product { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            Product = product;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingProduct = await _productService.GetByIdAsync(Product.Id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            await _productService.UpdateAsync(Product);
            TempData["SuccessMessage"] = $"El producto '{Product.Name}' se ha actualizado correctamente.";

            return RedirectToPage("./Index");
        }
    }
}
