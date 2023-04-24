using CompanyManagement.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyManagement.Repository.General
{
    public class UnitOfWork : CompanyMNGEntities
    {
        public UnitOfWork() 
        {
            _context = new CompanyMNGEntities();
        }

        #region Properties

        private readonly CompanyMNGEntities _context;

        public CompanyMNGEntities Context
        {
            get { return _context; }
        }
        
        #endregion
    }
}