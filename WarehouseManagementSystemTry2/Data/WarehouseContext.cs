using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Data
{
    public class WarehouseContext : DbContext
    {
        public WarehouseContext(DbContextOptions<WarehouseContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<InventoryEntry> InventoryEntries { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
    }
}