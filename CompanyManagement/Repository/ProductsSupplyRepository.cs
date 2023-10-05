using CompanyManagement.DAL;
using CompanyManagement.Repository.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyManagement.Repository
{
    public class ProductsSupplyRepository : Repository<SuppliedProduct>
    {
        public ProductsSupplyRepository(UnitOfWork uow) : base(uow)
        {
        }
    }
}