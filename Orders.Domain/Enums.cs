﻿namespace Orders.Domain
{
    public enum OperationStatus
    {
        Success = 200,
        Error = 400,
        NotFound = 404
    }
    public enum OrderStatus
    {
        New = 0,
        Paid = 1,
        InDelivery = 2,
        Delivered = 3,
        Completed = 4
    }
}
