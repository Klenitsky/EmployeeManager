﻿using EmployeeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeService.DAL.Repositories.Interfaces
{
    public interface IEmployeeRepo:IRepo<Employee>
    {
        IEnumerable<Employee> GetAllByOffice(int officeId);
    }
}
