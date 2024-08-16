using ORM_MiniProject.DTOs.OrderDetailDTOs;
using ORM_MiniProject.DTOs.OrderDTOs;
using ORM_MiniProject.DTOs.UserDTOs;

namespace ORM_MiniProject.Services.Interfaces
{
    public interface IOrdersService
    {
        Task<List<OrderGetDto>> GetAllOrdersAsync();
        Task<OrderGetDto> GetOrderAsync(int id);
        Task UpdateOrderAsync(OrderPutDto order);
        Task CreateOrderAsync(OrderPostDto order);
        Task CancelOrderAsync(int id,int userId);
        Task CompleteOrderAsync(int id,int userId);
        Task AddOrderDetailAsync(OrderDetailsPostDto detailsPostDto);
        Task<List<OrderDetailsGetDto>> GetOrderDetailByOrderIdAsync(int orderId);
    }
}
