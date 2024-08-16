using ORM_MiniProject.DTOs.OrderDTOs;
using ORM_MiniProject.DTOs.PaymentDTOs;

namespace ORM_MiniProject.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<List<PaymentGetDto>> GetAllPaymentsAsync(int userId);
        Task<PaymentGetDto> GetPaymentsAsync(int id);
        Task UpdatePaymentAsync(PaymentPutDto payment);
        Task MakePaymentAsync(PaymentPostDto payment,int userId);
        Task DeletePaymentAsync(int id);
    }
}
