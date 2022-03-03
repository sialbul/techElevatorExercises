using EmployeeProjects.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EmployeeProjects.DAO
{
    public class DepartmentSqlDao : IDepartmentDao
    {
        private readonly string connectionString;

        public DepartmentSqlDao(string connString)
        {
            connectionString = connString;
        }

        public Department GetDepartment(int departmentId)
        {
            Department department = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT department_id, name FROM department WHERE department_id = @department_id;", conn);
                cmd.Parameters.AddWithValue("@department_id", departmentId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    department = new Department();
                    department.DepartmentId = Convert.ToInt32(reader["department_id"]);
                    department.Name = Convert.ToString(reader["name"]);
                }
                else
                {
                    Console.WriteLine("Department by id is not found.");
                }
            }
            return department;
        }

        public IList<Department> GetAllDepartments()
        {
            IList<Department> departments = new List<Department>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM department;", conn);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Department department = new Department();
                    department.DepartmentId = Convert.ToInt32(reader["department_id"]);
                    department.Name = Convert.ToString(reader["name"]);
                    departments.Add(department);
                }
            }
            return departments;

        }

        public void UpdateDepartment(Department updatedDepartment)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("UPDATE department SET name = @name WHERE department_id = @department_id;", conn);
                cmd.Parameters.AddWithValue("@department_id", updatedDepartment.DepartmentId);
                cmd.Parameters.AddWithValue("@name", updatedDepartment.Name);
         

                cmd.ExecuteNonQuery();
            }
        }

    }


}
