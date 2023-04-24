using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CompanyManagement.DAL;
using CompanyManagement.Repository.General;

namespace CompanyManagement.Repository
{
    public class OrderProductRepository : Repository<OrderProduct>, IEOrderProductRepository
    {
        private readonly CompanyMNGEntities _context;
        //public OrderProductRepository()
        //{
        //    _context = new CompanyMNGEntities();
        //}
        public OrderProductRepository(CompanyMNGEntities context) : base(context)
        {
            _context = context;
        }

        //public IEnumerable<OrderProduct> GetAll()
        //{
        //    return _context.OrderProduct.ToList();
        //}

        public OrderProduct GetById(int IdOrder)
        {
            return _context.OrderProduct.Find(IdOrder);
        }

        public void Insert(OrderProduct orderProduct)
        {
            _context.OrderProduct.Add(orderProduct);
        }

        //public int Save()
        //{
        //    return
        //        _context.SaveChanges();
        //}

        //public void Update(OrderProduct orderProduct)
        //{
        //    _context.Entry(orderProduct).State = EntityState.Modified;
        //}

        public void Delete(int IdOrder, int IdProduct)
        {
            OrderProduct orderProduct = _context.OrderProduct.Find(IdOrder, IdProduct);
            _context.OrderProduct.Remove(orderProduct);
        }
    }
}