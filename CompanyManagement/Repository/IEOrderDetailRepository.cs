using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyManagement.DAL;
using CompanyManagement.Repository.General;

namespace CompanyManagement.Repository
{
    public interface IEOrderDetailRepository : IRepository<OrderDetail>
    {
        void Delete(int IdOrder, int IdProduct);
    }
}
