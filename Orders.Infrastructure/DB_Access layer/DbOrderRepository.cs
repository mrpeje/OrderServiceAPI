﻿using OrdersService.Context;
using Microsoft.EntityFrameworkCore;
using Orders.Domain;

namespace OrdersService.DB_Access
{
    public class DbOrderRepository : IOrderRepository
    {
        private readonly OrdersServiceContext _context;
        public DbOrderRepository(OrdersServiceContext context)
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
            catch (Exception ex)
            {
                return OperationStatus.Error;
            }

        }

        public Order? GetOrderById(Guid id)
        {
            return _context.Orders.AsNoTracking().Include(e => e.Lines).FirstOrDefault(e => e.Id == id);
        }

        public OperationStatus UpdateOrder(Order order)
        {
            try
            {
                var dbOrder = _context.Entry(order);
                dbOrder.State = EntityState.Modified;
                dbOrder.Property(x => x.Created).IsModified = false;
                dbOrder.Property(x => x.Id).IsModified = false;

                UpdateLines(order.Lines);

                var dbLines = GetOrderLinesByOrderId(order.Id);
                var deletedLines = FindDeletedLines(dbLines, order.Lines.ToList());
                foreach (var line in deletedLines)
                {
                    _context.OrderLines.Remove(line);
                }

                _context.SaveChanges();
                return OperationStatus.Success;
            }
            catch (Exception ex)// ?
            {
                return OperationStatus.Error;
            }
        }
        private void UpdateLines(ICollection<OrderLine> lines)
        {
            foreach(var line in lines)
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
        }
        private List<OrderLine> GetOrderLinesByOrderId(Guid id)
        {
            var lines = _context.OrderLines.AsNoTracking().Where(e => e.OrderId == id);
            return lines.ToList();
        }
        private List<OrderLine> FindDeletedLines(List<OrderLine> dbLines, List<OrderLine> requestLines)
        {
            var deletedLines = dbLines.Where(dbLine => requestLines.All(userLine => dbLine.Id != userLine.Id));
            return deletedLines.ToList();
        }
    }
}
