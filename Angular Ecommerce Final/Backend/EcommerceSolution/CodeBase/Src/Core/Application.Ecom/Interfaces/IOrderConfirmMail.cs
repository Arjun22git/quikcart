using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.Interfaces
{
    public interface IOrderConfirmMail
    {
        Task SendConfirmEmailAsync(string email, Order order);
    }
}
