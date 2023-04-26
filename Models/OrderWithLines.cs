
using OrdersService.Context;

namespace OrdersService.Models
{
    public class OrderWithLines
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; }
        public List<OrderLineModel> Lines { get; set; } // ? nullable
    }
}
