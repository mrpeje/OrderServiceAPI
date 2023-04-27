namespace OrdersService.Models
{
    public enum OrderStatus
    {
        New = 0,
        Paid = 1,
        InDelivery = 2,
        Delivered = 3,
        Completed = 4
    }
    public enum OperationStatus
    {
        Success = 0,
        Error = 1,
        NotFound = 2
    }
}
