using OrdersService.Context;

namespace OrdersService.Models
{
    public class NewOrderModel
    {
        public Guid Id { get; set; }
        public List<OrderLineModel> Lines { get; set; } // ? nullable
    }
}
