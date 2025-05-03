using Application.Ecom.Interfaces;
using Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.OrderOperations.Commands.ReturnOrder
{
    public class ReturnOrderCommandHandler : IRequestHandler<ReturnOrderCommand, ReturnOrderResponse>
    {

        private readonly IEcommerceDbContext _context;
        private readonly ILoggerManager _logger;
        public ReturnOrderCommandHandler(IEcommerceDbContext dbContext,ILoggerManager logger) 
        {
            _context = dbContext;
            _logger = logger;
        }

        public async Task<ReturnOrderResponse> Handle(ReturnOrderCommand request, CancellationToken cancellationToken)
        {
            var returnorder = await _context.Orders.Where(o=>!o.IsCancelled)
                                              .Include(p=>p.Payments)
                                              .Include(oi=>oi.OrderItems)
                                              .ThenInclude(p=>p.Product)
                                              .FirstOrDefaultAsync(o=>o.OrderId==request.OrderId);

           
            
            if (returnorder == null)
            {
                _logger.LogWarn($"Order No: {request.OrderId} Not Found");
                return new ReturnOrderResponse
                {
                    Message= $"Order No: {request.OrderId} could not be Found",
                    Success=false
                };
            }
            returnorder.OrderStatus = "Return Initiaited";
            
            Return NewReturn = new Return
            {
                ReturnId = new Guid(),
                Reason = request.Reason,
                OrderId = returnorder.OrderId,
                PickupDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(2),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            returnorder.ReturnId = NewReturn.ReturnId;

            _context.Returns.Add(NewReturn);
            
            await _context.SaveChangesAsync();
            

            return new ReturnOrderResponse
            {
                Success=true,
                Message = $"Order Return Initiated for Order : {returnorder.OrderId}"
            };
        }
    }
}
