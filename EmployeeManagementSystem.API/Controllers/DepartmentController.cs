using EmployeeManagementSystem.Data.Context;
using EmployeeManagementSystem.Domain.Dtos;
using EmployeeManagementSystem.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace EmployeeManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly EMSDbContext _context;

        /// <summary>
        /// Constructor for the DepartmentController.
        /// </summary>
        /// <param name="context">The database context for the application.</param>
        public DepartmentController(EMSDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Retrieves the list of departments from the database.
        /// </summary>
        /// <returns>
        /// An IActionResult containing ApiResponseDto<IEnumerable<Department>>.
        /// The ApiResponseDto includes the status code, a descriptive message, and the retrieved list of departments.
        /// </returns>
        [HttpGet("Department-List")]
        public async Task<IActionResult> GetDepartments()
        {
            try
            {
                var departments = await _context.Departments.ToListAsync();
                return Ok(new ApiResponseDto<IEnumerable<Department>>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Department list retrieved successfully.",
                    Data = departments
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseDto<object>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Internal Server Error: {ex.Message}",
                });
            }
        }


        /// <summary>
        /// Adds a new department to the database.
        /// </summary>
        /// <param name="departmentDto">The data transfer object containing department information.</param>
        /// <returns>An IActionResult containing ApiResponseDto<Department>.</returns>
        /// <remarks>
        /// This method checks the validity of the provided data using ModelState.
        /// If the data is valid, it creates a new Department entity based on the provided information.
        /// The new department is then added to the database asynchronously, and a successful response
        /// is returned along with ApiResponseDto containing the added department.
        /// In case of an exception, a 500 Internal Server Error response is returned with an error message.
        /// </remarks>
        [HttpPost("Add-Department")]
        public async Task<IActionResult> AddDepartment([FromBody] DepartmentDto departmentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseDto<object>
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid request."
                    });
                }

                var department = new Department
                {
                    DepartmentName = departmentDto.DepartmentName
                };
                await _context.Departments.AddAsync(department);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponseDto<Department>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Department added successfully.",
                    Data = department
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseDto<object>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Internal Server Error: {ex.Message}",
                });
            }
        }


        /// <summary>
        /// Edits an existing department in the database.
        /// </summary>
        /// <param name="id">Department ID.</param>
        /// <param name="departmentDto">Updated department information.</param>
        /// <returns>ApiResponseDto<Department>.</returns>
        /// <remarks>
        /// Updates department properties based on the provided data.
        /// 404 if department not found, 400 for invalid data.
        /// 200 with ApiResponseDto on successful update.
        /// 500 Internal Server Error in case of an exception.
        /// </remarks>
        [HttpPut("Edit-Department/{id}")]
        public async Task<IActionResult> EditDepartment([FromRoute] int id, [FromBody] DepartmentDto departmentDto)
        {
            try
            {
                var existingDepartment = await _context.Departments.FindAsync(id);

                if (existingDepartment == null)
                {
                    return NotFound(new ApiResponseDto<object>
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Department not found"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseDto<object>
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid request."
                    });
                }

                existingDepartment.DepartmentName = departmentDto.DepartmentName;
                await _context.SaveChangesAsync();

                return Ok(new ApiResponseDto<Department>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Department updated successfully.",
                    Data = existingDepartment
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseDto<object>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Internal Server Error: {ex.Message}",
                });
            }
        }


        /// <summary>
        /// Deletes an existing department from the database.
        /// </summary>
        /// <param name="id">Department ID.</param>
        /// <returns>ApiResponseDto<object>.</returns>
        /// <remarks>
        /// 404 Not Found if the department doesn't exist.
        /// 200 OK with ApiResponseDto on successful deletion.
        /// 500 Internal Server Error in case of an exception.
        /// </remarks>
        [HttpDelete("Delete-Department/{id}")]
        public async Task<IActionResult> DeleteDepartment([FromRoute] int id)
        {
            try
            {
                var department = await _context.Departments.FindAsync(id);
                if (department == null)
                {
                    return NotFound(new ApiResponseDto<object>
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Department not found"
                    });
                }

                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponseDto<object>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Department deleted successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseDto<object>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Internal Server Error: {ex.Message}",
                });
            }
        }
    }
}
