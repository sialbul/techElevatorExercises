using EmployeeProjects.Models;
using System.Collections.Generic;

namespace EmployeeProjects.DAO
{
    public interface IProjectDao
    {
        /// <summary>
        /// Gets a project from the data store that has the given id.
	    /// If the id is not found, return null.
        /// </summary>
        /// <param name="projectId">The id of the project to get from the data store.</param>
        /// <returns>A filled out Project object.</returns>
        Project GetProject(int projectId);

        /// <summary>
        /// Gets all projects from the data store.
        /// </summary>
        /// <returns>All projects as Project objects in an IList.</returns>
        IList<Project> GetAllProjects();

        /// <summary>
        /// Inserts a new project into the data store.
        /// </summary>
        /// <param name="newProject">The project object to insert.</param>
        /// <returns>The Project object with its new id filled in.</returns>
        Project CreateProject(Project newProject);

        /// <summary>
        /// Removes a project from the data store, which requires deleting records from multiple tables.
        /// </summary>
        /// <param name="projectId">The id of the project to remove.</param>
        void DeleteProject(int projectId);
    }
}
