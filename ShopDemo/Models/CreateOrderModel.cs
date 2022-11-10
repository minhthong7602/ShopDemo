using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace ShopDemo.Web.Models
{
    public class CreateOrderModel
    {
        public string OrderName { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [Min(1, ErrorMessage = "Amount at least 1")]
        public int Amount { get; set; }

        public CreateOrderModel()
        {
            OrderDate = DateTime.Now;
        }
    }
}
