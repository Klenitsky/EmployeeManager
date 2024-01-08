using EmployeeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeService.DAL.Repositories.Interfaces
{
    public interface IOfficeRepo: IRepo<Office>
    {

        public IEnumerable<Office> GetAllByCountry(int countryId);
    }
}
