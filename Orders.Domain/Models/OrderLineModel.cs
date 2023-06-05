namespace Orders.Domain.Models
{
    public class OrderLineModel
    {
        public Guid Id { get; set; }
        public uint qty { get; set; }
    }
}