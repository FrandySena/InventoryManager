namespace InventoryManager.Application.Dtos
{
    public class InventoryMovementDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public String? Date { get; set; }
        public string Reason { get; set; }
    }
}