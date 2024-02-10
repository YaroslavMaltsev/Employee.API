namespace Employee.Domain.Interfaces
{
    public interface IBaseResponse
    {
        string Description { get; set; }
        int StatusCode { get; set; }
    }
}