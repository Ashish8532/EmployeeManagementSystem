using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Domain.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Range(21, 100)]
        public int Age { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public decimal Salary { get; set; }
    }
}
