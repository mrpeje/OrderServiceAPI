namespace OrdersService.Context
{
    public class OrderLine : IEquatable<OrderLine>
    {
        public Guid Id { get; set; }
        public uint qty { get; set; }
        public Guid OrderId { get; set; }

        public bool Equals(OrderLine? other)
        {
            if(other.Id == Id && other.qty == qty)
                return true;
            return false;
        }
    }
}