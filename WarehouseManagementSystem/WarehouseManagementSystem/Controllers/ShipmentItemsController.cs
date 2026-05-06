using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Controllers
{
    public class ShipmentItemsController : Controller
    {
        private readonly WarehouseDbContext _context;

        public ShipmentItemsController(WarehouseDbContext context)
        {
            _context = context;
        }

        // GET: ShipmentItems
        public async Task<IActionResult> Index()
        {
            var warehouseDbContext = _context.ShipmentItems.Include(s => s.Location).Include(s => s.Product).Include(s => s.Shipment);
            return View(await warehouseDbContext.ToListAsync());
        }

        // GET: ShipmentItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipmentItem = await _context.ShipmentItems
                .Include(s => s.Location)
                .Include(s => s.Product)
                .Include(s => s.Shipment)
                .FirstOrDefaultAsync(m => m.ShipmentItemId == id);
            if (shipmentItem == null)
            {
                return NotFound();
            }

            return View(shipmentItem);
        }

        // GET: ShipmentItems/Create
        public IActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "LocationId");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            ViewData["ShipmentId"] = new SelectList(_context.Shipments, "ShipmentId", "ShipmentId");
            return View();
        }

        // POST: ShipmentItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShipmentItemId,ShipmentId,ProductId,LocationId,QuantityShipped,UnitPrice,LineTotal")] ShipmentItem shipmentItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shipmentItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "LocationId", shipmentItem.LocationId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", shipmentItem.ProductId);
            ViewData["ShipmentId"] = new SelectList(_context.Shipments, "ShipmentId", "ShipmentId", shipmentItem.ShipmentId);
            return View(shipmentItem);
        }

        // GET: ShipmentItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipmentItem = await _context.ShipmentItems.FindAsync(id);
            if (shipmentItem == null)
            {
                return NotFound();
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "LocationId", shipmentItem.LocationId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", shipmentItem.ProductId);
            ViewData["ShipmentId"] = new SelectList(_context.Shipments, "ShipmentId", "ShipmentId", shipmentItem.ShipmentId);
            return View(shipmentItem);
        }

        // POST: ShipmentItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShipmentItemId,ShipmentId,ProductId,LocationId,QuantityShipped,UnitPrice,LineTotal")] ShipmentItem shipmentItem)
        {
            if (id != shipmentItem.ShipmentItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shipmentItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShipmentItemExists(shipmentItem.ShipmentItemId))
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
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "LocationId", shipmentItem.LocationId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", shipmentItem.ProductId);
            ViewData["ShipmentId"] = new SelectList(_context.Shipments, "ShipmentId", "ShipmentId", shipmentItem.ShipmentId);
            return View(shipmentItem);
        }

        // GET: ShipmentItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipmentItem = await _context.ShipmentItems
                .Include(s => s.Location)
                .Include(s => s.Product)
                .Include(s => s.Shipment)
                .FirstOrDefaultAsync(m => m.ShipmentItemId == id);
            if (shipmentItem == null)
            {
                return NotFound();
            }

            return View(shipmentItem);
        }

        // POST: ShipmentItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shipmentItem = await _context.ShipmentItems.FindAsync(id);
            if (shipmentItem != null)
            {
                _context.ShipmentItems.Remove(shipmentItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShipmentItemExists(int id)
        {
            return _context.ShipmentItems.Any(e => e.ShipmentItemId == id);
        }
    }
}
