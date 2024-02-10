using Employee.Domain.Interfaces;
using Employee.Domain.Responses;
namespace Employee.Services.Services
{
    public static class FactoryService<T>
    {
        public static IDataResponse<T> CreateDataResponse()
        {
            return new DataResponse<T>();
        }
    }
}
