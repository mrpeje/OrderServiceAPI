using Microsoft.EntityFrameworkCore;

namespace OrdersService.Context
{
    public class OrdersServiceContext : DbContext
    {

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public OrdersServiceContext(DbContextOptions<OrdersServiceContext> options)
             : base(options)
        {
        }

    }
}
