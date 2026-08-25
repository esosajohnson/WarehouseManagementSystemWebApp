
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WarehouseManagementSystem.Controllers
{
    public class ShipmentsController : Controller
    {
        private readonly WarehouseDbContext _context;
        private readonly OutboundService _outboundService;

        public ShipmentsController(WarehouseDbContext context, OutboundService outboundService)
        {
            _context = context;
            _outboundService = outboundService;
        }

        // GET: Shipments
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Shipments
                .Include(s => s.Employee)
                .ToListAsync());
        }

        // GET: Shipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipments
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(m => m.ShipmentId == id);

            if (shipment == null)
            {
                return NotFound();
            }

            return View(shipment);
        }

        // GET: Shipments/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            PopulateDropDowns(new Shipment());
            return View();
        }

        // POST: Shipments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShipmentId,EmployeeId,Carrier,TrackingNumber,ShippingDate,Destination,Status,Notes")] Shipment shipment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateDropDowns(shipment);
            return View(shipment);
        }

        // GET: Shipments/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null) return NotFound();

            PopulateDropDowns(shipment);
            return View(shipment);
        }

        // POST: Shipments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShipmentId,EmployeeId,Carrier,TrackingNumber,ShippingDate,Destination,Status,Notes")] Shipment shipment)
        {
            if (id != shipment.ShipmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShipmentExists(shipment.ShipmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateDropDowns(shipment);
            return View(shipment);
        }

        // GET: Shipments/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipments
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(m => m.ShipmentId == id);

            if (shipment == null)
            {
                return NotFound();
            }

            return View(shipment);
        }

        // POST: Shipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment != null)
            {
                _context.Shipments.Remove(shipment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Dispatch(int id)
        {
            try
            {
                await _outboundService.DispatchShipmentAsync(id);
                TempData["SuccessMessage"] = "Shipment dispatched successfully. Stock levels updated.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while dispatching the shipment: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ShipmentExists(int id)
        {
            return _context.Shipments.Any(e => e.ShipmentId == id);
        }

        private void PopulateDropDowns(Shipment shipment)
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FullName", shipment.EmployeeId);
        }
    }
}
