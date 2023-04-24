using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CompanyManagement.DAL;
using CompanyManagement.Repository.General;

namespace CompanyManagement.Repository
{
    public class SupplierRepository : Repository<Supplier>, IESupplierRepository
    {
        private readonly CompanyMNGEntities _context;
        //public SupplierRepository()
        //{
        //    _context = new CompanyMNGEntities();
        //}
        public SupplierRepository(CompanyMNGEntities context) : base(context)
        {
            _context = context;
        }
        public void Delete(int IdSupplier)
        {
            Supplier supplier = _context.Supplier.Find(IdSupplier);
            _context.Supplier.Remove(supplier);
        }

        //public IEnumerable<Supplier> GetAll()
        //{
        //    return _context.Supplier.ToList();
        //}

        public Supplier GetByID(int IdSupplier)
        {
            return _context.Supplier.Find(IdSupplier);
        }
        
        public IEnumerable<Supplier> GetByName(string SupplierName)
        {
            return _context.Supplier.Where( c => c.Name.Contains(SupplierName)).ToList();
        }
        
        //public void Insert(Supplier supplier)
        //{
        //    _context.Supplier.Add(supplier);
        //}

        //public void Save()
        //{
        //    _context.SaveChanges();
        //}

        //public void Update(Supplier supplier)
        //{
        //    _context.Entry(supplier).State = EntityState.Modified;
        //}
    }
}