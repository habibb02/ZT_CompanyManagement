using System.Collections.Generic;
using System.Data;
using CompanyManagement.DAL;
using CompanyManagement.Repository.General;

namespace CompanyManagement.Repository
{
    public interface IEOrderRepository: IRepository<Order>
    {
        IEnumerable<Order> GetOrdersBySupplierId(int IdSupplier);

        IEnumerable<Order> GetOrdersByIdName(string IdOrder, string SupplierName);

        Order GetById(int IdOrder);

        new int Save();

        void Delete(int IdOrder);
    }
}
