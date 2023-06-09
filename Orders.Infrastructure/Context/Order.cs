﻿namespace OrdersService.Context
{
    public class Order
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; }
        public ICollection<OrderLine> Lines { get; set; }
    }
}
