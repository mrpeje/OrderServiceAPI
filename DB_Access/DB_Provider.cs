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
        public Order CreateOrder(Order order) 
        {
            var createdOrder = _context.Orders.Add(order);
            try
            {
               _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return null;
            }
            return createdOrder.Entity;
        }
        
        public OperationStatus DeleteOrder(Guid id)
        {
            try
            {
                var order = _context.Orders.FirstOrDefault(e => e.Id == id);
                if (order != null)
                {
                    _context.Orders.Remove(order);
                }
                else
                {
                    return OperationStatus.NotFound;
                }
                _context.SaveChanges();

                return OperationStatus.Success;
            }
            catch(Exception ex)
            {
                return OperationStatus.Error;
            }
            
        }

        public Order GetOrderById(Guid id)
        {           
            var order = _context.Orders.FirstOrDefault(e => e.Id == id);
            return order;
        }
       
        // Restrictions
        public Order UpdateOrder(Order order)
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
                return null;
            }
            return order;
        }
    }
}
