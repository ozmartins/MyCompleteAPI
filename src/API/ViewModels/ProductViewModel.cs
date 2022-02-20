using System;
using System.ComponentModel.DataAnnotations;

namespace API.ViewModels
{
    public class ProductViewModel
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 2)]
        public string Description { get; set; }

        public string ImageUpload { get; set; }

        public string Image { get; set; }

        [Required]
        public decimal Price { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreationDate { get; set; }

        public bool Active { get; set; }

        [Required]
        public Guid SupplierId { get; set; }
        
        [ScaffoldColumn(false)]
        public String SupplierName { get; set; }
    }
}