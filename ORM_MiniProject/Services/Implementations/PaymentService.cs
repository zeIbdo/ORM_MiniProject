using ORM_MiniProject.DTOs.PaymentDTOs;
using ORM_MiniProject.Exceptions;
using ORM_MiniProject.Models;
using ORM_MiniProject.Repositories.Implementations;
using ORM_MiniProject.Repositories.Interfaces;
using ORM_MiniProject.Services.Interfaces;

namespace ORM_MiniProject.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentsRepository _paymentsRepository;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        public PaymentService()
        {
            _paymentsRepository = new PaymentsRepository();
            _ordersRepository = new OrdersRepository();
            _orderDetailsRepository = new OrderDetailsRepository();
        }
        public async Task MakePaymentAsync(PaymentPostDto payment,int userId)
        {
            var order = await _ordersRepository.GetAsync(x => x.Id == payment.OrderId&&x.UserId==userId, "OrderDetails");

            if (order == null) throw new NotFoundException("Order tapilmadi");

            foreach (var item in order.OrderDetails)
            {
                payment.Amount += item.Quantity * item.PricePerItem;
            }
            if (order.OrderDetails == null || !order.OrderDetails.Any())
                throw new InvalidPaymentException("Cannot make a payment for an order with no order details");
            Payments newPayment = new Payments()
            {
                PaymentDate = DateTime.UtcNow,
                Amount = payment.Amount,
                OrderId = payment.OrderId
            };
            await _paymentsRepository.CreateAsync(newPayment);
            await _paymentsRepository.SaveChangesAsync();
        }

        public async Task DeletePaymentAsync(int id)
        {
            var payment = await _getPaymentById(id);
            _paymentsRepository.Delete(payment);
            await _paymentsRepository.SaveChangesAsync();
        }

        public async Task<List<PaymentGetDto>> GetAllPaymentsAsync(int userId)
        {
            var payments = await _paymentsRepository.GetAllAsync("Order");
            List<PaymentGetDto> result = new List<PaymentGetDto>();
            foreach (var payment in payments)
            {
                if (payment.Order.UserId == userId)
                {
                    PaymentGetDto dto = new PaymentGetDto()
                    {
                        Id = payment.Id,
                        Amount = payment.Amount,
                        Order = payment.Order,
                        OrderId = payment.OrderId,
                        PaymentDate = payment.PaymentDate
                    };
                    result.Add(dto);
                }
            }
            return result;
        }

        public async Task<PaymentGetDto> GetPaymentsAsync(int id)
        {
            var payment = await _getPaymentById(id);
            PaymentGetDto dto = new PaymentGetDto()
            {
                Id = payment.Id,
                Amount = payment.Amount,
                Order = payment.Order,
                OrderId = payment.OrderId,
                PaymentDate = payment.PaymentDate
            };
            return dto;
        }

        public async Task UpdatePaymentAsync(PaymentPutDto payment)
        {
            var dbPayment = await _getPaymentById(payment.Id);
            var orderExists = await _ordersRepository.IsExistAsync(x => x.Id == payment.OrderId);
            if (!orderExists) throw new NotFoundException("Order tapilmadi");
            if (payment.Amount < 0) throw new InvalidPaymentException("Amount 0 dan kicik ola bilmez");

            dbPayment.Amount = payment.Amount;
            dbPayment.OrderId = payment.OrderId;

            _paymentsRepository.Update(dbPayment);
            await _paymentsRepository.SaveChangesAsync();
        }
        private async Task<Payments> _getPaymentById(int id)
        {
            var payment = await _paymentsRepository.GetAsync(x => x.Id == id);

            if (payment is null)
                throw new NotFoundException("Payment is not found");


            return payment;
        }
    }
}
