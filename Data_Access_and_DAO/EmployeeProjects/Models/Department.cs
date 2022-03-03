namespace EmployeeProjects.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }

        public string Name { get; set; }

        public Department() { }

        public Department(int id, string name)
        {
            DepartmentId = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
