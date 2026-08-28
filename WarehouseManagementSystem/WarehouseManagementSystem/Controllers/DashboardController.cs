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
            // KPI cards
            ViewData["TotalStockUnits"] = await _context.StockLevels
                .SumAsync(s => (int?)s.QuantityOnHand) ?? 0;

            ViewData["InboundToday"] = await _context.InventoryTransactions
                .Where(t => t.TransactionType == "Inbound"
                         && t.TransactionDate.Date == DateTime.UtcNow.Date)
                .SumAsync(t => (int?)t.QuantityChanged) ?? 0;

            ViewData["OutboundToday"] = await _context.InventoryTransactions
                .Where(t => t.TransactionType == "Outbound"
                         && t.TransactionDate.Date == DateTime.UtcNow.Date)
                .SumAsync(t => (int?)t.QuantityChanged) ?? 0;

            ViewData["OpenPurchaseOrders"] = await _context.PurchaseOrders
                .Where(p => p.OrderStatus == PurchaseOrderStatus.Approved
                         || p.OrderStatus == PurchaseOrderStatus.PartiallyReceived)
                .CountAsync();

            // Critical alerts
            ViewData["LowStockCount"] = await _context.StockLevels
                .Where(s => s.QuantityOnHand < 10)
                .CountAsync();

            ViewData["PendingShipments"] = await _context.Shipments
                .Where(s => s.Status == ShipmentStatus.Pending)
                .CountAsync();

            // Use 0 for PickingTasks for now if not yet wired up
            ViewData["OpenPickingTasks"] = await _context.PickingTasks
                .CountAsync();

            // Recent activity
            ViewData["RecentGoodsReceipts"] = await _context.GoodsReceipts
                .Include(gr => gr.Supplier)
                .OrderByDescending(gr => gr.ReceiptDate)
                .Take(5)
                .ToListAsync();

            ViewData["RecentShipments"] = await _context.Shipments
                .OrderByDescending(s => s.ShippingDate)
                .Take(5)
                .ToListAsync();

            // Inventory flow chart data (last 7 days)
            var last7Days = Enumerable.Range(0, 7)
                .Select(i => DateTime.UtcNow.Date.AddDays(-6 + i))
                .ToList();

            var inboundData = await _context.InventoryTransactions
                .Where(t => t.TransactionType == "Inbound"
                         && t.TransactionDate.Date >= DateTime.UtcNow.Date.AddDays(-6))
                .GroupBy(t => t.TransactionDate.Date)
                .Select(g => new { Date = g.Key, Total = g.Sum(t => t.QuantityChanged) })
                .ToListAsync();

            var outboundData = await _context.InventoryTransactions
                .Where(t => t.TransactionType == "Outbound"
                         && t.TransactionDate.Date >= DateTime.UtcNow.Date.AddDays(-6))
                .GroupBy(t => t.TransactionDate.Date)
                .Select(g => new { Date = g.Key, Total = g.Sum(t => t.QuantityChanged) })
                .ToListAsync();

            var labels = last7Days.Select(d => d.ToString("dd MMM")).ToList();
            var inboundSeries = last7Days
                .Select(d => inboundData.FirstOrDefault(x => x.Date == d)?.Total ?? 0)
                .ToList();
            var outboundSeries = last7Days
                .Select(d => Math.Abs(outboundData.FirstOrDefault(x => x.Date == d)?.Total ?? 0))
                .ToList();

            ViewData["ChartLabels"] = System.Text.Json.JsonSerializer.Serialize(labels);
            ViewData["ChartInbound"] = System.Text.Json.JsonSerializer.Serialize(inboundSeries);
            ViewData["ChartOutbound"] = System.Text.Json.JsonSerializer.Serialize(outboundSeries);

            return View();
        }
    }
}