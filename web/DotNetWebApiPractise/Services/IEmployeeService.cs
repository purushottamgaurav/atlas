using DotNetWebApiPractise.Data;
using DotNetWebApiPractise.Modals;

namespace DotNetWebApiPractise.Services
{
    public interface IEmployeeService
    {
        List<Employee> GetAllEmployees();
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _dbContext;
        public EmployeeService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Employee> GetAllEmployees()
        {
            var employees = _dbContext.Employees.ToList();

            return employees;
        }


    }
}
