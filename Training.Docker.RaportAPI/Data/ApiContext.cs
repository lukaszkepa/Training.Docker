using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Training.Docker.RaportAPI.Data
{
    public class ApiContext: DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options): base(options)
        {
        }

        public DbSet<OrdersAggregatedData> OrdersAggregatedData { get; set; }
    }
}
