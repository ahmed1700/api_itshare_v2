using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IRepositroryManager
    {
        IEmployeeRepository Employee { get; }
        ICompanyRepository Company { get; }

        void Save();
    }
}
