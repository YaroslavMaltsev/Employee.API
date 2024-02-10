using Employee.Domain.Interfaces;
using Employee.Domain.Models;

namespace Employee.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IBaseResponse> EmployeeCreateServiceAsync(string companyName, EmployeeModel employeeModel);
        Task<IBaseResponse> EmployeeDeleteServiceAsync(int id);
        Task<IBaseResponse> GetEmployeesByCompanyServiceAsync(string companyName);
        Task<IBaseResponse> GetEmployeesByDepartmentServiceAsync(string companyName, string departmentName);
    }
}