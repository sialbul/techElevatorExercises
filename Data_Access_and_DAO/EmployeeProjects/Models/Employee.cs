using System;

namespace EmployeeProjects.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        public int DepartmentId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime HireDate { get; set; }

        public Employee() { }

        public Employee(int employeeId, int departmentId, string firstName, string lastName, DateTime birthDate, DateTime hireDate)
        {
            EmployeeId = employeeId;
            DepartmentId = departmentId;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            HireDate = hireDate;
        }

        public override string ToString()
        {
            return $"{LastName}, {FirstName}";
        }
    }
}
