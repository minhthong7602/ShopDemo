namespace ShopDemo.Domain.Entities
{
    public class Customer
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public Customer(string name, string address)
        {
            Name = name; 
            Address = address;
        }
    }
}
