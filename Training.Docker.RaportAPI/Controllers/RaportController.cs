using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Training.Docker.RaportAPI.Data;

namespace Training.Docker.RaportAPI.Controllers
{
    [ApiController]

    [Route("api/Raport")]
    public class RaportController : ControllerBase
    {
        readonly ApiContext context;
        public RaportController(ApiContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<OrdersAggregatedData>> Get()
        {
            return context.OrdersAggregatedData;
        }


        [HttpGet("{customer}")]
        [Route("ByCustomer/{customer}")]
        public ActionResult<IEnumerable<OrdersAggregatedData>> GetByCustomer(string customer)
        {
            return context.OrdersAggregatedData
                 .Where(e => e.CustomerName == customer)
                 .GroupBy(e => e.CustomerName, (c, v) => new OrdersAggregatedData()
                 {
                     Id = 0,
                     CustomerName = c,
                     OrderPlacementDate = v.Max(e => e.OrderPlacementDate),
                     TotalPrice = v.Sum(e => e.TotalPrice)
                 }
               ).ToList();
        }

        [HttpGet("{date}")]
        [Route("ByDate/{date}")]
        public ActionResult<IEnumerable<OrdersAggregatedData>> Get(DateTime date)
        {
            return context.OrdersAggregatedData
                 .Where(e => e.OrderPlacementDate.ToShortDateString() == date.ToShortDateString())
                 .GroupBy(e => e.Date, (d, v) => new OrdersAggregatedData()
                 {
                     Id = 0,
                     CustomerName = "All",
                     OrderPlacementDate = d,
                     TotalPrice = v.Sum(e => e.TotalPrice)
                 }
               ).ToList();
        }
    }
}
