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
    public class StockLevelsController : Controller
    {
        private readonly WarehouseDbContext _context;

        public StockLevelsController(WarehouseDbContext context)
        {
            _context = context;
        }

        // GET: StockLevels
        public async Task<IActionResult> Index()
        {
            var warehouseDbContext = _context.StockLevels.Include(s => s.Location).Include(s => s.Product);
            return View(await warehouseDbContext.ToListAsync());
        }

        // GET: StockLevels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockLevel = await _context.StockLevels
                .Include(s => s.Location)
                .Include(s => s.Product)
                .FirstOrDefaultAsync(m => m.StockLevelId == id);
            if (stockLevel == null)
            {
                return NotFound();
            }

            return View(stockLevel);
        }

        // GET: StockLevels/Create
        public IActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "LocationId");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            return View();
        }

        // POST: StockLevels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StockLevelId,ProductId,LocationId,QuantityOnHand,LastUpdated")] StockLevel stockLevel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stockLevel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "LocationId", stockLevel.LocationId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", stockLevel.ProductId);
            return View(stockLevel);
        }

        // GET: StockLevels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockLevel = await _context.StockLevels.FindAsync(id);
            if (stockLevel == null)
            {
                return NotFound();
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "LocationId", stockLevel.LocationId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", stockLevel.ProductId);
            return View(stockLevel);
        }

        // POST: StockLevels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StockLevelId,ProductId,LocationId,QuantityOnHand,LastUpdated")] StockLevel stockLevel)
        {
            if (id != stockLevel.StockLevelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockLevel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockLevelExists(stockLevel.StockLevelId))
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
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "LocationId", stockLevel.LocationId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", stockLevel.ProductId);
            return View(stockLevel);
        }

        // GET: StockLevels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockLevel = await _context.StockLevels
                .Include(s => s.Location)
                .Include(s => s.Product)
                .FirstOrDefaultAsync(m => m.StockLevelId == id);
            if (stockLevel == null)
            {
                return NotFound();
            }

            return View(stockLevel);
        }

        // POST: StockLevels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stockLevel = await _context.StockLevels.FindAsync(id);
            if (stockLevel != null)
            {
                _context.StockLevels.Remove(stockLevel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockLevelExists(int id)
        {
            return _context.StockLevels.Any(e => e.StockLevelId == id);
        }
    }
}
