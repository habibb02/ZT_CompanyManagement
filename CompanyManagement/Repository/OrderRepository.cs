using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CompanyManagement.DAL;
using CompanyManagement.Repository.General;

namespace CompanyManagement.Repository
{
    public class OrderRepository : Repository<Order>, IEOrderRepository
    {
        public OrderRepository(UnitOfWork uow) : base(uow)
        {
        }
        
        public void Delete(int IdOrder)
        {
            Order order = _uow.Context.Order.Find(IdOrder);
            _uow.Context.Order.Remove(order);
        }
        
        public IEnumerable<Order> GetOrdersBySupplierId(int IdSupplier)
        {
            return
                _uow.Context.Order
                .Where(o => 
                o.Supplier.IdSupplier == IdSupplier
                ).ToList();
        }
        
        public IEnumerable<Order> GetOrdersByIdName(string idOrderPk, string supplierName)
        {
            return
                _uow.Context.Order
                .Where(o =>
                    (string.IsNullOrEmpty(idOrderPk) || o.IdOrder.ToString().Contains(idOrderPk))
                    && (string.IsNullOrEmpty(supplierName) || o.Supplier.Name.ToLower().Contains(supplierName.ToLower()))
                ).ToList();
        }
        
        public Order GetById(int IdOrder)
        {
            return _uow.Context.Order.Include(nameof(Order.OrderDetail)).FirstOrDefault(o => o.IdOrder == IdOrder);
        }

        public void Insert(Order order)
        {
            _uow.Context.Order.Add(order);
        }

        public new int Save()
        {
            return _uow.Context.SaveChanges();
        }
    }
}