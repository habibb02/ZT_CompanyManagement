using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CompanyManagement.DAL;
using CompanyManagement.Repository.General;

namespace CompanyManagement.Repository
{
    public class OrderRepository : Repository<Order>, IEOrderRepository
    {
        private readonly CompanyMNGEntities _context;
        //public OrderRepository() : base(context)
        //{
        //    _context = new CompanyMNGEntities();
        //}
        public OrderRepository(CompanyMNGEntities context) : base(context)
        {
            _context = context;
        }
        public void Delete(int IdOrder)
        {
            Order order = _context.Order.Find(IdOrder);
            _context.Order.Remove(order);
        }

        //public IEnumerable<Order> GetAll()
        //{
        //    return _context.Order.ToList();
        //}
        
        public IEnumerable<Order> GetOrdersBySupplierId(int IdSupplier)
        {
            return
                _context.Order
                .Where(o => 
                o.Supplier.IdSupplier == IdSupplier
                ).ToList();
        }
        
        public IEnumerable<Order> GetOrdersByIdName(string idOrderPk, string supplierName)
        {
            return
                _context.Order
                .Where(o =>
                    (string.IsNullOrEmpty(idOrderPk) || o.IdOrder.ToString().Contains(idOrderPk))
                    && (string.IsNullOrEmpty(supplierName) || o.Supplier.Name.ToLower().Contains(supplierName.ToLower()))
                ).ToList();
        }
        
        public Order GetById(int IdOrder)
        {
            return _context.Order.Include(nameof(Order.OrderProduct)).FirstOrDefault(o => o.IdOrder == IdOrder);
        }

        public void Insert(Order order)
        {
            _context.Order.Add(order);
        }

        public new int Save()
        {
            return _context.SaveChanges();
        }

        //public void Update(Order order)
        //{
        //    _context.Entry(order).State = EntityState.Modified;
        //}
    }
}