namespace WarehouseManagementSystem.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Email { get; set; }
        public IList<string> Role { get; set; }

    }
}
