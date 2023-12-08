using AutoMapper;
using EmployeeManagementSystem.Data.Context;
using EmployeeManagementSystem.Domain.Dtos;
using EmployeeManagementSystem.Domain.Helper;
using EmployeeManagementSystem.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EMSDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for the DepartmentController.
        /// </summary>
        /// <param name="context">The database context for the application.</param>
        public EmployeeController(EMSDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a list of all employees.
        /// </summary>
        /// <returns>An IActionResult containing ApiResponseDto<IEnumerable<Employee>>.</returns>
        [HttpGet("Employee-List")]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var employees = await _context.Employees.ToListAsync();

                return Ok(new ApiResponse<IEnumerable<Employee>>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Employee list retrieved successfully.",
                    Data = employees
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Internal Server Error: {ex.Message}",
                });
            }
        }


        /// <summary>
        /// Adds a new employee to the database.
        /// </summary>
        /// <param name="employeeDto">The data transfer object containing employee information.</param>
        /// <returns>An IActionResult containing ApiResponseDto<Employee>.</returns>
        [HttpPost("Add-Employee")]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeDto employeeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid request."
                    });
                }

                var employee = _mapper.Map<Employee>(employeeDto);

                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();

                // Explicitly load the related Department data
                await _context.Entry(employee).Reference(e => e.Department).LoadAsync();

                return Ok(new ApiResponse<Employee>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Employee added successfully.",
                    Data = employee
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Internal Server Error: {ex.Message}",
                });
            }
        }


        /// <summary>
        /// Edits an existing employee in the database.
        /// </summary>
        /// <param name="id">The ID of the employee to be edited.</param>
        /// <param name="employeeDto">The updated employee information.</param>
        /// <returns>An IActionResult containing ApiResponseDto<Employee>.</returns>
        [HttpPut("Edit-Employee/{id}")]
        public async Task<IActionResult> EditEmployee([FromRoute] int id, [FromBody] EmployeeDto employeeDto)
        {
            try
            {
                var existingEmployee = await _context.Employees.FindAsync(id);

                if (existingEmployee == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Employee not found"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid request."
                    });
                }

                _mapper.Map(employeeDto, existingEmployee);

                await _context.SaveChangesAsync();

                // Explicitly load the related Department data
                await _context.Entry(existingEmployee).Reference(e => e.Department).LoadAsync();

                return Ok(new ApiResponse<Employee>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Employee updated successfully.",
                    Data = existingEmployee
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Internal Server Error: {ex.Message}",
                });
            }
        }


        /// <summary>
        /// Deletes an existing employee from the database.
        /// </summary>
        /// <param name="id">The ID of the employee to be deleted.</param>
        /// <returns>An IActionResult containing ApiResponseDto<object>.</returns>
        [HttpDelete("Delete-Employee/{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);

                if (employee == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Employee not found"
                    });
                }

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Employee deleted successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Internal Server Error: {ex.Message}",
                });
            }
        }
    }
}
