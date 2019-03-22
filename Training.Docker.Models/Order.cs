using System;
using System.Collections.Generic;

namespace Training.Docker.Models
{
    public class Order
    {
        public DateTime OrderPlacementDate { get; set; }

        public List<OrderDetails> OrderDetails { get; set; }
    }
}
