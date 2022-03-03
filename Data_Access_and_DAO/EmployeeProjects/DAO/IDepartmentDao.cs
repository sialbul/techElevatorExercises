using EmployeeProjects.Models;
using System.Collections.Generic;

namespace EmployeeProjects.DAO
{
    public interface IDepartmentDao
    {
        /// <summary>
        /// Gets a department from the data store that has the given id.
	    /// If the id is not found, returns null.
        /// </summary>
        /// <param name="departmentId">The department id to get from the data store.</param>
        /// <returns>A filled out Department object.</returns>
        Department GetDepartment(int departmentId);

        /// <summary>
        /// Gets all departments from the data store.
        /// </summary>
        /// <returns>All departments as Department objects in an IList.</returns>
        IList<Department> GetAllDepartments();

        /// <summary>
        /// Update a department to the data store. Only called on departments that are already in the data store.
        /// </summary>
        /// <param name="updatedDepartment">The department object to update.</param>
        void UpdateDepartment(Department updatedDepartment);
    }
}
