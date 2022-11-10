namespace ShopDemo.Domain.Entities
{
    public class Order
    {
        public int Id { get; private set; }
        public int CustomerId { get; private set; }
        public int ProductId { get; private set; }
        public int Amount { get; private set; }
        public DateTime OrderDate { get; private set; }
        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
        public Order(int customerId, int productId, int amount, DateTime orderDate)
        {
            this.CustomerId = customerId;
            this.ProductId = productId;
            this.Amount = amount;
            this.OrderDate = orderDate;
        }
    }
}
