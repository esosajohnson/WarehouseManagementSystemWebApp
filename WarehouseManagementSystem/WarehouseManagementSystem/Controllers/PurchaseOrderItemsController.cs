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
    public class PurchaseOrderItemsController : Controller
    {
        private readonly WarehouseDbContext _context;

        public PurchaseOrderItemsController(WarehouseDbContext context)
        {
            _context = context;
        }

        // GET: PurchaseOrderItems
        public async Task<IActionResult> Index()
        {
            var warehouseDbContext = _context.PurchaseOrderItems.Include(p => p.Product).Include(p => p.PurchaseOrder);
            return View(await warehouseDbContext.ToListAsync());
        }

        // GET: PurchaseOrderItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrderItem = await _context.PurchaseOrderItems
                .Include(p => p.Product)
                .Include(p => p.PurchaseOrder)
                .FirstOrDefaultAsync(m => m.PurchaseOrderItemId == id);
            if (purchaseOrderItem == null)
            {
                return NotFound();
            }

            return View(purchaseOrderItem);
        }

        // GET: PurchaseOrderItems/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrders, "PurchaseOrderId", "PurchaseOrderId");
            return View();
        }

        // POST: PurchaseOrderItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PurchaseOrderItemId,PurchaseOrderId,ProductId,Quantity,UnitCost,LineTotal")] PurchaseOrderItem purchaseOrderItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchaseOrderItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", purchaseOrderItem.ProductId);
            ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrders, "PurchaseOrderId", "PurchaseOrderId", purchaseOrderItem.PurchaseOrderId);
            return View(purchaseOrderItem);
        }

        // GET: PurchaseOrderItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrderItem = await _context.PurchaseOrderItems.FindAsync(id);
            if (purchaseOrderItem == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", purchaseOrderItem.ProductId);
            ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrders, "PurchaseOrderId", "PurchaseOrderId", purchaseOrderItem.PurchaseOrderId);
            return View(purchaseOrderItem);
        }

        // POST: PurchaseOrderItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PurchaseOrderItemId,PurchaseOrderId,ProductId,Quantity,UnitCost,LineTotal")] PurchaseOrderItem purchaseOrderItem)
        {
            if (id != purchaseOrderItem.PurchaseOrderItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchaseOrderItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseOrderItemExists(purchaseOrderItem.PurchaseOrderItemId))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", purchaseOrderItem.ProductId);
            ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrders, "PurchaseOrderId", "PurchaseOrderId", purchaseOrderItem.PurchaseOrderId);
            return View(purchaseOrderItem);
        }

        // GET: PurchaseOrderItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrderItem = await _context.PurchaseOrderItems
                .Include(p => p.Product)
                .Include(p => p.PurchaseOrder)
                .FirstOrDefaultAsync(m => m.PurchaseOrderItemId == id);
            if (purchaseOrderItem == null)
            {
                return NotFound();
            }

            return View(purchaseOrderItem);
        }

        // POST: PurchaseOrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchaseOrderItem = await _context.PurchaseOrderItems.FindAsync(id);
            if (purchaseOrderItem != null)
            {
                _context.PurchaseOrderItems.Remove(purchaseOrderItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseOrderItemExists(int id)
        {
            return _context.PurchaseOrderItems.Any(e => e.PurchaseOrderItemId == id);
        }
    }
}
