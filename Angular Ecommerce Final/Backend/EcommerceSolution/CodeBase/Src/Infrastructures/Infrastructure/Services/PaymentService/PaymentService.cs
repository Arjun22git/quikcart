using Application.Ecom.Interfaces;
using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IEcommerceDbContext _context;

        public PaymentService(IEcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentStatus> ProcessPaymentAsync(decimal amount, Guid orderId, string paymentMethod)
        {
            
            var paymentStatus = PaymentStatus.Success;

           
            var payment = new Payment
            {
                PaymentId = Guid.NewGuid(),
                OrderId = orderId,
                Amount = amount,
                PaymentMethod = paymentMethod, 
                PaymentDate = DateTime.UtcNow,
                PaymentStatus = paymentStatus,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.PaymentId = payment.PaymentId;
                order.IsPaid = (paymentStatus == PaymentStatus.Success);
                order.OrderStatus = (paymentStatus == PaymentStatus.Success) ? "Processed" : "Payment Failed";
                _context.Orders.Update(order);
            }

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            

            return paymentStatus;
        }
    }
}
