using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Services
{
    public class InboundService
    {
        private readonly WarehouseDbContext _context;

        public InboundService(WarehouseDbContext context)
        {
            _context = context;
        }

        public async Task ReceiveGoodsAsync(int goodsReceiptId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var receipt = await _context.GoodsReceipts
                    .Include(gr => gr.GoodsReceiptItems)
                    .FirstOrDefaultAsync(gr => gr.GoodsReceiptId == goodsReceiptId);

                if (receipt == null)
                    throw new Exception("Goods receipt not found.");

                if (receipt.Status == "Received")
                    throw new Exception("Goods receipt has already been processed.");

                foreach (var item in receipt.GoodsReceiptItems)
                {
                    await ProcessReceiptItem(item, receipt.EmployeeId);
                }

                if (receipt.PurchaseOrderId.HasValue)
                {
                    var po = await _context.PurchaseOrders.FindAsync(receipt.PurchaseOrderId.Value);
                    if (po != null)
                    {
                        po.OrderStatus = "Received";
                        _context.PurchaseOrders.Update(po);
                    }

                    receipt.Status = "Received";

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        private async Task ProcessReceiptItem(GoodsReceiptItem item, int? employeeId)
        {
            if (item.QuantityReceived <= 0)
                throw new Exception($"Invalid quantity received for product {item.ProductId}.");

            var stock = await _context.StockLevels.FirstOrDefaultAsync(s => 
                s.ProductId == item.ProductId && 
                s.LocationId == item.LocationId);

            if (stock == null) 
            {
                stock = new StockLevel
                {
                    ProductId = item.ProductId,
                    LocationId = item.LocationId,
                    QuantityChanged = item.QuantityReceived
                };
                _context.StockLevels.Add(stock);
            }
            else
            {
                stock.QuantityChanged += item.QuantityReceived;
                _context.StockLevels.Update(stock);
            }

            _context.InventoryTransactions.Add(new InventoryTransaction
            {
                ProductId = item.ProductId,
                QuantityChanged = item.QuantityReceived,
                TransactionType = "Inbound",
                TransactionDate = DateTime.UtcNow,
                EmployeeId = employeeId,
                Notes = $"Received {item.QuantityReceived} units at location {item.LocationId}."
            });
        }
    }
}
