using System.Collections.Generic;
using CompanyManagement.DAL;
using CompanyManagement.Repository.General;

namespace CompanyManagement.Repository
{
    public interface IESupplierRepository : IRepository<Supplier>
    {
        Supplier GetByID(int IdSupplier);
        IEnumerable<Supplier> GetByName(string SupplierName);
        //void Update(Supplier supplier);

        void Delete(int IdProduct);
    }
}
