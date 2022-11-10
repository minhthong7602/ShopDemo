namespace ShopDemo.Infrastructure.DTO
{
    public class OrderViewDTO
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public DateTime OrderDate { get; set; }
        public int Amount { get; set; }
    }
}
