using CompanyManagement.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace CompanyManagement.Repository.General
{
    public class UnitOfWork : IUnitOfWork<CompanyMNGEntities>
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

        #region Public Methods

        public int Save()
        {
            int returnSave = 0;
            string error = string.Empty;
            try
            {
                returnSave = _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    error += string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        error += string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw new Exception(error);
            }

            return returnSave;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        #endregion
    }
}