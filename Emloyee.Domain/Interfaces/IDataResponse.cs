using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Domain.Interfaces
{
    public interface IDataResponse<T> : IBaseResponse
    {
        public T Data { get;set;}
    }
}
