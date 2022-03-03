using EmployeeProjects.DAO;
using EmployeeProjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeProjects
{
    public class EmployeeProjectsCLI
    {

        private static readonly string MAIN_MENU_OPTION_EMPLOYEES = "Employees";
	    private static readonly string MAIN_MENU_OPTION_DEPARTMENTS = "Departments";
	    private static readonly string MAIN_MENU_OPTION_PROJECTS = "Projects";
	    private static readonly string MAIN_MENU_OPTION_EXIT = "Exit";
	    private static readonly string[] MAIN_MENU_OPTIONS = new string[] { MAIN_MENU_OPTION_DEPARTMENTS,
																	        MAIN_MENU_OPTION_EMPLOYEES,
																	        MAIN_MENU_OPTION_PROJECTS,
																	        MAIN_MENU_OPTION_EXIT };

        private static readonly string MENU_OPTION_RETURN_TO_MAIN = "Return to main menu";

        private static readonly string DEPT_MENU_OPTION_ALL_DEPARTMENTS = "Show all departments";
        private static readonly string DEPT_MENU_OPTION_UPDATE_NAME = "Update department name";
        private static readonly string[] DEPARTMENT_MENU_OPTIONS = new string[] { DEPT_MENU_OPTION_ALL_DEPARTMENTS,
                                                                                  DEPT_MENU_OPTION_UPDATE_NAME,
                                                                                  MENU_OPTION_RETURN_TO_MAIN
};

        private static readonly string EMPL_MENU_OPTION_ALL_EMPLOYEES = "Show all employees";
        private static readonly string EMPL_MENU_OPTION_SEARCH_BY_NAME = "Employee search by name";
        private static readonly string EMPL_MENU_OPTION_EMPLOYEES_NO_PROJECTS = "Show employees without projects";
        private static readonly string[] EMPL_MENU_OPTIONS = new string[] { EMPL_MENU_OPTION_ALL_EMPLOYEES,
                                                                            EMPL_MENU_OPTION_SEARCH_BY_NAME,
                                                                            EMPL_MENU_OPTION_EMPLOYEES_NO_PROJECTS,
                                                                            MENU_OPTION_RETURN_TO_MAIN};

        private static readonly string PROJ_MENU_OPTION_ALL_PROJECTS = "Show all projects";
        private static readonly string PROJ_MENU_OPTION_ADD_PROJECT = "Add new project";
        private static readonly string PROJ_MENU_OPTION_PROJECT_EMPLOYEES = "Show project employees";
        private static readonly string PROJ_MENU_OPTION_ASSIGN_EMPLOYEE_TO_PROJECT = "Assign an employee to a project";
        private static readonly string PROJ_MENU_OPTION_REMOVE_EMPLOYEE_FROM_PROJECT = "Remove employee from project";
        private static readonly string PROJ_MENU_OPTION_DELETE_PROJECT = "Delete project";
        private static readonly string[] PROJ_MENU_OPTIONS = new string[] { PROJ_MENU_OPTION_ALL_PROJECTS,
                                                                            PROJ_MENU_OPTION_ADD_PROJECT,
                                                                            PROJ_MENU_OPTION_PROJECT_EMPLOYEES,
                                                                            PROJ_MENU_OPTION_ASSIGN_EMPLOYEE_TO_PROJECT,
                                                                            PROJ_MENU_OPTION_REMOVE_EMPLOYEE_FROM_PROJECT,
                                                                            PROJ_MENU_OPTION_DELETE_PROJECT,
                                                                            MENU_OPTION_RETURN_TO_MAIN };

        private readonly IEmployeeDao employeeDao;
        private readonly IProjectDao projectDao;
        private readonly IDepartmentDao departmentDao;

        public EmployeeProjectsCLI(IEmployeeDao employeeDao, IProjectDao projectDao, IDepartmentDao departmentDao)
        {
            this.employeeDao = employeeDao;
            this.projectDao = projectDao;
            this.departmentDao = departmentDao;
        }

        public void Run()
        {
            DisplayBanner();

			bool running = true;
			while (running)
			{
				PrintHeading("Main Menu");
				string choice = (string)CLIHelper.GetChoiceFromOptions(MAIN_MENU_OPTIONS);
				if (choice == MAIN_MENU_OPTION_DEPARTMENTS)
				{
					HandleDepartments();
				}
				else if (choice == MAIN_MENU_OPTION_EMPLOYEES)
				{
					HandleEmployees();
				}
				else if (choice == MAIN_MENU_OPTION_PROJECTS)
				{
					HandleProjects();
				}
				else if (choice == MAIN_MENU_OPTION_EXIT)
				{
					running = false;
				}
			}
		}

		private void HandleDepartments()
		{
			PrintHeading("Departments");
			string choice = (string)CLIHelper.GetChoiceFromOptions(DEPARTMENT_MENU_OPTIONS);
			if (choice == DEPT_MENU_OPTION_ALL_DEPARTMENTS)
			{
				HandleListAllDepartments();
			}
			else if (choice == DEPT_MENU_OPTION_UPDATE_NAME)
			{
				HandleUpdateDepartmentName();
			}
		}

		private void HandleUpdateDepartmentName()
		{
			PrintHeading("Update Department Name");
			IList<Department> allDepartments = departmentDao.GetAllDepartments();
			if (allDepartments.Count > 0)
			{
				Console.WriteLine("\n*** Choose a Department ***");
				Department selectedDepartment = (Department)CLIHelper.GetChoiceFromOptions(allDepartments.ToArray());
				string newDepartmentName = GetUserInput("Enter new Department name");
				selectedDepartment.Name = newDepartmentName;
				departmentDao.UpdateDepartment(selectedDepartment);
			}
			else
			{
				Console.WriteLine("\n*** No results ***");
			}
		}

		private void HandleListAllDepartments()
		{
			PrintHeading("All Departments");
			IList<Department> allDepartments = departmentDao.GetAllDepartments();
			ListDepartments(allDepartments);
		}


		private void ListDepartments(IList<Department> departments)
		{
			Console.WriteLine();
			if (departments.Count > 0)
			{
				foreach (Department dept in departments)
				{
					Console.WriteLine(dept);
				}
			}
			else
			{
				Console.WriteLine("\n*** No results ***");
			}
		}

		private void HandleEmployees()
		{
			PrintHeading("Employees");
			string choice = (string)CLIHelper.GetChoiceFromOptions(EMPL_MENU_OPTIONS);
			if (choice == EMPL_MENU_OPTION_ALL_EMPLOYEES)
			{
				HandleListAllEmployees();
			}
			else if (choice == EMPL_MENU_OPTION_SEARCH_BY_NAME)
			{
				HandleEmployeeSearch();
			}
			else if (choice == EMPL_MENU_OPTION_EMPLOYEES_NO_PROJECTS)
			{
				HandleUnassignedEmployeeSearch();
			}
		}

		private void HandleListAllEmployees()
		{
			PrintHeading("All Employees");
			IList<Employee> allEmployees = employeeDao.GetAllEmployees();
			ListEmployees(allEmployees);
		}

		private void HandleEmployeeSearch()
		{
			PrintHeading("Employee Search");
			string firstNameSearch = GetUserInput("Enter first name to search for");
			string lastNameSearch = GetUserInput("Enter last name to search for");
			IList<Employee> employees = employeeDao.SearchEmployeesByName(firstNameSearch, lastNameSearch);
			ListEmployees(employees);
		}

		private void HandleUnassignedEmployeeSearch()
		{
			PrintHeading("Unassigned Employees");
			IList<Employee> employees = employeeDao.GetEmployeesWithoutProjects();
			ListEmployees(employees);
		}

		private void ListEmployees(IList<Employee> employees)
		{
			Console.WriteLine();
			if (employees.Count > 0)
			{
				foreach (Employee employee in employees)
				{
					Console.WriteLine(employee);
				}
			}
			else
			{
				Console.WriteLine("\n*** No results ***");
			}
		}

		private void HandleProjects()
		{
			PrintHeading("Projects");
			string choice = (string)CLIHelper.GetChoiceFromOptions(PROJ_MENU_OPTIONS);
			if (choice == PROJ_MENU_OPTION_ALL_PROJECTS)
			{
				HandleListActiveProjects();
			}
			else if (choice ==  PROJ_MENU_OPTION_ADD_PROJECT)
			{
				HandleAddProject();
			}
			else if (choice == PROJ_MENU_OPTION_PROJECT_EMPLOYEES)
			{
				HandleProjectEmployeeList();
			}
			else if (choice == PROJ_MENU_OPTION_ASSIGN_EMPLOYEE_TO_PROJECT)
			{
				HandleEmployeeProjectAssignment();
			}
			else if (choice == PROJ_MENU_OPTION_REMOVE_EMPLOYEE_FROM_PROJECT)
			{
				HandleEmployeeProjectRemoval();
			}
			else if (choice == PROJ_MENU_OPTION_DELETE_PROJECT)
			{
				HandleDeleteProject();
			}
		}

		private void HandleListActiveProjects()
		{
			PrintHeading("All Projects");
			IList<Project> projects = projectDao.GetAllProjects();
			ListProjects(projects);
		}

		private void HandleAddProject()
		{
			PrintHeading("Add New Project");
			string newProjectName = GetUserInput("Enter new Project name");
			string startDate = GetUserInput("Enter start date (YYYY-MM-DD)");
			string endDate = GetUserInput("Enter end date (YYYY-MM-DD)");
            Project newProject = new Project
            {
                Name = newProjectName,
                FromDate = DateTime.Parse(startDate),
                ToDate = DateTime.Parse(endDate)
            };
            newProject = projectDao.CreateProject(newProject);
			Console.WriteLine("\n*** " + newProject.Name + " created ***");
		}

		private void HandleEmployeeProjectRemoval()
		{
			PrintHeading("Remove Employee From Project");

			Project selectedProject = GetProjectSelectionFromUser();

			Console.WriteLine("Choose an employee to remove:");
			IList<Employee> projectEmployees = employeeDao.GetEmployeesByProjectId(selectedProject.ProjectId);
			if (projectEmployees.Count > 0)
			{
				Employee selectedEmployee = (Employee)CLIHelper.GetChoiceFromOptions(projectEmployees.ToArray());
				employeeDao.RemoveEmployeeFromProject(selectedProject.ProjectId, selectedEmployee.EmployeeId);
				Console.WriteLine("\n*** " + selectedEmployee + " removed from " + selectedProject + " ***");
			}
			else
			{
				Console.WriteLine("\n*** No results ***");
			}
		}

		private void HandleEmployeeProjectAssignment()
		{
			PrintHeading("Assign Employee To Project");

			Project selectedProject = GetProjectSelectionFromUser();

			Console.WriteLine("Choose an employee to add:");
			IList<Employee> allEmployees = employeeDao.GetAllEmployees();
			Employee selectedEmployee = (Employee)CLIHelper.GetChoiceFromOptions(allEmployees.ToArray());

			employeeDao.AddEmployeeToProject(selectedProject.ProjectId, selectedEmployee.EmployeeId);
			Console.WriteLine("\n*** " + selectedEmployee + " added to " + selectedProject + " ***");
		}

		private void HandleDeleteProject()
		{
			PrintHeading("Delete Project");
			Project selectedProject = GetProjectSelectionFromUser();

			projectDao.DeleteProject(selectedProject.ProjectId);
			Console.WriteLine("\n*** " + selectedProject.Name + " deleted ***");
		}

		private void HandleProjectEmployeeList()
		{
			Project selectedProject = GetProjectSelectionFromUser();
			IList<Employee> projectEmployees = employeeDao.GetEmployeesByProjectId(selectedProject.ProjectId);
			ListEmployees(projectEmployees);
		}

		private Project GetProjectSelectionFromUser()
		{
			Console.WriteLine("Choose a project:");
			IList<Project> allProjects = projectDao.GetAllProjects();
			return (Project)CLIHelper.GetChoiceFromOptions(allProjects.ToArray());
		}

		private void ListProjects(IList<Project> projects)
		{
			Console.WriteLine();
			if (projects.Count > 0)
			{
				foreach (Project proj in projects)
				{
					Console.WriteLine(proj);
				}
			}
			else
			{
				Console.WriteLine("\n*** No results ***");
			}
		}

		private void PrintHeading(string headingText)
		{
			Console.WriteLine("\n" + headingText);
			for (int i = 0; i < headingText.Length; i++)
			{
				Console.Write("-");
			}
			Console.WriteLine();
		}

		private string GetUserInput(string prompt)
		{
			Console.Write(prompt + " >>> ");
			return Console.ReadLine();
		}

		private void DisplayBanner()
        {
            Console.WriteLine(@" ______                 _                           _____           _           _       _____  ____  ");
            Console.WriteLine(@"|  ____|               | |                         |  __ \         (_)         | |     |  __ \|  _ \ ");
            Console.WriteLine(@"| |__   _ __ ___  _ __ | | ___  _   _  ___  ___    | |__) | __ ___  _  ___  ___| |_    | |  | | |_) |");
            Console.WriteLine(@"|  __| | '_ ` _ \| '_ \| |/ _ \| | | |/ _ \/ _ \   |  ___/ '__/ _ \| |/ _ \/ __| __|   | |  | |  _ < ");
            Console.WriteLine(@"| |____| | | | | | |_) | | (_) | |_| |  __/  __/   | |   | | | (_) | |  __/ (__| |_    | |__| | |_) |");
            Console.WriteLine(@"|______|_| |_| |_| .__/|_|\___/ \__, |\___|\___|   |_|   |_|  \___/| |\___|\___|\__|   |_____/|____/ ");
            Console.WriteLine(@"                 | |             __/ |                            _/ |                               ");
            Console.WriteLine(@"                 |_|            |___/                            |__/                                ");
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
