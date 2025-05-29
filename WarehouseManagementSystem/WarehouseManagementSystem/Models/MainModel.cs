namespace WarehouseManagementSystem.Models
{
    public class MainModel
    {
        public MainModel()
        {
            this.Id = Guid.NewGuid();

        }
        public Guid Id { get; set; }
    }
}
