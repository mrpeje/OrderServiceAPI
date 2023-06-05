using Microsoft.EntityFrameworkCore;

namespace OrdersService.Context
{
    public class OrdersServiceContext : DbContext
    {

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=OrdersDB;Username=postgres;Password=postgres;Include Error Detail=True;");
        }

    }
}
