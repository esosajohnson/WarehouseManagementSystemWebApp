using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Services;
using System.Linq;
using System;

namespace WarehouseManagementSystem.Controllers
{
    public class ReturnTransactionsController : Controller
    {
        private readonly WarehouseDbContext _context;
        private readonly ReturnService _returnService;

        public ReturnTransactionsController(WarehouseDbContext context, ReturnService returnService)
        {
            _context = context;
            _returnService = returnService;
        }

        // GET: ReturnTransactions
        [AllowAnonymous]
        public async Task<IActionResult> Index(int? productId, string? status, int? employeeId)
        {
            var query = _context.ReturnTransactions
                .Include(r => r.Product)
                .Include(r => r.Location)
                .Include(r => r.Shipment)
                .Include(r => r.ProcessedByEmployee)
                .AsQueryable();

            if (productId.HasValue)
            {
                query = query.Where(r => r.ProductId == productId.Value);
            }

            if (!string.IsNullOrEmpty(status))
            {
                if (Enum.TryParse<ReturnStatus>(status, out var parsed))
                {
                    query = query.Where(r => r.ConditionStatus == parsed);
                }
            }

            if (employeeId.HasValue)
            {
                query = query.Where(r => r.ProcessedByEmployeeId == employeeId.Value);
            }

            var products = await _context.Products.ToListAsync();
            var employees = await _context.Employees.ToListAsync();

            ViewData["ProductId"] = new SelectList(products, "ProductId", "Name", productId);
            ViewData["StatusList"] = new SelectList(Enum.GetValues<ReturnStatus>().Cast<ReturnStatus>().Select(s => new { Value = s.ToString(), Text = s.ToString() }), "Value", "Text", status);
            ViewData["EmployeeId"] = new SelectList(employees, "EmployeeId", "FullName", employeeId);

            return View(await query.ToListAsync());
        }

        // GET: ReturnTransactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var returnTransaction = await _context.ReturnTransactions
                .Include(r => r.Product)
                .Include(r => r.Location)
                .Include(r => r.Shipment)
                .Include(r => r.ProcessedByEmployee)
                .FirstOrDefaultAsync(m => m.ReturnTransactionId == id);

            if (returnTransaction == null) return NotFound();

            return View(returnTransaction);
        }

        // GET: ReturnTransactions/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            PopulateDropDowns();
            return View();
        }

        // POST: ReturnTransactions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ReturnTransactionId,ProductId,LocationId,ShipmentId,Quantity,ReturnReason,ReturnDate,ProcessedByEmployeeId,ConditionStatus,Notes")] ReturnTransaction returnTransaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(returnTransaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateDropDowns(returnTransaction);
            return View(returnTransaction);
        }

        // GET: ReturnTransactions/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var returnTransaction = await _context.ReturnTransactions.FindAsync(id);
            if (returnTransaction == null) return NotFound();

            // Block editing of processed returns
            if (returnTransaction.ConditionStatus != ReturnStatus.Pending)
            {
                TempData["ErrorMessage"] = "Only pending returns can be edited.";
                return RedirectToAction(nameof(Index));
            }

            PopulateDropDowns(returnTransaction);
            return View(returnTransaction);
        }

        // POST: ReturnTransactions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ReturnTransactionId,ProductId,LocationId,ShipmentId,Quantity,ReturnReason,ReturnDate,ProcessedByEmployeeId,ConditionStatus,Notes")] ReturnTransaction returnTransaction)
        {
            if (id != returnTransaction.ReturnTransactionId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(returnTransaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReturnTransactionExists(returnTransaction.ReturnTransactionId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateDropDowns(returnTransaction);
            return View(returnTransaction);
        }

        // GET: ReturnTransactions/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var returnTransaction = await _context.ReturnTransactions
                .Include(r => r.Product)
                .Include(r => r.Location)
                .Include(r => r.Shipment)
                .Include(r => r.ProcessedByEmployee)
                .FirstOrDefaultAsync(m => m.ReturnTransactionId == id);

            if (returnTransaction == null) return NotFound();

            // Block deletion of processed returns
            if (returnTransaction.ConditionStatus != ReturnStatus.Pending)
            {
                TempData["ErrorMessage"] = "Only pending returns can be deleted.";
                return RedirectToAction(nameof(Index));
            }

            return View(returnTransaction);
        }

        // POST: ReturnTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var returnTransaction = await _context.ReturnTransactions.FindAsync(id);
            if (returnTransaction == null) return NotFound();

            if (returnTransaction.ConditionStatus != ReturnStatus.Pending)
            {
                TempData["ErrorMessage"] = "Only pending returns can be deleted.";
                return RedirectToAction(nameof(Index));
            }

            _context.ReturnTransactions.Remove(returnTransaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: ReturnTransactions/Restock/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Restock(int id)
        {
            try
            {
                await _returnService.RestockAsync(id);
                TempData["SuccessMessage"] = "Return successfully restocked. Stock levels have been updated.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while restocking the return. Please try again.";
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: ReturnTransactions/WriteOff/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> WriteOff(int id)
        {
            try
            {
                await _returnService.WriteOffAsync(id);
                TempData["SuccessMessage"] = "Return successfully written off.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while writing off the return. Please try again.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ReturnTransactionExists(int id)
        {
            return _context.ReturnTransactions.Any(e => e.ReturnTransactionId == id);
        }

        private void PopulateDropDowns(ReturnTransaction? selected = null)
        {
            ViewData["ProductId"] = new SelectList(
                _context.Products, "ProductId", "Name", selected?.ProductId);

            ViewData["LocationId"] = new SelectList(
                _context.Locations, "LocationId", "Name", selected?.LocationId);

            ViewData["ShipmentId"] = new SelectList(
                _context.Shipments
                    .Where(s => s.Status == ShipmentStatus.Dispatched)
                    .Select(s => new { s.ShipmentId, Display = "Shipment #" + s.ShipmentId }),
                "ShipmentId", "Display", selected?.ShipmentId);

            ViewData["ProcessedByEmployeeId"] = new SelectList(
                _context.Employees, "EmployeeId", "FullName", selected?.ProcessedByEmployeeId);
        }
    }
}