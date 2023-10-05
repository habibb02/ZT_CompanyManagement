using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagement.Repository.General
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : IContext
    {
        TContext Context { get; }

        int Save();
    }
}
