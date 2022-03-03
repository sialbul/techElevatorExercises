using System;

namespace EmployeeProjects.Models
{
    public class Project
    {
        public int ProjectId { get; set; }

        public string Name { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public Project() { }

        public Project(int id, string name, DateTime fromDate, DateTime toDate)
        {
            ProjectId = id;
            Name = name;
            FromDate = fromDate;
            ToDate = toDate;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
