using EmployeeProjects.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EmployeeProjects.DAO
{
    public class TimesheetSqlDao : ITimesheetDao
    {
        private readonly string connectionString;

        public TimesheetSqlDao(string connString)
        {
            connectionString = connString;
        }

        public Timesheet GetTimesheet(int timesheetId)
        {
            Timesheet timesheet = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT timesheet_id, employee_id, project_id, date_worked, hours_worked, is_billable, description " +
                                                "FROM timesheet " +
                                                "WHERE timesheet_id = @timesheet_id;", conn);
                cmd.Parameters.AddWithValue("@timesheet_id", timesheetId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    timesheet = CreateTimesheetFromReader(reader);
                }
            }

            return timesheet;
        }

        public IList<Timesheet> GetTimesheetsByEmployeeId(int employeeId)
        {
            IList<Timesheet> timesheets = new List<Timesheet>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT timesheet_id, employee_id, project_id, date_worked, hours_worked, is_billable, description " +
                                                "FROM timesheet " +
                                                "WHERE employee_id = @employee_id " +
                                                "ORDER BY timesheet_id;", conn);
                cmd.Parameters.AddWithValue("@employee_id", employeeId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Timesheet timesheet = CreateTimesheetFromReader(reader);
                    timesheets.Add(timesheet);
                }
            }

            return timesheets;
        }

        public IList<Timesheet> GetTimesheetsByProjectId(int projectId)
        {
            IList<Timesheet> timesheets = new List<Timesheet>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT timesheet_id, employee_id, project_id, date_worked, hours_worked, is_billable, description " +
                                                "FROM timesheet " +
                                                "WHERE project_id = @project_id " +
                                                "ORDER BY timesheet_id;", conn);
                cmd.Parameters.AddWithValue("@project_id", projectId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Timesheet timesheet = CreateTimesheetFromReader(reader);
                    timesheets.Add(timesheet);
                }
            }

            return timesheets;
        }

        public Timesheet CreateTimesheet(Timesheet newTimesheet)
        {
            int newTimesheetId;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO timesheet (employee_id, project_id, date_worked, hours_worked, is_billable, description) " +
                                                "OUTPUT INSERTED.timesheet_id " +
                                                "VALUES (@employee_id, @project_id, @date_worked, @hours_worked, @is_billable, @description);", conn);
                cmd.Parameters.AddWithValue("@employee_id", newTimesheet.EmployeeId);
                cmd.Parameters.AddWithValue("@project_id", newTimesheet.ProjectId);
                cmd.Parameters.AddWithValue("@date_worked", newTimesheet.DateWorked);
                cmd.Parameters.AddWithValue("@hours_worked", newTimesheet.HoursWorked);
                cmd.Parameters.AddWithValue("@is_billable", newTimesheet.IsBillable);
                cmd.Parameters.AddWithValue("@description", newTimesheet.Description);

                newTimesheetId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return GetTimesheet(newTimesheetId);
        }
            public void UpdateTimesheet(Timesheet updatedTimesheet)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE timesheet " +
                                                    "SET employee_id = @employee_id, project_id = @project_id, date_worked = @date_worked, " +
                                                    "hours_worked = @hours_worked, is_billable = @is_billable, description = @description " +
                                                    "WHERE timesheet_id = @timesheet_id;", conn);
                    cmd.Parameters.AddWithValue("@employee_id", updatedTimesheet.EmployeeId);
                    cmd.Parameters.AddWithValue("@project_id", updatedTimesheet.ProjectId);
                    cmd.Parameters.AddWithValue("@date_worked", updatedTimesheet.DateWorked);
                    cmd.Parameters.AddWithValue("@hours_worked", updatedTimesheet.HoursWorked);
                    cmd.Parameters.AddWithValue("@is_billable", updatedTimesheet.IsBillable);
                    cmd.Parameters.AddWithValue("@description", updatedTimesheet.Description);
                    cmd.Parameters.AddWithValue("@timesheet_id", updatedTimesheet.TimesheetId);

                    cmd.ExecuteNonQuery();
                }
            }



        public void DeleteTimesheet(int timesheetId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("DELETE FROM timesheet WHERE timesheet_id = @timesheet_id;", conn);
                cmd.Parameters.AddWithValue("@timesheet_id", timesheetId);
                cmd.ExecuteNonQuery();
            }
        }

        public decimal GetBillableHours(int employeeId, int projectId)
        {
            decimal billableHours = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT SUM(hours_worked) AS billable_hours " +
                                                "FROM timesheet " +
                                                "WHERE employee_id = @employee_id AND project_id = @project_id AND is_billable =1;", conn);
                cmd.Parameters.AddWithValue("@employee_id", employeeId);
                cmd.Parameters.AddWithValue("@project_id", projectId);

                object returnedValue = cmd.ExecuteScalar();

                if (returnedValue != DBNull.Value)
                {
                    billableHours = Convert.ToDecimal(returnedValue);
                }
              
            }

            return billableHours;
        }

        private Timesheet CreateTimesheetFromReader(SqlDataReader reader)
        {
            Timesheet timesheet = new Timesheet();
            timesheet.TimesheetId = Convert.ToInt32(reader["timesheet_id"]);
            timesheet.EmployeeId = Convert.ToInt32(reader["employee_id"]);
            timesheet.ProjectId = Convert.ToInt32(reader["project_id"]);
            timesheet.DateWorked = Convert.ToDateTime(reader["date_worked"]);
            timesheet.HoursWorked = Convert.ToDecimal(reader["hours_worked"]);
            timesheet.IsBillable = Convert.ToBoolean(reader["is_billable"]);
            timesheet.Description = Convert.ToString(reader["description"]);

            return timesheet;
        }
    }
}
