using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Services
{
    public class OutboundService
    {
        private readonly WarehouseDbContext _context;
        
        public OutboundService(WarehouseDbContext context)
        {
            _context = context;
        }

        public async Task DispatchShipmentAsync(int shipmentId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                var shipment = await _context.Shipments
                    .Include(s => s.ShipmentItems)
                    .FirstOrDefaultAsync(s => s.ShipmentId == shipmentId);

                if (shipment == null)
                {
                    throw new InvalidOperationException("Shipment not found");
                }
                if (shipment.Status == ShipmentStatus.Dispatched)
                {
                    throw new InvalidOperationException("Shipment has already been dispatched");
                }
                if (shipment.ShipmentItems == null || !shipment.ShipmentItems.Any())
                {
                    throw new InvalidOperationException("Shipment has no items to dispatch");
                }
                foreach (var item in shipment.ShipmentItems)
                {
                    await ProcessShipmentItem(item, shipment.EmployeeId, shipment.ShipmentId);
                }

                shipment.Status = ShipmentStatus.Dispatched;
                shipment.ShippingDate = DateTime.UtcNow;
                _context.Shipments.Update(shipment);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task ProcessShipmentItem(ShipmentItem item, int? employeeId, int ShipmentId)
        {
            if (item.QuantityShipped <= 0)
                throw new InvalidOperationException($"Invalid quantity for product {item.ProductId}");

            var stock = await _context.StockLevels.FirstOrDefaultAsync(s => 
                s.ProductId == item.ProductId && s.
                LocationId == item.LocationId);

            if (stock == null || stock.QuantityOnHand < item.QuantityShipped)
                throw new InvalidOperationException(
                    $"Insufficient stock for product {item.ProductId} at location {item.LocationId}. " +
                    $"Available: {stock?.QuantityOnHand ?? 0}, Requested: {item.QuantityShipped}.");

            stock.QuantityOnHand -= item.QuantityShipped;
            _context.StockLevels.Update(stock);

            _context.InventoryTransactions.Add(new InventoryTransaction
            {
                ProductId = item.ProductId,
                LocationId = item.LocationId,
                QuantityChanged = -item.QuantityShipped,
                TransactionType = "Outbound",
                TransactionDate = DateTime.UtcNow,
                EmployeeId = employeeId,
                ReferenceId = item.ShipmentId,
                Notes = $"Dispatched {item.QuantityShipped} units from location {item.LocationId}."
            });
        }
    }
}
