using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.OrderOperations.Queries.TrackOrder
{
    public class TrackOrderQuery : IRequest<TrackOrderQueryResponse>
    {
        public Guid OrderId { get; set; }
    }
}
