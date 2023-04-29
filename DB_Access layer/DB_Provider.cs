using OrdersService.Context;
using Microsoft.EntityFrameworkCore;
using OrdersService.Models;

namespace OrdersService.DB_Access
{
    public class DB_Provider : IDB_Provider
    {
        private readonly OrdersServiceContext _context;
        public DB_Provider(OrdersServiceContext context)
        {
            _context = context;
        }
        public OperationStatus CreateOrder(Order order)
        {
            var createdOrder = _context.Orders.Add(order);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return OperationStatus.Error;
            }
            return OperationStatus.Success;
        }

        public OperationStatus DeleteOrder(Guid id)
        {
            try
            {
                var order = _context.Orders.FirstOrDefault(e => e.Id == id);
                if (order != null)
                {
                    _context.Orders.Remove(order);
                    _context.SaveChanges();
                }
                else
                {
                    return OperationStatus.NotFound;
                }
                return OperationStatus.Success;
            }
            catch(Exception ex)
            {
                return OperationStatus.Error;
            }

        }
        List<OrderLine> GetOrderLinesByOrderId(Guid id)
        {
            var lines = _context.OrderLines.AsNoTracking().Where(e => e.OrderId == id);
            return lines.ToList();
        }
        public Order GetOrderById(Guid id)
        {
            var order = _context.Orders.AsNoTracking().Include(e=>e.Lines).FirstOrDefault(e => e.Id == id);
            return order;
        }

        public OperationStatus UpdateOrder(Order order)
        {
            try
            {
                // Update order
                var dbOrder = _context.Entry(order);
                dbOrder.State = EntityState.Modified;
                dbOrder.Property(x => x.Created).IsModified = false;
                dbOrder.Property(x => x.Id).IsModified = false;
                

                foreach(var line in order.Lines)
                {
                    bool foundLine = _context.OrderLines.Any(e => e.Id == line.Id);
                    if (foundLine)
                    {
                        _context.Attach(line);
                        _context.Entry(line).State = EntityState.Modified; 
                    }
                    else
                    {
                        _context.OrderLines.Add(line);
                    }
                }
                // Delete OrderLines that not found in request data
                var dbLines = GetOrderLinesByOrderId(order.Id);
                var deletedLines = dbLines.Where(dbLine => order.Lines.All(userLine => dbLine.Id != userLine.Id)).ToList();
                foreach (var line in deletedLines)
                {
                    _context.OrderLines.Remove(line);
                }
                _context.SaveChanges();
                return OperationStatus.Success;
            }
            catch (Exception ex)
            {
                return OperationStatus.Error;
            }           
        }               
    }
}
