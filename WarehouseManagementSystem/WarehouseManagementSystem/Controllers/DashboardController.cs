using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly WarehouseDbContext _context;

        public DashboardController(WarehouseDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalProducts = await _context.Products.CountAsync();
            var lowStock = await _context.Products.Where(p => p.Quantity < 10).ToListAsync();
            var totalSuppliers = await _context.Suppliers.CountAsync();
            var totalCategories = await _context.Categories.CountAsync();

            var recentTransactions = await _context.InventoryTransactions
                .Include(t => t.Product)
                .Include(t => t.Employee)
                .OrderByDescending(t => t.TransactionDate)
                .Take(5)
                .ToListAsync();

            ViewData["TotalProducts"] = totalProducts;
            ViewData["LowStock"] = lowStock;
            ViewData["TotalSuppliers"] = totalSuppliers;
            ViewData["TotalCategories"] = totalCategories;
            ViewData["RecentTransactions"] = recentTransactions;

            return View();
        }
    }
}
