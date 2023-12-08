using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Domain.Dtos
{
    public class ApiResponseDto<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
