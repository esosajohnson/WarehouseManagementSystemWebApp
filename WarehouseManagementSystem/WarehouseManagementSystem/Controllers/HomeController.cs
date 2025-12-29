using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WarehouseDbContext _context;

        public HomeController(ILogger<HomeController> logger, WarehouseDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalProducts = await _context.Products.CountAsync();
            var totalSuppliers = await _context.Suppliers.CountAsync();
            var totalCategories = await _context.Categories.CountAsync();
            var totalLocations = await _context.Locations.CountAsync();
            var totalEmployees = await _context.Employees.CountAsync();
            var InventoryTransactions = await _context.InventoryTransactions.CountAsync();
            var PurchaseOrders = await _context.PurchaseOrders.CountAsync();
            var ReturnTransactions = await _context.ReturnTransactions.CountAsync();
            var OutboundShipments = await _context.Shipments.CountAsync();
            var GoodsReceipts = await _context.GoodsReceipts.CountAsync();
            var StockLevels = await _context.StockLevels.CountAsync();

            ViewData["TotalProducts"] = totalProducts;
            ViewData["TotalSuppliers"] = totalSuppliers;
            ViewData["TotalCategories"] = totalCategories;
            ViewData["TotalLocations"] = totalLocations;
            ViewData["TotalEmployees"] = totalEmployees;
            ViewData["InventoryTransactions"] = InventoryTransactions;
            ViewData["PurchaseOrders"] = PurchaseOrders;
            ViewData["ReturnTransactions"] = ReturnTransactions;
            ViewData["OutboundShipments"] = OutboundShipments;
            ViewData["GoodsReceipts"] = GoodsReceipts;
            ViewData["StockLevels"] = StockLevels;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
