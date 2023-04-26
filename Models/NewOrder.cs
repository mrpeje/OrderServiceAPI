using OrdersService.Context;

namespace OrdersService.Models
{
    public class NewOrder
    {
        public Guid Id { get; set; }
        public List<OrderLineModel> Lines { get; set; } // ? nullable
    }
}
