using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementSystem.Models
{
    public class Category : MainModel
    {
        public Category() 
        { 
            this.Products = new HashSet<Product>();
        }

        [Required]
        public int Name { get; set; }
        public ICollection<Product> Products { get; set; }

    }
}
