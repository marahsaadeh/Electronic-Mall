using System.ComponentModel.DataAnnotations;

namespace Electronic_Mall
{
    public class CategoryViewModel
    {
        [Required]
        public string CategoryName { get; set; }

        public IFormFile Photo { get; set; }
    }

}
