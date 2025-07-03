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

            ViewData["TotalProducts"] = totalProducts;
            ViewData["TotalSuppliers"] = totalSuppliers;
            ViewData["TotalCategories"] = totalCategories;

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
