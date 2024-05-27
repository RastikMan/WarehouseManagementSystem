using Microsoft.AspNetCore.Mvc;
using WarehouseManagementSystem.Data;
using WarehouseManagementSystem.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WarehouseManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly WarehouseContext _context;

        public WarehouseController(WarehouseContext context)
        {
            _context = context;
        }

        // Прием товара на склад
        [HttpPost("receive")]
        public async Task<ActionResult<Product>> ReceiveProduct(InventoryEntry inventoryEntry)
        {
            var existingInventoryEntry = await _context.InventoryEntries
                .FirstOrDefaultAsync(entry => entry.ProductId == inventoryEntry.ProductId);

            if (existingInventoryEntry != null)
            {
                // Если запись инвентаризации уже существует, увеличиваем количество товара
                existingInventoryEntry.Quantity += inventoryEntry.Quantity;
            }
            else
            {
                // Если запись инвентаризации не существует, создаем новую запись
                _context.InventoryEntries.Add(inventoryEntry);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpGet("receipts")]
        public async Task<ActionResult<IEnumerable<InventoryEntry>>> GetReceipts()
        {
            return await _context.InventoryEntries.ToListAsync();
        }

        [HttpPut("receipts/{id}")]
        public async Task<ActionResult> UpdateReceipt(int id, [FromBody] InventoryEntry inventoryEntry)
        {
            if (id != inventoryEntry.Id)
            {
                return BadRequest();
            }

            _context.Entry(inventoryEntry).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("receipts/{id}")]
        public async Task<ActionResult> DeleteReceipt(int id)
        {
            var receipt = await _context.InventoryEntries.FindAsync(id);
            if (receipt == null)
            {
                return NotFound();
            }

            _context.InventoryEntries.Remove(receipt);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Отгрузка товара со склада
        [HttpPost("ship")]
        public async Task<ActionResult> ShipProduct([FromBody] Shipment shipment)
        {
            _context.Shipments.Add(shipment);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("shipments")]
        public async Task<ActionResult<IEnumerable<Shipment>>> GetShipments()
        {
            return await _context.Shipments.ToListAsync();
        }

        [HttpPut("shipments/{id}")]
        public async Task<ActionResult> UpdateShipment(int id, [FromBody] Shipment shipment)
        {
            if (id != shipment.Id)
            {
                return BadRequest();
            }

            _context.Entry(shipment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("shipments/{id}")]
        public async Task<ActionResult> DeleteShipment(int id)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null)
            {
                return NotFound();
            }

            _context.Shipments.Remove(shipment);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Просмотр остатков на складе
        [HttpGet("inventory")]
        public async Task<ActionResult<IEnumerable<Product>>> GetInventory()
        {
            var inventory = await _context.Products
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Category,
                    Quantity = _context.InventoryEntries.Where(i => i.ProductId == p.Id).Sum(i => i.Quantity) - _context.Shipments.Where(s => s.ProductId == p.Id).Sum(s => s.Quantity)
                })
                .ToListAsync();

            return Ok(inventory);
        }


    }


}