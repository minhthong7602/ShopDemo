namespace ShopDemo.Domain.Entities
{
    public class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int CategoryId { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public int Quantity { get; private set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public void OrderProduct(int amount)
        {
            Quantity -= amount;
        }
        public Product(string name, int categoryId, decimal price, string description, int quantity)
        {
            Name = name;
            CategoryId = categoryId;
            Price = price;
            Description = description;
            Quantity = quantity;
        }
    }
}
