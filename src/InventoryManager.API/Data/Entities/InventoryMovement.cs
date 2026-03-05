namespace InventoryManager.API.Data.Entities
{
    public class InventoryMovement
    {
        public int Id { get; set; }
        public int ProductId {get; set;}
        public int Quantity { get; set;}
        public string Type { get; set;}
        public DateTime Date { get; set;} = DateTime.Now;
        public string Reason { get; set;}

    }
}
