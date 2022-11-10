using ShopDemo.Infrastructure.DTO;
using UnitTest.Mock;

namespace UnitTest
{
    public class OrderRepostiroyTest
    {

        [Fact]
        public void GetAllOrderView()
        {
            // ARRANGE 
            var orderRepositoryMock = new OrderRepositoryMock().GetAllOrderView(
                Enumerable.Empty<OrderViewDTO>().AsQueryable()
            );
            //ACT
            var orders = orderRepositoryMock.Object.GetAllOrderView(string.Empty, "product_desc");

            // ASSERT
            Assert.True(orders.Count() == 0);
        }
    }
}
