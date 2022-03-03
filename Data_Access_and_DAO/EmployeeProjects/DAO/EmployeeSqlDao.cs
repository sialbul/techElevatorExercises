using EmployeeProjects.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EmployeeProjects.DAO
{
    public class EmployeeSqlDao : IEmployeeDao
    {
        private readonly string connectionString;

        public EmployeeSqlDao(string connString)
        {
            connectionString = connString;
        }

        public IList<Employee> GetAllEmployees()
        {
            IList<Employee> employees = new List<Employee>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM employee;", conn);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Employee employee = new Employee();
                    employee.EmployeeId = Convert.ToInt32(reader["employee_id"]);
                    employee.DepartmentId = Convert.ToInt32(reader["department_id"]);
                    employee.FirstName = Convert.ToString(reader["first_name"]);
                    employee.LastName = Convert.ToString(reader["last_name"]);
                    employee.BirthDate = Convert.ToDateTime(reader["birth_date"]);
                    employee.HireDate = Convert.ToDateTime(reader["hire_date"]);
                    employees.Add(employee);
                }
            }
            return employees;

        }

        public IList<Employee> SearchEmployeesByName(string firstNameSearch, string lastNameSearch)
        {
            IList<Employee> employees = new List<Employee>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(@"SELECT employee_id,  department_id, first_name, last_name, birth_date, hire_date FROM employee
                WHERE first_name LIKE @first_name AND last_name LIKE @last_name;", conn);

                command.Parameters.AddWithValue("@first_name", "%" + firstNameSearch + "%");
                command.Parameters.AddWithValue("@last_name", "%" + lastNameSearch + "%");

               SqlDataReader reader = command.ExecuteReader();


                while (reader.Read())
                {
                    Employee employee = new Employee();
                    employee.EmployeeId = Convert.ToInt32(reader["employee_id"]);
                    employee.DepartmentId = Convert.ToInt32(reader["department_id"]);
                    employee.FirstName = Convert.ToString(reader["first_name"]);
                    employee.LastName = Convert.ToString(reader["last_name"]);
                    employee.BirthDate = Convert.ToDateTime(reader["birth_date"]);
                    employee.HireDate = Convert.ToDateTime(reader["hire_date"]);
                    employees.Add(employee);
                }
            }
            return employees;
        }

        public IList<Employee> GetEmployeesByProjectId(int projectId)
        {
            IList<Employee> employees = new List<Employee>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(@"SELECT employee.employee_id,  department_id, first_name, last_name, birth_date, hire_date FROM employee 
                                                      JOIN project_employee ON employee.employee_id=project_employee.employee_id
                                                      WHERE project_employee.project_id = @project_id", conn);

                command.Parameters.AddWithValue("@project_id", projectId);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Employee employee = new Employee();
                    employee.EmployeeId = Convert.ToInt32(reader["employee_id"]);
                    employee.DepartmentId = Convert.ToInt32(reader["department_id"]);
                    employee.FirstName = Convert.ToString(reader["first_name"]);
                    employee.LastName = Convert.ToString(reader["last_name"]);
                    employee.BirthDate = Convert.ToDateTime(reader["birth_date"]);
                    employee.HireDate = Convert.ToDateTime(reader["hire_date"]);
                    employees.Add(employee);
                }
            }
            return employees;
        }

        public void AddEmployeeToProject(int projectId, int employeeId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                //OPEN connection
                conn.Open();

                //create command
                SqlCommand cmd = new SqlCommand(@"INSERT INTO project_employee(project_id, employee_id)
                    VALUES (@project_id,@employee_id)", conn);

                //add parameters to command
                cmd.Parameters.AddWithValue("@project_id", projectId);
                cmd.Parameters.AddWithValue("@employee_id", employeeId);

                //execute command
                cmd.ExecuteNonQuery();
            }

        }

        public void RemoveEmployeeFromProject(int projectId, int employeeId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("DELETE FROM project_employee WHERE project_id = @project_id AND employee_id = @employee_id;", conn);
                cmd.Parameters.AddWithValue("@project_id", projectId);
                cmd.Parameters.AddWithValue("@employee_id", employeeId);

                cmd.ExecuteNonQuery();
            }

        }

        public IList<Employee> GetEmployeesWithoutProjects()
        {
            IList<Employee> employees = new List<Employee>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(@"SELECT employee.employee_id,  department_id, first_name, last_name, birth_date, hire_date
                                                        FROM employee
                                                        LEFT JOIN project_employee ON employee.employee_id=project_employee.employee_id
                                                           WHERE project_employee.employee_id is Null; ", conn);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Employee employee = new Employee();
                    employee.EmployeeId = Convert.ToInt32(reader["employee_id"]);
                    employee.DepartmentId = Convert.ToInt32(reader["department_id"]);
                    employee.FirstName = Convert.ToString(reader["first_name"]);
                    employee.LastName = Convert.ToString(reader["last_name"]);
                    employee.BirthDate = Convert.ToDateTime(reader["birth_date"]);
                    employee.HireDate = Convert.ToDateTime(reader["hire_date"]);
                    employees.Add(employee);
                }
            }
            return employees;
        }


    }
}
