namespace WarehouseManagementSystem.Models
{
    public class Shipment
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Destination { get; set; }
        public DateTime Date { get; set; }
    }
}