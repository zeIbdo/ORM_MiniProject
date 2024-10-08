﻿using ORM_MiniProject.DTOs.OrderDetailDTOs;
using ORM_MiniProject.DTOs.OrderDTOs;
using ORM_MiniProject.Exceptions;
using ORM_MiniProject.Models;
using ORM_MiniProject.Repositories.Implementations;
using ORM_MiniProject.Repositories.Interfaces;
using ORM_MiniProject.Services.Interfaces;

namespace ORM_MiniProject.Services.Implementations
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IPaymentsRepository _paymentsRepository;
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        private readonly IProductsRepository _productsRepository;
        public OrdersService()
        {
            _ordersRepository=new OrdersRepository();
            _usersRepository=new UsersRepository();
            _paymentsRepository=new PaymentsRepository();
            _orderDetailsRepository=new OrderDetailsRepository();
            _productsRepository=new ProductsRepository();
        }
        public async Task CreateOrderAsync(OrderPostDto order)
        {
            bool userExists = await _usersRepository.IsExistAsync(u => u.Id == order.UserId);
            if (!userExists) { throw new InvalidOrderException("User tapilmadi"); }
            if (order.TotalAmount < 0) throw new InvalidOrderException("total amount must be greater than zero");
            var orderDetails = await GetOrderDetailByOrderIdAsync(order.Id);
            //foreach (var orderDetail in orderDetails)
            //{
            //    order.TotalAmount += orderDetail.PricePerItem * orderDetail.Quantity;
            //}
            Orders newOrder = new Orders()
            {
                UserId = order.UserId,
                Status = Enums.OrderStatus.Pending,
                TotalAmount = 0,
                OrderDate = DateTime.UtcNow
            };
            await _ordersRepository.CreateAsync(newOrder);
            await _ordersRepository.SaveChangesAsync();
        }

        public async Task CancelOrderAsync(int id,int userId)
        {
            Orders order=null;
            foreach (var item in await _ordersRepository.GetAllAsync())
            {
               
                if (item.UserId == userId && item.Id == id)
                     order = item;
            }
            if (order == null) throw new NotFoundException("order not found");
            if (order.Status == Enums.OrderStatus.Cancelled) throw new OrderAlreadyCancelledException("Order already cancelled");
            order.Status = Enums.OrderStatus.Cancelled;
            foreach (var item in await _orderDetailsRepository.GetAllAsync())
            {
                if (item.OrderId == id)
                {
                    foreach(var item2 in await _productsRepository.GetAllAsync())
                    {
                        if (item2.Id == item.ProductId) item2.Stock += item.Quantity;
                        _productsRepository.Update(item2);
                    }
                }
            }

            await _ordersRepository.SaveChangesAsync();
            await _productsRepository.SaveChangesAsync();
        }
        public async Task CompleteOrderAsync(int id,int userId)
        {
            Orders order=null;
            foreach (var item in await _ordersRepository.GetAllAsync())
            {

                if (item.UserId == userId && item.Id == id)
                    order = item;
            }

            if (order == null) throw new NotFoundException("order not found");
            if (order.Status == Enums.OrderStatus.Completed) throw new OrderAlreadyCompletedException("Order already completed");
            //foreach (var orderDetail in orderDetails)
            //{
            //    order.TotalAmount+= orderDetail.PricePerItem*orderDetail.Quantity;
            //}
            var payment = await _paymentsRepository.GetAsync(x => x.OrderId == id);
            if (payment == null) throw new NotFoundException("Odenis tapilmadi");
            order.Status = Enums.OrderStatus.Completed;
            await _ordersRepository.SaveChangesAsync();
        }

        public async Task<List<OrderGetDto>> GetAllOrdersAsync()
        {
            var orders = await _ordersRepository.GetAllAsync("OrderDetails","User");
            List<OrderGetDto> result = new List<OrderGetDto>();
            foreach (var order in orders)
            {
                OrderGetDto dto = new OrderGetDto()
                {
                    Id = order.Id,
                    Status = order.Status,
                    TotalAmount = order.TotalAmount,
                    OrderDate = order.OrderDate,
                    User = order.User,
                    UserId = order.UserId,
                    OrderDetails = order.OrderDetails
                };
                result.Add(dto);
            }
            return result;
        }

        public async Task<OrderGetDto> GetOrderAsync(int id)
        {
            var order = await _getOrderById(id);
            OrderGetDto dto = new OrderGetDto()
            {
                Id = order.Id,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                OrderDate = order.OrderDate,
                User = order.User,
                UserId = order.UserId,
                OrderDetails = order.OrderDetails,

            };
            return dto;
        }

        public async Task UpdateOrderAsync(OrderPutDto order)
        {
            var dbOrder = await _getOrderById(order.Id);
            bool userExists = await _usersRepository.IsExistAsync(u => u.Id == order.UserId);
            if (!userExists) { throw new NotFoundException("User tapilmadi"); }
            if (order.TotalAmount < 0) throw new InvalidOrderException("total amount must be greater than zero");
            dbOrder.UserId = order.UserId;
            dbOrder.Status = order.Status;
            dbOrder.TotalAmount = order.TotalAmount;

            _ordersRepository.Update(dbOrder);
            await _ordersRepository.SaveChangesAsync();
        }
        private async Task<Orders> _getOrderById(int id)
        {
            var orders = await _ordersRepository.GetAsync(x => x.Id == id);

            if (orders is null)
                throw new NotFoundException("Order is not found");


            return orders;
        }

        public async Task AddOrderDetailAsync(OrderDetailsPostDto orderDetail)
        {
            var order = await _ordersRepository.GetAsync(x => x.Id == orderDetail.OrderId);
            if (order==null) throw new NotFoundException("Order not found");

            //var productExists = await _productsRepository.IsExistAsync(x => x.Id == orderDetail.ProductId);
            //if (!productExists) throw new NotFoundException("Product not found");
            var product = await _productsRepository.GetAsync(x=>x.Id == orderDetail.ProductId);
            if (product == null) throw new NotFoundException("product not found");
            if (orderDetail.Quantity <= 0) throw new InvalidOrderDetailException("Quantity must be greater than zero");
            if (product.Stock - orderDetail.Quantity < 0) throw new InvalidOrderDetailException("order quantity cannot be higher product than stock");
            product.Stock-=orderDetail.Quantity;
             _productsRepository.Update(product);
            OrderDetails newOrderDetail = new OrderDetails
            {
                OrderId = orderDetail.OrderId,
                ProductId = orderDetail.ProductId,
                Quantity = orderDetail.Quantity,
                PricePerItem = product.Price
            };
            order.TotalAmount += newOrderDetail.Quantity * newOrderDetail.PricePerItem;
            _ordersRepository.Update(order);
            await _orderDetailsRepository.CreateAsync(newOrderDetail);
            await _orderDetailsRepository.SaveChangesAsync();
            await _productsRepository.SaveChangesAsync();
        }

        public async Task<List<OrderDetailsGetDto>> GetOrderDetailByOrderIdAsync(int orderId)
        {
            var details  = await _orderDetailsRepository.GetAllAsync();
            if (details == null) throw new NotFoundException("order detail not found");
            List<OrderDetailsGetDto> dtos = new List<OrderDetailsGetDto>();
            foreach(var item in details)
            {
                if (item.OrderId == orderId)
                {
                    OrderDetailsGetDto dto = new OrderDetailsGetDto()
                    {
                        OrderId = item.OrderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Id = item.Id,
                        Order = item.Order,
                        PricePerItem = item.PricePerItem,
                        Product = item.Product
                    };
                    dtos.Add(dto);
                }

            }
            return dtos;

        }
    }
}
