using Employee.Domain.Interfaces;

namespace Employee.Domain.Responses
{
    public class DataResponse<T> : IDataResponse<T>
    {
        public int StatusCode { get; set; }
        public string Description { get; set; }
        public T Data { get; set; }
    }
}
