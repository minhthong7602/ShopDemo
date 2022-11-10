using Moq;
using ShopDemo.Infrastructure.DTO;
using ShopDemo.Infrastructure.Repositories;

namespace UnitTest.Mock
{
    public class OrderRepositoryMock : Mock<IOrderRepository>
    {
        public OrderRepositoryMock GetAllOrderView(IQueryable<OrderViewDTO> orderViews)
        {
            Setup(x => x.GetAllOrderView(string.Empty, string.Empty)).Returns(orderViews);
            return this;
        }
    }
}
