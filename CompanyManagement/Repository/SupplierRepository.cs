using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CompanyManagement.DAL;
using CompanyManagement.Repository.General;

namespace CompanyManagement.Repository
{
    public class SupplierRepository : Repository<Supplier>, IESupplierRepository
    {
        public SupplierRepository(UnitOfWork uow) : base(uow)
        {
        }

        public void Delete(int IdSupplier)
        {
            Supplier supplier = _uow.Context.Supplier.Find(IdSupplier);
            _uow.Context.Supplier.Remove(supplier);
        }

        public Supplier GetByID(int IdSupplier)
        {
            return _uow.Context.Supplier.Find(IdSupplier);
        }

        public IEnumerable<Supplier> GetByName(string SupplierName)
        {
            return _uow.Context.Supplier.Where(c => c.Name.Contains(SupplierName)).ToList();
        }
    }
}