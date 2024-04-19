using Electronic_Mall.Models;
using System.ComponentModel.DataAnnotations;

namespace Electronic_Mall
{
    public class ProductViewModel
    {
        [Required]
        public int Categoryid { get; set; }
        //  <!--@Model.NameProductName-->
        [Required]
        public string? ProductName { get; set; }
        [Required]
        public string? ProductDescription { get; set; }
        [Required]
        public decimal ProductPrice { get; set; }
        [Required]
        public int ProductQuantity { get; set; }
        // public IFormFile? Photo { get; set; }
        public IFormFile? Photo { get; set; }
        public virtual Category ProductCategory { get; set; } = null!;
    }
}