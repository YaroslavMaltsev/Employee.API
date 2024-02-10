using Dapper;
using Employee.DAL.Data;
using Employee.DAL.Interfaces;
using Employee.Domain.Models;


namespace Employee.DAL.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDBContext _applicationDBContext;

        public EmployeeRepository(ApplicationDBContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }
        public async Task<int> CreateEmployeeAsync(string companyName, EmployeeModel employeeModel)
        {
            using (var connection = _applicationDBContext.CreateConnection())
            {

                string query = "INSERT INTO employees(Name, Surname, Phone, CompanyId, DepartmentId) " +
                    "OUTPUT INSERTED.[Id] " +
                    "VALUES ( @name, @surname, @phone, (SELECT company.Id FROM company WHERE company.Company_name = @companyName), " +
                    "(SELECT departments.Id FROM departments WHERE departments.Department_Name = @departmentsName ) ); " +
                    "DECLARE @employee_id INT; SET @employee_id = (SELECT SCOPE_IDENTITY()); " +
                    "INSERT INTO passports(Passport_Type, Passport_Number, EmployeeId) " +
                    "VALUES ( @passport_type, @passport_number, @employee_id); ";

                var requestId = await connection.QuerySingleAsync<int>(query,
                    new
                    {
                        passport_type = employeeModel.Passport.Passport_Type,
                        passport_number = employeeModel.Passport.Passport_Number,
                        name = employeeModel.Name,
                        surname = employeeModel.Surname,
                        phone = employeeModel.Phone,
                        companyName,
                        departmentsName = employeeModel.Department.Department_Name

                    });



                return requestId;
            }

        }

        public async Task<bool> EmployeeDelete(int id)
        {
            using (var connection = _applicationDBContext.CreateConnection())
            {

                string query = "DELETE employees " +
                               "WHERE employees.Id = @Id";

                await connection.ExecuteAsync(query, new { Id = id });

                return true;

            }
        }

        public async Task<IEnumerable<EmployeeModel>> GetEmployeesSpecificCompanyAsync(string companyName)
        {
            using (var connection = _applicationDBContext.CreateConnection())
            {
                string query = "SELECT employees.Id, " +
                                   "employees.Name," +
                                   " employees.Surname," +
                                   "employees.Phone, " +
                                    "employees.CompanyId," +
                                   "departments.Department_Name," +
                                   "departments.Department_Phone, " +
                                   "passports.Passport_Type," +
                                   " passports.Passport_Number" +
                                   " FROM employees " +
                                   "JOIN departments " +
                                   "ON departments.Id = employees.DepartmentId " +
                                   "JOIN company" +
                                   " ON company.Id = employees.CompanyId " +
                                   "JOIN passports" +
                                   " ON passports.EmployeeId = employees.Id   " +
                                   "WHERE company.Company_name = @companyName";

                var employees = await connection.QueryAsync<EmployeeModel, Department, Passport, EmployeeModel>(query,
                    (employee, department, passport) =>
                    {
                        employee.Department = department;
                        employee.Passport = passport;
                        return employee;
                    },
                    new { companyName },
                    splitOn: "Department_Name,Passport_Type");

                return employees;
            }
        }

        public async Task<IEnumerable<EmployeeModel>> GetEmployeesSpecificDepartmentAsync(string companyName, string departmentName)
        {
            using (var connection = _applicationDBContext.CreateConnection())
            {
                string query = "SELECT employees.Id, " +
                                    "employees.Name," +
                                    " employees.Surname," +
                                    "employees.Phone, " +
                                     "employees.CompanyId," +
                                    "departments.Department_Name," +
                                    "departments.Department_Phone, " +
                                    "passports.Passport_Type," +
                                    " passports.Passport_Number" +
                                    " FROM employees " +
                                    "JOIN departments " +
                                    "ON departments.Id = employees.DepartmentId " +
                                    "JOIN company" +
                                    " ON company.Id = employees.CompanyId " +
                                    "JOIN passports" +
                                    " ON passports.EmployeeId = employees.id " +
                                    "WHERE company.Company_name = @companyName AND " +
                                    "departments.Department_Name = @departmentName";

                var employees = await connection.QueryAsync<EmployeeModel, Department, Passport, EmployeeModel>(query,
                    (employee, department, passport) =>
                    {
                        employee.Department = department;
                        employee.Passport = passport;
                        return employee;
                    },
                    new { companyName, departmentName },
                    splitOn: "Department_Name,Passport_Type");

                return employees;

            }
        }

        public async Task<bool> UpdateEmployeeAsync(int id, EmployeeModel employeeModel)
        {

            using (var connection = _applicationDBContext.CreateConnection())
            {
                string query = "UPDATE employees " +
                                  "SET Name = @employeeName, " +
                                  "Phone = @employeePhone, " +
                                  "Surname = @employeeSurname " +
                                  "WHERE employees.Id = @Id " +

                                  "UPDATE passports " +
                                  "SET Passport_Type = @passportType, " +
                                  "Passport_Number = @passportNumber " +
                                  "FROM employees AS PT " +
                                  "JOIN passports ON passports.EmployeeId = PT.id " +
                                  "WHERE PT.Id = @Id; " +

                                  "UPDATE departments " +
                                  "SET Department_Name = @departmentName, " +
                                  "Department_Phone =  @departmentPhone " +
                                  "FROM employees AS PT " +
                                  "JOIN departments ON departments.Id = PT.DepartmentId " +
                                  "WHERE PT.Id = @Id;";

                await connection.ExecuteAsync(query, new
                {
                    employeeName = employeeModel.Name,
                    employeePhone = employeeModel.Phone,
                    employeeSurname = employeeModel.Surname,
                    passportType = employeeModel.Passport.Passport_Type,
                    passportNumber = employeeModel.Passport.Passport_Number,
                    departmentName = employeeModel.Department.Department_Name,
                    departmentPhone = employeeModel.Department.Department_Phone,
                    Id = id
                });

                return true;
            }


        }

        public async Task<EmployeeModel> FindEmployeeById(int id)
        {
            using (var connection = _applicationDBContext.CreateConnection())
            {
                string query = "SELECT employees.Id, " +
                                   "employees.Name," +
                                   " employees.Surname," +
                                   "employees.Phone, " +
                                    "employees.CompanyId," +
                                   "departments.Department_Name," +
                                   "departments.Department_Phone, " +
                                   "passports.Passport_Type," +
                                   " passports.Passport_Number" +
                                   " FROM employees " +
                                   "JOIN departments " +
                                   "ON departments.Id = employees.DepartmentId " +
                                   "JOIN company" +
                                   " ON company.Id = employees.CompanyId " +
                                   "JOIN passports" +
                                   " ON passports.EmployeeId = employees.id " +
                                   "WHERE employees.Id = @Id ";

                var employees = await connection.QueryAsync<EmployeeModel, Department, Passport, EmployeeModel>(query,
                    (employee, department, passport) =>
                    {
                        employee.Department = department;
                        employee.Passport = passport;
                        return employee;
                    },
                    new { Id = id },
                    splitOn: "Department_Name,Passport_Type");

                return employees.FirstOrDefault();
            }
        }
    }
}
