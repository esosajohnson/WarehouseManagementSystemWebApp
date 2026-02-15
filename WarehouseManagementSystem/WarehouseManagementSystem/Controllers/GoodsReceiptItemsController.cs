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
    public class GoodsReceiptItemsController : Controller
    {
        private readonly WarehouseDbContext _context;

        public GoodsReceiptItemsController(WarehouseDbContext context)
        {
            _context = context;
        }

        // GET: GoodsReceiptItems
        public async Task<IActionResult> Index()
        {
            var warehouseDbContext = _context.GoodsReceiptItems.Include(g => g.GoodsReceipt).Include(g => g.Location).Include(g => g.Product);
            return View(await warehouseDbContext.ToListAsync());
        }

        // GET: GoodsReceiptItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goodsReceiptItem = await _context.GoodsReceiptItems
                .Include(g => g.GoodsReceipt)
                .Include(g => g.Location)
                .Include(g => g.Product)
                .FirstOrDefaultAsync(m => m.GoodsReceiptItemId == id);
            if (goodsReceiptItem == null)
            {
                return NotFound();
            }

            return View(goodsReceiptItem);
        }

        // GET: GoodsReceiptItems/Create
        public IActionResult Create()
        {
            ViewData["GoodsReceiptId"] = new SelectList(_context.GoodsReceipts, "GoodsReceiptId", "GoodsReceiptId");
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "LocationId");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            return View();
        }

        // POST: GoodsReceiptItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GoodsReceiptItemId,GoodsReceiptId,ProductId,QuantityReceived,LocationId,ExpiryDate,Notes")] GoodsReceiptItem goodsReceiptItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(goodsReceiptItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GoodsReceiptId"] = new SelectList(_context.GoodsReceipts, "GoodsReceiptId", "GoodsReceiptId", goodsReceiptItem.GoodsReceiptId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "LocationId", goodsReceiptItem.LocationId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", goodsReceiptItem.ProductId);
            return View(goodsReceiptItem);
        }

        // GET: GoodsReceiptItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goodsReceiptItem = await _context.GoodsReceiptItems.FindAsync(id);
            if (goodsReceiptItem == null)
            {
                return NotFound();
            }
            ViewData["GoodsReceiptId"] = new SelectList(_context.GoodsReceipts, "GoodsReceiptId", "GoodsReceiptId", goodsReceiptItem.GoodsReceiptId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "LocationId", goodsReceiptItem.LocationId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", goodsReceiptItem.ProductId);
            return View(goodsReceiptItem);
        }

        // POST: GoodsReceiptItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GoodsReceiptItemId,GoodsReceiptId,ProductId,QuantityReceived,LocationId,ExpiryDate,Notes")] GoodsReceiptItem goodsReceiptItem)
        {
            if (id != goodsReceiptItem.GoodsReceiptItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(goodsReceiptItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoodsReceiptItemExists(goodsReceiptItem.GoodsReceiptItemId))
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
            ViewData["GoodsReceiptId"] = new SelectList(_context.GoodsReceipts, "GoodsReceiptId", "GoodsReceiptId", goodsReceiptItem.GoodsReceiptId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "LocationId", goodsReceiptItem.LocationId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", goodsReceiptItem.ProductId);
            return View(goodsReceiptItem);
        }

        // GET: GoodsReceiptItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goodsReceiptItem = await _context.GoodsReceiptItems
                .Include(g => g.GoodsReceipt)
                .Include(g => g.Location)
                .Include(g => g.Product)
                .FirstOrDefaultAsync(m => m.GoodsReceiptItemId == id);
            if (goodsReceiptItem == null)
            {
                return NotFound();
            }

            return View(goodsReceiptItem);
        }

        // POST: GoodsReceiptItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var goodsReceiptItem = await _context.GoodsReceiptItems.FindAsync(id);
            if (goodsReceiptItem != null)
            {
                _context.GoodsReceiptItems.Remove(goodsReceiptItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GoodsReceiptItemExists(int id)
        {
            return _context.GoodsReceiptItems.Any(e => e.GoodsReceiptItemId == id);
        }
    }
}
