using System;

namespace Training.Docker.Models
{
    public class CustomerOrderAdded
    {
        public Guid RequestId { get; set; }

        public string CustomerOrderId { get; set; }
    }
}
