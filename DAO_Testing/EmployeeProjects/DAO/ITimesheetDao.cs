using EmployeeProjects.Models;
using System.Collections.Generic;

namespace EmployeeProjects.DAO
{
    public interface ITimesheetDao
    {
        /// <summary>
        /// Gets a timesheet from the data store that has the given id.
        /// If the id is not found, returns null.
        /// </summary>
        /// <param name="timesheetId">The id of the timesheet.</param>
        /// <returns>A filled out Timesheet object</returns>
        Timesheet GetTimesheet(int timesheetId);

        /// <summary>
        /// Gets all timesheets from the data store for a given employee,
        /// ordered by timesheet_id.
        /// </summary>
        /// <param name="employeeId">The id of the employee.</param>
        /// <returns>All timesheets as Timesheet objects in an IList.</returns>
        IList<Timesheet> GetTimesheetsByEmployeeId(int employeeId);

        /// <summary>
        /// Gets all timesheets from the data store for a given project,
        /// ordered by timesheet_id.
        /// </summary>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>All timesheets as Timesheet objects in an IList.</returns>
        IList<Timesheet> GetTimesheetsByProjectId(int projectId);

        /// <summary>
        /// Adds a new timesheet into the data store.
        /// </summary>
        /// <param name="newTimesheet">The Timesheet object to add.</param>
        /// <returns>The Timesheet object with its new id filled in.</returns>
        Timesheet CreateTimesheet(Timesheet newTimesheet);

        /// <summary>
        /// Updates a timesheet to the data store. Only called on timesheets that are already in the data store.
        /// </summary>
        /// <param name="updatedTimesheet">The Timesheet object to update</param>
        void UpdateTimesheet(Timesheet updatedTimesheet);

        /// <summary>
        /// Removes a timesheet from the data store.
        /// </summary>
        /// <param name="timesheetId">The id of the timesheet to remove.</param>
        void DeleteTimesheet(int timesheetId);

        /// <summary>
        /// Gets the total hours from all timesheets classified as billable
        /// for a given combination of employee and project.
        /// Non-billable timesheets are ignored.
        /// </summary>
        /// <param name="employeeId">The id of an employee.</param>
        /// <param name="projectId">The id of a project.</param>
        /// <returns>The total billable hours.</returns>
        decimal GetBillableHours(int employeeId, int projectId);
    }
}
