using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CompanyManagement.DAL;
using CompanyManagement.Repository.General;

namespace CompanyManagement.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IEOrderDetailRepository
    {
        public OrderDetailRepository(UnitOfWork uow) : base(uow)
        {
        }

        public OrderDetail GetById(int IdOrder)
        {
            return _uow.Context.OrderDetail.Find(IdOrder);
        }

        public void Insert(OrderDetail OrderDetail)
        {
            _uow.Context.OrderDetail.Add(OrderDetail);
        }

        public void Delete(int IdOrder, int IdProduct)
        {
            OrderDetail OrderDetail = _uow.Context.OrderDetail.Find(IdOrder, IdProduct);
            _uow.Context.OrderDetail.Remove(OrderDetail);
        }
    }
}