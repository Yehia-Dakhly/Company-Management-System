using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        //IEnumerable // Get All Employees And Filter them in my app here
        IQueryable<Employee> GetEmployeesByAddress(string address); // Filter in DB
        IQueryable<Employee> GetEmployeesByName(string name); // Filter in DB

    }
}
