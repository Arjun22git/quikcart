using Application.Ecom.Dtos;
using Application.Ecom.Interfaces;
using Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.OrderOperations.Commands.ProcessPayment
{
    public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, PaymentDto>
    {
        private readonly IEcommerceDbContext _context;
        private readonly IPaymentService _paymentService;
        private readonly ILoggerManager _logger;
        private readonly IOrderConfirmMail confirmMail;

        public ProcessPaymentCommandHandler(IEcommerceDbContext context, IPaymentService paymentService, ILoggerManager logger, IOrderConfirmMail confirmMail)
        {
            _context = context;
            _paymentService = paymentService;
            _logger = logger;
            this.confirmMail = confirmMail;
        }

        public async Task<PaymentDto> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
        {
            // Validate the payment request
            if (request.Amount <= 0)
            {
                _logger.LogWarn("Invalid payment amount.");
                throw new ArgumentException("Invalid payment amount.");
            }

            // Retrieve the order to verify the total amount
            var order = await _context.Orders
                .Where(o => o.OrderId == request.OrderId)
                .Select(o => new { o.TotalAmount, o.OrderStatus })
                .FirstOrDefaultAsync(cancellationToken);

            if (order == null)
            {
                _logger.LogWarn($"Order with ID {request.OrderId} not found.");
                throw new ArgumentException("Order not found.");
            }

            if (request.Amount != order.TotalAmount)
            {
                _logger.LogWarn("Payment amount does not match the total amount of the order.");
                throw new ArgumentException("Payment amount must match the total amount of the order.");
            }

       
            var paymentStatus = await _paymentService.ProcessPaymentAsync(request.Amount, request.OrderId, request.PaymentMethod);

            // Save changes to the database
            await _context.SaveChangesAsync(cancellationToken);

          
            
            var currentorder = await _context.Orders
                .Include(o => o.Customer) 
                .FirstOrDefaultAsync(o => o.OrderId == request.OrderId);

            var customerEmail = currentorder?.Customer?.Email;
            

            if (customerEmail == null) 
            {
                _logger.LogWarn($"Customer not found.");
                throw new ArgumentException("Customer not found.");
            }

            await confirmMail.SendConfirmEmailAsync(customerEmail,currentorder);


            // Return the PaymentDto
            return new PaymentDto
            {
                OrderId = request.OrderId,
                Amount = request.Amount,
                PaymentMethod = request.PaymentMethod,
                PaymentDate = DateTime.UtcNow,
                PaymentStatus = paymentStatus.ToString(),
                Message = "Payment Successful. Email Confirmation is sent successfully!"
            };
        }
    }

}
