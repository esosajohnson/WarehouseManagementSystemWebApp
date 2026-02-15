using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.Services;

namespace WarehouseManagementSystem.Controllers
{
    public class GoodsReceiptsController : Controller
    {
        private readonly WarehouseDbContext _context;
        private readonly InboundService _inboundService;

        public GoodsReceiptsController(WarehouseDbContext context, InboundService inboundService)
        {
            _context = context;
            _inboundService = inboundService;
        }

        // GET: GoodsReceipts
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var warehouseDbContext = _context.GoodsReceipts.Include(g => g.Employee).Include(g => g.PurchaseOrder).Include(g => g.Supplier);
            return View(await warehouseDbContext.ToListAsync());
        }

        // GET: GoodsReceipts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goodsReceipt = await _context.GoodsReceipts
                .Include(g => g.Employee)
                .Include(g => g.PurchaseOrder)
                .Include(g => g.Supplier)
                .Include (g => g.GoodsReceiptItems)
                    .ThenInclude(i => i.Product)
                .Include (g => g.GoodsReceiptItems)
                    .ThenInclude(i => i.Location)
                .FirstOrDefaultAsync(m => m.GoodsReceiptId == id);
            if (goodsReceipt == null)
            {
                return NotFound();
            }

            return View(goodsReceipt);
        }

        // GET: GoodsReceipts/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FullName", null);
            ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrders, "PurchaseOrderId", "PurchaseOrderId", null);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "Name", null);
            return View();
        }

        // POST: GoodsReceipts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GoodsReceiptId,PurchaseOrderId,SupplierId,EmployeeId,ReceiptDate,ReferenceNumber,Notes,Status")] GoodsReceipt goodsReceipt)
        {
            if (ModelState.IsValid)
            {
                _context.Add(goodsReceipt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FullName", goodsReceipt.EmployeeId);
            ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrders, "PurchaseOrderId", "PurchaseOrderId", goodsReceipt.PurchaseOrderId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "Name", goodsReceipt.SupplierId);
            return View(goodsReceipt);
        }

        // GET: GoodsReceipts/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goodsReceipt = await _context.GoodsReceipts.FindAsync(id);
            if (goodsReceipt == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FullName", goodsReceipt.EmployeeId);
            ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrders, "PurchaseOrderId", "PurchaseOrderId", goodsReceipt.PurchaseOrderId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "Name", goodsReceipt.SupplierId);
            return View(goodsReceipt);
        }

        // POST: GoodsReceipts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GoodsReceiptId,PurchaseOrderId,SupplierId,EmployeeId,ReceiptDate,ReferenceNumber,Notes,Status")] GoodsReceipt goodsReceipt)
        {
            if (id != goodsReceipt.GoodsReceiptId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(goodsReceipt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoodsReceiptExists(goodsReceipt.GoodsReceiptId))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", goodsReceipt.EmployeeId);
            ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrders, "PurchaseOrderId", "PurchaseOrderId", goodsReceipt.PurchaseOrderId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierId", goodsReceipt.SupplierId);
            return View(goodsReceipt);
        }

        // GET: GoodsReceipts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goodsReceipt = await _context.GoodsReceipts
                .Include(g => g.Employee)
                .Include(g => g.PurchaseOrder)
                .Include(g => g.Supplier)
                .FirstOrDefaultAsync(m => m.GoodsReceiptId == id);
            if (goodsReceipt == null)
            {
                return NotFound();
            }

            return View(goodsReceipt);
        }

        // POST: GoodsReceipts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var goodsReceipt = await _context.GoodsReceipts.FindAsync(id);
            if (goodsReceipt != null)
            {
                _context.GoodsReceipts.Remove(goodsReceipt);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Receive(int id)
        {
            await _inboundService.ReceiveGoodsAsync(id);
            return RedirectToAction(nameof(Index));
        }


        private bool GoodsReceiptExists(int id)
        {
            return _context.GoodsReceipts.Any(e => e.GoodsReceiptId == id);
        }
    }
}
