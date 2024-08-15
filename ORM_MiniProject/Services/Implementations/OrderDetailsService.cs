using ORM_MiniProject.DTOs.OrderDetailDTOs;
using ORM_MiniProject.Exceptions;
using ORM_MiniProject.Models;
using ORM_MiniProject.Repositories.Implementations;
using ORM_MiniProject.Repositories.Interfaces;
using ORM_MiniProject.Services.Interfaces;

namespace ORM_MiniProject.Services.Implementations
{
    public class OrderDetailsService : IOrderDetailsService
    {
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IProductsRepository _productsRepository;
        public OrderDetailsService()
        {
            _orderDetailsRepository= new OrderDetailsRepository();
            _ordersRepository = new OrdersRepository();
            _productsRepository = new ProductsRepository();
        }
        //public async Task CreateOrderDetailAsync(OrderDetailsPostDto orderDetail)
        //{
        //    var orderExists = await _ordersRepository.IsExistAsync(x => x.Id == orderDetail.OrderId);
        //    if (!orderExists) throw new NotFoundException("Order not found");

        //    var productExists = await _productsRepository.IsExistAsync(x => x.Id == orderDetail.ProductId);
        //    if (!productExists) throw new NotFoundException("Product not found");

        //    if (orderDetail.Quantity <= 0) throw new InvalidOrderDetailException("Quantity must be greater than zero");
        //    if (orderDetail.PricePerItem <= 0) throw new InvalidOrderDetailException("Price per item must be greater than zero");

        //    OrderDetails newOrderDetail = new OrderDetails
        //    {
        //        OrderId = orderDetail.OrderId,
        //        ProductId = orderDetail.ProductId,
        //        Quantity = orderDetail.Quantity,
        //        PricePerItem = orderDetail.PricePerItem
        //    };

        //    await _orderDetailsRepository.CreateAsync(newOrderDetail);
        //    await _orderDetailsRepository.SaveChangesAsync();
        //}

        public async Task DeleteOrderDetailAsync(int id)
        {
            var orderDetail = await _getOrderDetailById(id);
            _orderDetailsRepository.Delete(orderDetail);
            await _orderDetailsRepository.SaveChangesAsync();
        }

        public async Task<List<OrderDetailsGetDto>> GetAllOrderDetailsAsync()
        {
            var orderDetails = await _orderDetailsRepository.GetAllAsync("Order,Product");
            List<OrderDetailsGetDto> result = new List<OrderDetailsGetDto>();

            foreach (var detail in orderDetails)
            {
                OrderDetailsGetDto dto = new OrderDetailsGetDto
                {
                    Id = detail.Id,
                    OrderId = detail.OrderId,
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    PricePerItem = detail.PricePerItem,
                    Order = detail.Order,
                    Product = detail.Product
                };
                result.Add(dto);
            }

            return result;
        }

        public async Task<OrderDetailsGetDto> GetOrderDetailAsync(int id)
        {
            var orderDetail = await _getOrderDetailById(id);
            OrderDetailsGetDto dto = new OrderDetailsGetDto
            {
                Id = orderDetail.Id,
                OrderId = orderDetail.OrderId,
                ProductId = orderDetail.ProductId,
                Quantity = orderDetail.Quantity,
                PricePerItem = orderDetail.PricePerItem,
                Order = orderDetail.Order,
                Product = orderDetail.Product
            };

            return dto;
        }

        public async Task UpdateOrderDetailAsync(OrderDetailsPutDto orderDetail)
        {
            var dbOrderDetail = await _getOrderDetailById(orderDetail.Id);
            var orderExists = await _ordersRepository.IsExistAsync(x => x.Id == orderDetail.OrderId);
            if (!orderExists) throw new NotFoundException("Order not found");

            var productExists = await _productsRepository.IsExistAsync(x => x.Id == orderDetail.ProductId);
            if (!productExists) throw new NotFoundException("Product not found");

            if (orderDetail.Quantity <= 0) throw new InvalidOrderDetailException("Quantity must be greater than zero");
            if (orderDetail.PricePerItem <= 0) throw new InvalidOrderDetailException("Price per item must be greater than zero");

            dbOrderDetail.OrderId = orderDetail.OrderId;
            dbOrderDetail.ProductId = orderDetail.ProductId;
            dbOrderDetail.Quantity = orderDetail.Quantity;
            dbOrderDetail.PricePerItem = orderDetail.PricePerItem;

            _orderDetailsRepository.Update(dbOrderDetail);
            await _orderDetailsRepository.SaveChangesAsync();
        }

        private async Task<OrderDetails> _getOrderDetailById(int id)
        {
            var orderDetail = await _orderDetailsRepository.GetAsync(x => x.Id == id);

            if (orderDetail is null)
                throw new NotFoundException("Order detail not found");

            return orderDetail;
        }
    }
}
