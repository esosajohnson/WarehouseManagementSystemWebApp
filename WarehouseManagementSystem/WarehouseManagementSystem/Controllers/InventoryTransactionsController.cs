using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Controllers
{
    public class InventoryTransactionsController : Controller
    {
        private readonly WarehouseDbContext _context;

        public InventoryTransactionsController(WarehouseDbContext context)
        {
            _context = context;
        }

        // GET: InventoryTransactions
        public async Task<IActionResult> Index()
        {
            var transactions = _context.InventoryTransactions
                .Include(t => t.Product)
                .Include(t => t.Location)
                .Include(t => t.Employee);

            return View(await transactions.ToListAsync());
        }

        // GET: InventoryTransactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var inventoryTransaction = await _context.InventoryTransactions
                .Include(t => t.Product)
                .Include(t => t.Location)
                .Include(t => t.Employee)
                .FirstOrDefaultAsync(m => m.TransactionId == id);

            if (inventoryTransaction == null) return NotFound();

            return View(inventoryTransaction);
        }

        // GET: InventoryTransactions/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name");
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "Name");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName");
            return View();
        }

        // POST: InventoryTransactions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactionId,ProductId,QuantityChanged,TransactionType,TransactionDate,Notes,EmployeeId,LocationId,ReferenceId")] InventoryTransaction inventoryTransaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventoryTransaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", inventoryTransaction.ProductId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "Name", inventoryTransaction.LocationId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", inventoryTransaction.EmployeeId);
            return View(inventoryTransaction);
        }

        // GET: InventoryTransactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var inventoryTransaction = await _context.InventoryTransactions.FindAsync(id);
            if (inventoryTransaction == null) return NotFound();

            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", inventoryTransaction.ProductId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "Name", inventoryTransaction.LocationId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", inventoryTransaction.EmployeeId);
            return View(inventoryTransaction);
        }

        // POST: InventoryTransactions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransactionId,ProductId,QuantityChanged,TransactionType,TransactionDate,Notes,EmployeeId,LocationId,ReferenceId")] InventoryTransaction inventoryTransaction)
        {
            if (id != inventoryTransaction.TransactionId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventoryTransaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryTransactionExists(inventoryTransaction.TransactionId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", inventoryTransaction.ProductId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "Name", inventoryTransaction.LocationId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", inventoryTransaction.EmployeeId);
            return View(inventoryTransaction);
        }

        // GET: InventoryTransactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var inventoryTransaction = await _context.InventoryTransactions
                .Include(t => t.Product)
                .Include(t => t.Location)
                .Include(t => t.Employee)
                .FirstOrDefaultAsync(m => m.TransactionId == id);

            if (inventoryTransaction == null) return NotFound();

            return View(inventoryTransaction);
        }

        // POST: InventoryTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventoryTransaction = await _context.InventoryTransactions.FindAsync(id);
            if (inventoryTransaction != null)
            {
                _context.InventoryTransactions.Remove(inventoryTransaction);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryTransactionExists(int id)
        {
            return _context.InventoryTransactions.Any(e => e.TransactionId == id);
        }
    }
}
