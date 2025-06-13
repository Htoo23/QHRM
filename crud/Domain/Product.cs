namespace crud.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Default to current time
    }

    // DTO for adding/updating products (avoids exposing internal IDs directly)
    public class ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
