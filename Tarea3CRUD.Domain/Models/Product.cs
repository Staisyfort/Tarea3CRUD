using System.ComponentModel.DataAnnotations;

namespace Tarea3CRUD.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [StringLength(50, ErrorMessage = "La categoría no puede exceder los 50 caracteres.")]
        [Display(Name = "Categoría")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, 100000.00, ErrorMessage = "El precio debe ser un valor mayor a cero.")]
        [Display(Name = "Precio ($)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio.")]
        [Range(0, 10000, ErrorMessage = "El stock debe ser un número entero no negativo.")]
        [Display(Name = "Stock (Unidades)")]
        public int Stock { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        [Display(Name = "Descripción")]
        public string? Description { get; set; }
    }
}
