using Microsoft.Extensions.Configuration;
using EmployeeProjects.DAO;
using System.IO;

namespace EmployeeProjects
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the connection string from the appsettings.json file
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            string connectionString = configuration.GetConnectionString("EmployeeProjects");

            IProjectDao projectDao = new ProjectSqlDao(connectionString);
            IEmployeeDao employeeDao = new EmployeeSqlDao(connectionString);
            IDepartmentDao departmentDao = new DepartmentSqlDao(connectionString);

            EmployeeProjectsCLI application = new EmployeeProjectsCLI(employeeDao, projectDao, departmentDao);
            application.Run();
        }
    }
}
