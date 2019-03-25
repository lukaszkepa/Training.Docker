using System;
using System.ComponentModel.DataAnnotations;

namespace Training.Docker.RaportAPI.Data
{
    public class OrdersAggregatedData
    {
        [Key]
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderPlacementDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}