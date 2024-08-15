using ORM_MiniProject.DTOs.OrderDetailDTOs;
using ORM_MiniProject.DTOs.OrderDTOs;
using ORM_MiniProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_MiniProject.Services.Interfaces
{
    public interface IOrderDetailsService
    {
        Task<List<OrderDetailsGetDto>> GetAllOrderDetailsAsync();
        Task<OrderDetailsGetDto> GetOrderDetailAsync(int id);
        Task UpdateOrderDetailAsync(OrderDetailsPutDto orderDetail);
        //Task CreateOrderDetailAsync(OrderDetailsPostDto orderDetail);
        //taska uygun olmasi ucun orderservice classinda yazdim
        Task DeleteOrderDetailAsync(int id);
    }
}
