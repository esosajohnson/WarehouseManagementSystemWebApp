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

            var RecentGoodsReceipts = await _context.GoodsReceipts
                .Include(gr => gr.Supplier)
                .OrderByDescending(gr => gr.ReceiptDate)
                .Take(5)
                .ToListAsync();

            var RecentShipments = await _context.Shipments
                .OrderByDescending(s => s.ShippingDate)
                .Take(5)
                .ToListAsync();

            ViewData["TotalProducts"] = totalProducts;
            ViewData["LowStock"] = lowStock;
            ViewData["TotalSuppliers"] = totalSuppliers;
            ViewData["TotalCategories"] = totalCategories;
            ViewData["RecentTransactions"] = recentTransactions;
            ViewData["RecentGoodsReceipts"] = RecentGoodsReceipts;
            ViewData["RecentShipments"] = RecentShipments;

            return View();
        }
    }
}
