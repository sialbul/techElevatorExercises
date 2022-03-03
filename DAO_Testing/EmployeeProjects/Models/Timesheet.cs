using System;

namespace EmployeeProjects.Models
{
    public class Timesheet
    {
        public int TimesheetId { get; set; }

        public int EmployeeId { get; set; }

        public int ProjectId { get; set; }

        public DateTime DateWorked { get; set; }

        public decimal HoursWorked { get; set; }

        public bool IsBillable { get; set; }

        public string Description { get; set; }

        public Timesheet() { }

        public Timesheet(int timesheetId, int employeeId, int projectId, DateTime dateWorked, decimal hoursWorked, bool isBillable, string description)
        {
            TimesheetId = timesheetId;
            EmployeeId = employeeId;
            ProjectId = projectId;
            DateWorked = dateWorked;
            HoursWorked = hoursWorked;
            IsBillable = isBillable;
            Description = description;
        }
    }
}
