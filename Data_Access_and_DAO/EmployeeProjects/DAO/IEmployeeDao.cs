using EmployeeProjects.Models;
using System.Collections.Generic;

namespace EmployeeProjects.DAO
{
    public interface IEmployeeDao
    {
        /// <summary>
        /// Gets all employees from the data store.
        /// </summary>
        /// <returns>All the employees as Employee objects in an IList.</returns>
        IList<Employee> GetAllEmployees();

        /// <summary>
        /// Finds all employees whose names contain the search strings.
        /// Returned employees must match both first and last name search strings.
        /// If a search string is blank, ignore it. If both strings are blank, return all employees.
        /// </summary>
        /// <remarks>Be sure to use LIKE for proper search matching.</remarks>
        /// <param name="firstNameSearch">The string to search for in the first_name field, ignore if blank.</param>
        /// <param name="lastNameSearch">The string to search for in the last_name field, ignore if blank.</param>
        /// <returns>All employees whose name matches as Employee objects in an IList.</returns>
        IList<Employee> SearchEmployeesByName(string firstNameSearch, string lastNameSearch);

        /// <summary>
        /// Gets all of the employees that are on the project with the given id.
        /// </summary>
        /// <param name="projectId">The project id to get the employees from.</param>
        /// <returns>All the employees assigned to that project as Employee objects in an IList.</returns>
        IList<Employee> GetEmployeesByProjectId(int projectId);

        /// <summary>
        /// Assigns an employee to a project.
        /// </summary>
        /// <param name="projectId">The project to put the employee on.</param>
        /// <param name="employeeId">The employee to assign.</param>
        void AddEmployeeToProject(int projectId, int employeeId);

        /// <summary>
        /// Unassigns the employee from a project.
        /// </summary>
        /// <param name="projectId">The project to remove the employee from.</param>
        /// <param name="employeeId">The employee to remove.</param>
        void RemoveEmployeeFromProject(int projectId, int employeeId);

        /// <summary>
        /// Gets all of the employees that aren't assigned to any project.
        /// </summary>
        /// <returns>All the employees not on a project as Employee objects in an IList.</returns>
        IList<Employee> GetEmployeesWithoutProjects();
    }
}
