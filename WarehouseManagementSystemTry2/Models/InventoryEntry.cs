namespace WarehouseManagementSystem.Models
{
    public class InventoryEntry
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
    }
}

