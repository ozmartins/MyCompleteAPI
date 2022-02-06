using System;
using System.ComponentModel.DataAnnotations;

namespace API.ViewModels
{
    public class AddressViewModel
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Street { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Number { get; set; }

        public string Complement { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 8)]
        public string ZipCode { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Neighborhood { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string CityName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string State { get; set; }

        public Guid SupplierId { get; set; }        
    }
}