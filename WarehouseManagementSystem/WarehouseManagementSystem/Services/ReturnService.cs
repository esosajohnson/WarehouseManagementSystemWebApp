using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Services
{
    public class ReturnService
    {
        private readonly WarehouseDbContext _context;

        public ReturnService(WarehouseDbContext context)
        {
            _context = context;
        }

        public async Task RestockAsync(int returnTransactionId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var returnTx = await _context.ReturnTransactions
                    .FirstOrDefaultAsync(rt => rt.ReturnTransactionId == returnTransactionId);

                if (returnTx == null)
                    throw new InvalidOperationException($"Return transaction with ID {returnTransactionId} not found.");

                if (returnTx.ConditionStatus != ReturnStatus.Pending)
                    throw new InvalidOperationException($"Return transaction with ID {returnTransactionId} has already been processed.");

                if (returnTx.LocationId == null)
                    throw new InvalidOperationException($"Return transaction with ID {returnTransactionId} must have a valid location.");

                await RestockReturn(returnTx);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task WriteOffAsync(int returnTransactionId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var returnTx = await _context.ReturnTransactions
                    .FirstOrDefaultAsync(rt => rt.ReturnTransactionId == returnTransactionId);

                if (returnTx == null)
                    throw new InvalidOperationException($"Return transaction with ID {returnTransactionId} not found.");

                if (returnTx.ConditionStatus != ReturnStatus.Pending)
                    throw new InvalidOperationException($"Return transaction with ID {returnTransactionId} has already been processed.");

                WriteOffReturn(returnTx);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task RestockReturn(ReturnTransaction returnTx)
        {
            var stock = await _context.StockLevels.FirstOrDefaultAsync(s =>
                s.ProductId == returnTx.ProductId &&
                s.LocationId == returnTx.LocationId);

            if (stock == null)
            {
                stock = new StockLevel
                {
                    ProductId = returnTx.ProductId,
                    LocationId = returnTx.LocationId!.Value,
                    QuantityOnHand = returnTx.Quantity,
                    LastUpdated = DateTime.UtcNow
                };
                _context.StockLevels.Add(stock);
            }
            else
            {
                stock.QuantityOnHand += returnTx.Quantity;
                _context.StockLevels.Update(stock);
            }

            _context.InventoryTransactions.Add(new InventoryTransaction
            {
                ProductId = returnTx.ProductId,
                LocationId = returnTx.LocationId!.Value,
                QuantityChanged = returnTx.Quantity,
                TransactionType = "Return",
                TransactionDate = DateTime.UtcNow,
                EmployeeId = returnTx.ProcessedByEmployeeId,
                ReferenceId = returnTx.ReturnTransactionId,
                Notes = $"Restocked {returnTx.Quantity} units at location {returnTx.LocationId}."
            });

            returnTx.ConditionStatus = ReturnStatus.Restocked;
        }

        private void WriteOffReturn(ReturnTransaction returnTx)
        {
            _context.InventoryTransactions.Add(new InventoryTransaction
            {
                ProductId = returnTx.ProductId,
                LocationId = returnTx.LocationId ?? 0,
                QuantityChanged = 0,
                TransactionType = "Return Write-Off",
                TransactionDate = DateTime.UtcNow,
                EmployeeId = returnTx.ProcessedByEmployeeId,
                ReferenceId = returnTx.ReturnTransactionId,
                Notes = $"Written off {returnTx.Quantity} units - condition deemed unsellable."
            });

            returnTx.ConditionStatus = ReturnStatus.WrittenOff;
        }
    }
}
