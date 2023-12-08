using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Domain.Dtos
{
    public class DepartmentDto
    {
        [Required]
        [MaxLength(50)]
        public string DepartmentName { get; set; }
    }
}
