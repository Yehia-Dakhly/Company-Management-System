using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly MvcDemoDbContext _dbContext;

        public EmployeeRepository(MvcDemoDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Employee> GetEmployeesByAddress(string address)
        => _dbContext.Employees.Where(e => e.Address == address);

        public IQueryable<Employee> GetEmployeesByName(string name)
        {
            return _dbContext.Employees.Where(E=> E.Name.ToLower().Contains(name.ToLower()));
        }
    }
}
