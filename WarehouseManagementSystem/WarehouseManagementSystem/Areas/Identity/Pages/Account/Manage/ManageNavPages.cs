#nullable disable
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace WarehouseManagementSystem.Areas.Identity.Pages.Account.Manage
{
    public static class ManageNavPages
    {
        public static string Index => "Index";
        public static string EditProfile => "EditProfile";
        public static string ChangePassword => "ChangePassword";
        public static string SecuritySettings => "SecuritySettings";
        public static string Notifications => "Notifications";
        public static string ActivityLog => "ActivityLog";
        public static string UserManagement => "UserManagement";
        public static string DeleteAccount => "DeleteAccount";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);
        public static string EditProfileNavClass(ViewContext viewContext) => PageNavClass(viewContext, EditProfile);
        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);
        public static string SecurityNavClass(ViewContext viewContext) => PageNavClass(viewContext, SecuritySettings);
        public static string NotificationsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Notifications);
        public static string ActivityNavClass(ViewContext viewContext) => PageNavClass(viewContext, ActivityLog);
        public static string UserManagementNavClass(ViewContext viewContext) => PageNavClass(viewContext, UserManagement);
        public static string DeleteAccountNavClass(ViewContext viewContext) => PageNavClass(viewContext, DeleteAccount);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
