using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopDemo.Domain.Entities;
using ShopDemo.Infrastructure.DTO;
using ShopDemo.Infrastructure.Repositories;
using ShopDemo.Web.Models;

namespace ShopDemo.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public HomeController(ICustomerRepository customerRepository, IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string keyword, string sortOrder, int page = 1)
        {
            ViewData["productSort"] = sortOrder == "product_asc" ? "product_desc" : "product_asc";
            ViewData["categorySort"] = sortOrder == "category_asc" ? "category_desc" : "category_asc";
            ViewData["customerSort"] = sortOrder == "customer_asc" ? "customer_desc" : "customer_asc";
            ViewData["currentSort"] = sortOrder;
            ViewData["keyword"] = keyword;
          //  page = page < 1 ? 1 : page;

            var orders = _orderRepository.GetAllOrderView(keyword, sortOrder);
            var ordersModel = await PaginatedList<OrderViewDTO>.CreateAsync(orders, page, pageSize: 1);
            return View(ordersModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var orderModel = new CreateOrderModel();
            await InitCreateForm();
            return View(orderModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderName,ProductId,CustomerId,OrderDate,Amount")] CreateOrderModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var product = await _productRepository.GetById(c => c.Id == model.ProductId, cancellationToken);
                if (product == null)
                {
                    return NotFound();
                }

                if (product.Quantity < model.Amount)
                {
                    ModelState.AddModelError("Amount", $"Product not enough, Please enter <= {product.Quantity}");
                    goto ErrorView;
                }

                await _orderRepository.Insert(new Order(model.CustomerId, model.ProductId, model.Amount, model.OrderDate), cancellationToken);
                product.OrderProduct(model.Amount);
                await _productRepository.Update(product, cancellationToken);
                return RedirectToAction(nameof(Index));
            }

        ErrorView:
            await InitCreateForm();
            return View(model);
        }

        private async Task InitCreateForm()
        {
            var products = await _productRepository.GetAll();
            var customers = await _customerRepository.GetAll();

            ViewData["Products"] = products.Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() }).ToList();
            ViewData["Customers"] = customers.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToList();
        }
    }
}