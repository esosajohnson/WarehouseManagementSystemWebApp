using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementSystem.Models
{
    public class Product : MainModel
    {
        public Guid CategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public decimal Cost { get; set; }

        [Required]
        public int StockCount { get; set; }

        public string ImageUrl { get; set; }

        public Category Category { get; set; }
    }
}
