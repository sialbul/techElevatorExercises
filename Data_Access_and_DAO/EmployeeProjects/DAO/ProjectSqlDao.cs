using EmployeeProjects.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EmployeeProjects.DAO
{
    public class ProjectSqlDao : IProjectDao
    {
        private readonly string connectionString;

        public ProjectSqlDao(string connString)
        {
            connectionString = connString;
        }

        public Project GetProject(int projectId)
        {


            Project project = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT project_id,name,from_date,to_date FROM project WHERE project_id = @project_id", conn);
                cmd.Parameters.AddWithValue("@project_id", projectId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    project = new Project();
                    project.ProjectId = Convert.ToInt32(reader["project_id"]);
                    project.Name = Convert.ToString(reader["name"]);
                    project.FromDate = Convert.ToDateTime(reader["from_date"]);
                    project.ToDate = Convert.ToDateTime(reader["to_date"]);
                }
                else
                {
                    Console.WriteLine("Project by id is not found.");
                }
            }
            return project;
        }

        public IList<Project> GetAllProjects()
        {
            IList<Project> projects = new List<Project>();


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM project;", conn);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Project project = new Project();
                    project.ProjectId = Convert.ToInt32(reader["project_id"]);
                    project.Name = Convert.ToString(reader["name"]);
                    project.FromDate = Convert.ToDateTime(reader["from_date"]);
                    project.ToDate = Convert.ToDateTime(reader["to_date"]);
                    projects.Add(project);
                }
            }
            return projects;
        }

        public Project CreateProject(Project newProject)
        {
            int newProjectId;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO project (name, from_date, to_date) " +
                                                "OUTPUT INSERTED.project_id " +
                                                "VALUES (@name, @from_date, @to_date);", conn);
                cmd.Parameters.AddWithValue("@name", newProject.Name);
                cmd.Parameters.AddWithValue("@from_date", newProject.FromDate);
                cmd.Parameters.AddWithValue("@to_date", newProject.ToDate);

                newProjectId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return GetProject(newProjectId);

        }

        public void DeleteProject(int projectId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"DELETE FROM project_employee WHERE project_id = @project_id;
                                                  DELETE FROM timesheet WHERE project_id = @project_id;
                                                  DELETE FROM project WHERE project_id = @project_id;", conn);
                cmd.Parameters.AddWithValue("@project_id", projectId);

                cmd.ExecuteNonQuery();
            }
        }

    }
}
