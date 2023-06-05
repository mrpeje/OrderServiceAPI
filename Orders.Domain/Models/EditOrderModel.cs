namespace Orders.Domain.Models
{
    public class EditOrderModel
    {
        public string Status { get; set; }
        public List<OrderLineModel> Lines { get; set; }
    }
}
