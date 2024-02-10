using Employee.DAL.Interfaces;
using Employee.Domain.DTOs;
using Employee.Domain.Interfaces;
using Employee.Domain.Models;
using Employee.Services.Interfaces;

namespace Employee.Services.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }
        public async Task<IBaseResponse> EmployeeCreateServiceAsync(string companyName, EmployeeModel employeeModel)
        {
            var dataResponse = FactoryService<CreateEmployeeDTO>.CreateDataResponse();
            try
            {

                if (companyName == null || employeeModel == null)
                {
                    dataResponse.StatusCode = 400;
                    dataResponse.Description = "check the data";
                    return dataResponse;
                }

                var createDTO = new CreateEmployeeDTO
                {
                    id = await _repository.CreateEmployeeAsync(companyName, employeeModel)
                };
                dataResponse.StatusCode = 200;
                dataResponse.Description = "Employee create";
                dataResponse.Data = createDTO;

                return dataResponse;
            }
            catch (Exception ex)
            {
                dataResponse.StatusCode = 500;
                dataResponse.Description = "Server error";
                return dataResponse;
            }
        }
        public async Task<IBaseResponse> EmployeeDeleteServiceAsync(int id)
        {
            var dataResponse = FactoryService<bool>.CreateDataResponse();
            try
            {

                if (id <= 0)
                {
                    dataResponse.StatusCode = 400;
                    dataResponse.Description = "check the data";
                    return dataResponse;
                }

                var responseDelete = await _repository.EmployeeDelete(id);

                if (!responseDelete)
                {
                    dataResponse.StatusCode = 404;
                    dataResponse.Description = "check the data";
                    return dataResponse;
                }
                dataResponse.StatusCode = 200;
                dataResponse.Description = "Employee delete";
                dataResponse.Data = responseDelete;

                return dataResponse;
            }
            catch (Exception ex)
            {
                dataResponse.StatusCode = 500;
                dataResponse.Description = "Server error";
                return dataResponse;
            }
        }

        public async Task<IBaseResponse> GetEmployeesByCompanyServiceAsync(string companyName)
        {
            var dataResponse = FactoryService<IEnumerable<EmployeeModel>>.CreateDataResponse();
            try
            {

                if (companyName == null)
                {
                    dataResponse.StatusCode = 400;
                    dataResponse.Description = "check the data";
                    return dataResponse;
                }

                var response = await _repository.GetEmployeesSpecificCompanyAsync(companyName);

                if (response.Count() == 0)
                {
                    dataResponse.StatusCode = 400;
                    dataResponse.Description = "check the data";
                    return dataResponse;
                }
                dataResponse.StatusCode = 200;
                dataResponse.Description = "Get Employees";
                dataResponse.Data = response;

                return dataResponse;
        }
            catch (Exception ex)
            {
                dataResponse.StatusCode = 500;
                dataResponse.Description = "Server error";
                return dataResponse;
            }

}
        public async Task<IBaseResponse> GetEmployeesByDepartmentServiceAsync(string companyName, string departmentName)
        {
            var dataResponse = FactoryService<IEnumerable<EmployeeModel>>.CreateDataResponse();
            try
            {

                if (companyName == null)
                {
                    dataResponse.StatusCode = 400;
                    dataResponse.Description = "check the data";
                    return dataResponse;
                }

                var response = await _repository.GetEmployeesSpecificDepartmentAsync(companyName, departmentName);

                if (response.Count() == 0)
                {
                    dataResponse.StatusCode = 400;
                    dataResponse.Description = "check the data";
                    return dataResponse;
                }
                dataResponse.StatusCode = 200;
                dataResponse.Description = "Get Employee";
                dataResponse.Data = response;

                return dataResponse;
            }
            catch (Exception ex)
            {
                dataResponse.StatusCode = 500;
                dataResponse.Description = "Server error";
                return dataResponse;
            }
        }

    }
}

