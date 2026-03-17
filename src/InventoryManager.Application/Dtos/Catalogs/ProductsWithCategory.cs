namespace InventoryManager.Application.Dtos.Catalogs
{
    public class ProductsWithCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public CategoryDto Category { get; set; } = new();

    }
}
