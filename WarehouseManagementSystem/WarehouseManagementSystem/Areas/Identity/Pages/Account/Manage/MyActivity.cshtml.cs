using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WarehouseManagementSystem.Areas.Identity.Pages.Account.Manage
{
    public class MyActivityModel : PageModel
    {
        public List<ActivityItem> Activities { get; set; }

        public void OnGet()
        {
            Activities = new List<ActivityItem>
            {
                new ActivityItem("Login Successful", "bi-box-arrow-in-right", DateTime.Now.AddHours(-1)),
                new ActivityItem("Password Changed", "bi-key", DateTime.Now.AddDays(-1)),
                new ActivityItem("Profile Updated", "bi-person", DateTime.Now.AddDays(-2)),
                new ActivityItem("2FA Enabled", "bi-shield-lock", DateTime.Now.AddDays(-4)),
            };
        }

        public record ActivityItem(string Action, string Icon, DateTime Timestamp);
    }
}
