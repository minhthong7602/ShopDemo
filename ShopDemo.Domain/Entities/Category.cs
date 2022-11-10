namespace ShopDemo.Domain.Entities
{
    public class Category
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        public Category(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }
    }
}
