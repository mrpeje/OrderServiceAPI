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
        public List<OrderLine> GetOrderLinesByOrderId(Guid id)
        {
            var lines = _context.OrderLines.Where(e => e.OrderId == id);
            return lines.ToList();
        }
        public Order GetOrderById(Guid id)
        {           
            var order = _context.Orders.FirstOrDefault(e => e.Id == id);           
            return order;
        }
       
        // Restrictions
        public OperationStatus UpdateOrder(Order order)
        {
            try
            {
                // Update order
                var dbOrder = _context.Entry(order);
                dbOrder.State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return OperationStatus.Error;
            }
            return OperationStatus.Success;
        }

        public OperationStatus CreateOrderLine(OrderLine orderLine)
        {
            try
            {
                _context.OrderLines.Add(orderLine);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return OperationStatus.Error;
            }
            return OperationStatus.Success;
        }
        public OperationStatus UpdateOrderLine(OrderLine orderLine)
        {
            try
            {
                _context.OrderLines.Entry(orderLine).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return OperationStatus.NotFound;
            }
            catch (Exception ex)
            {
                return OperationStatus.Error;
            }

            return OperationStatus.Success;    
        }
        public OperationStatus DeleteOrderLine(Guid id)
        {
            try
            {
                var orderLine = _context.OrderLines.FirstOrDefault(e => e.Id == id);
                if (orderLine != null)
                {
                    _context.OrderLines.Remove(orderLine);
                    _context.SaveChanges();
                }
                else
                {
                    return OperationStatus.NotFound;
                }               
                return OperationStatus.Success;
            }
            catch (Exception ex)
            {
                return OperationStatus.Error;
            }
        }
    }
}
