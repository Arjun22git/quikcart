using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.Authentication.Queries.ViewCustomerbyEmail
{
    public class ViewCustomerQueryResponse
    {
        public Guid? CustomerId { get; set; }  
        public string CustomerName { get; set; }
        public DateTime? JoinDate { get; set; } 
        public bool IsVerified { get; set; }
        
    }
}
