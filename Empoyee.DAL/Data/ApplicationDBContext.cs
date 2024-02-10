using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
namespace Employee.DAL.Data
{
    public class ApplicationDBContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public ApplicationDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:DefaltConnectigString"];

        }
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
