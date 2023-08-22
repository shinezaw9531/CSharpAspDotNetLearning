using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ConsumeWebAPI.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [DisplayName("Product Name")]
        [Required]
        public string ProductName { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Qty { get; set; }
    }
}
