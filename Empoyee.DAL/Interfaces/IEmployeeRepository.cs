

using Employee.Domain.Models;

namespace Employee.DAL.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<int> CreateEmployeeAsync(string companyName, EmployeeModel employeeModel);
        Task<bool> EmployeeDelete(int id);
        Task<IEnumerable<EmployeeModel>> GetEmployeesSpecificCompanyAsync(string companyName);
        Task<IEnumerable<EmployeeModel>> GetEmployeesSpecificDepartmentAsync(string companyName, string departmentName);
        Task<bool> UpdateEmployeeAsync(int id, EmployeeModel employeeModel);
        Task<EmployeeModel> FindEmployeeById(int id);
    }
}
