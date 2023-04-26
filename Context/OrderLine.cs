namespace OrdersService.Context
{
    public class OrderLine
    {
        public Guid Id { get; set; }
        public uint qty { get; set; }
        public Guid OrderId { get; set; }
    }
}