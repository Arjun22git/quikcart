using Application.Ecom.Dtos;
using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Ecom.OrderOperations.Queries.TrackOrder
{
    public class TrackOrderQueryResponse
    {
        [JsonIgnore]
        public bool Success { get; set; }
        public string Message { get; set; }
        public OrderDto Order { get; set; }

    }

}
