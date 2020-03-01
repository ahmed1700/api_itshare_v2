using Contracts;
using Entities.Data;

namespace Repository
{
  public  class RepositroryManager : IRepositroryManager
    {
        private ICompanyRepository _companyRepository;
        private IEmployeeRepository _employeeRepository;
        private RepositryContext _repositryContext;
        public RepositroryManager(RepositryContext repositryContext) => _repositryContext = repositryContext;

        public IEmployeeRepository Employee
        {
            get
            {
                if (_employeeRepository == null) _employeeRepository = new EmployeeRepositrory(_repositryContext);
                return _employeeRepository;
            }
        }

        public ICompanyRepository Company
        {
            get
            {
                if (_companyRepository == null) _companyRepository = new CompanyRepositrory(_repositryContext);
                return _companyRepository;
            }
        }

        public void Save()=> _repositryContext.SaveChanges();
    }
}
