
using OrdersService.Models;

namespace OrdersService.Context
{
    public class Order
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; }
    }
}
