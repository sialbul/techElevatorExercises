using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using EmployeeProjects.DAO;
using EmployeeProjects.Models;

namespace EmployeeProjects.Tests.DAO
{
    [TestClass]
    public class EmployeeSqlDaoTests : BaseDaoTests
    {
        private static readonly Employee EMPLOYEE_1 =
            new Employee(1, 1, "First1", "Last1", DateTime.Parse("1981-01-01"), DateTime.Parse("2001-01-02"));
        private static readonly Employee EMPLOYEE_2 =
            new Employee(2, 2, "First2", "Last2", DateTime.Parse("1982-02-01"), DateTime.Parse("2002-02-03"));
        private static readonly Employee EMPLOYEE_3 =
            new Employee(3, 1, "First3", "Last3", DateTime.Parse("1983-03-01"), DateTime.Parse("2003-03-04"));

        private EmployeeSqlDao dao;

        [TestInitialize]
        public override void Setup()
        {
            dao = new EmployeeSqlDao(ConnectionString);
            base.Setup();
        }

        [TestMethod]
        public void GetAllEmployees_ReturnsListOfAllEmployees()
        {
            IList<Employee> employees = dao.GetAllEmployees();

            Assert.AreEqual(3, employees.Count, "GetAllEmployees failed to return all employees");
            AssertEmployeesMatch(EMPLOYEE_1, employees[0], "GetAllEmployees returned wrong or partial data");
            AssertEmployeesMatch(EMPLOYEE_2, employees[1], "GetAllEmployees returned wrong or partial data");
            AssertEmployeesMatch(EMPLOYEE_3, employees[2], "GetAllEmployees returned wrong or partial data");
        }

        [TestMethod]
        public void SearchEmployeesByName_FindsExactMatches()
        {
            IList<Employee> employees = dao.SearchEmployeesByName("First1", "Last1");
            Assert.AreEqual(1, employees.Count, "SearchEmployeesByName returned the wrong number of matches for an exact match");
            AssertEmployeesMatch(EMPLOYEE_1, employees[0], "SearchEmployeesByName returned wrong or partial data");

            employees = dao.SearchEmployeesByName("First2", "");
            Assert.AreEqual(1, employees.Count, "SearchEmployeesByName returned the wrong number of matches for an exact match");
            AssertEmployeesMatch(EMPLOYEE_2, employees[0], "SearchEmployeesByName returned wrong or partial data");

            employees = dao.SearchEmployeesByName("", "Last3");
            Assert.AreEqual(1, employees.Count, "SearchEmployeesByName returned the wrong number of matches for an exact match");
            AssertEmployeesMatch(EMPLOYEE_3, employees[0], "SearchEmployeesByName returned wrong or partial data");
        }

        [TestMethod]
        public void SearchEmployeesByName_FindsPartialMatches()
        {
            IList<Employee> employees = dao.SearchEmployeesByName("irst", "ast");
            Assert.AreEqual(3, employees.Count, "SearchEmployeesByName returned the wrong number of matches for a partial match");

            employees = dao.SearchEmployeesByName("first", "last");
            Assert.AreEqual(3, employees.Count, "SearchEmployeesByName returned the wrong number of matches for a case-insensitive match");

            employees = dao.SearchEmployeesByName("FIRST", "LAST");
            Assert.AreEqual(3, employees.Count, "SearchEmployeesByName returned the wrong number of matches for a case-insensitive match");

            employees = dao.SearchEmployeesByName("", "");
            Assert.AreEqual(3, employees.Count, "SearchEmployeesByName should return all employees when passed 2 empty strings");
        }

        [TestMethod]
        public void SearchEmployeesByName_FindsNoMatchesForNamesNotFound()
        {
            IList<Employee> employees = dao.SearchEmployeesByName("TEST", "TEST");
            Assert.AreEqual(0, employees.Count, "SearchEmployeesByName returned matches for full name not in database");

            employees = dao.SearchEmployeesByName("TEST", "Last1");
            Assert.AreEqual(0, employees.Count, "SearchEmployeesByName should only return matches when both names match");

            employees = dao.SearchEmployeesByName("First1", "TEST");
            Assert.AreEqual(0, employees.Count, "SearchEmployeesByName should only return matches when both names match");
        }

        [TestMethod]
        public void GetEmployeesByProjectId_ReturnsAllEmployeesForProject()
        {
            IList<Employee> employees = dao.GetEmployeesByProjectId(1);
            Assert.AreEqual(1, employees.Count, "GetEmployeesByProjectId returned wrong number of employees");
            AssertEmployeesMatch(EMPLOYEE_1, employees[0], "GetEmployeesByProjectId returned wrong or partial data");

            employees = dao.GetEmployeesByProjectId(2);
            Assert.AreEqual(2, employees.Count, "GetEmployeesByProjectId returned wrong number of employees");
            AssertEmployeesMatch(EMPLOYEE_2, employees[0], "GetEmployeesByProjectId returned wrong or partial data");
            AssertEmployeesMatch(EMPLOYEE_3, employees[1], "GetEmployeesByProjectId returned wrong or partial data");

            employees = dao.GetEmployeesByProjectId(99);
            Assert.AreEqual(0, employees.Count, "GetEmployeesByProjectId returned employees for project id not in database");
        }

        [TestMethod]
        public void AddEmployeeToProject_EmployeeAddedIsInListOfEmployeesForProject()
        {
            dao.AddEmployeeToProject(1, 3);
            IList<Employee> employees = dao.GetEmployeesByProjectId(1);
            Assert.AreEqual(2, employees.Count, "AddEmployeeToProject didn't increase number of employees assigned to project");
            AssertEmployeesMatch(EMPLOYEE_3, employees[1], "AddEmployeeToProject assigned wrong employee to project");
        }

        [TestMethod]
        public void RemoveEmployeeFromProject_RemovedEmployeeIsNotInListOfEmployeesForProject()
        {
            dao.RemoveEmployeeFromProject(2, 3);
            IList<Employee> employees = dao.GetEmployeesByProjectId(2);
            Assert.AreEqual(1, employees.Count, "RemoveEmployeeFromProject didn't decrease number of employees assigned to project");
            AssertEmployeesMatch(EMPLOYEE_2, employees[0], "RemoveEmployeeFromProject removed wrong employee from project");
        }

        [TestMethod]
        public void GetEmployeesWithoutProjects_FindsAllEmployeesNotAssignedToProjects()
        {
            IList<Employee> employees = dao.GetEmployeesWithoutProjects();
            Assert.AreEqual(0, employees.Count, "GetEmployeesWithoutProjects returned employees assigned to projects");

            dao.RemoveEmployeeFromProject(1, 1);
            employees = dao.GetEmployeesWithoutProjects();
            Assert.AreEqual(1, employees.Count, "GetEmployeesWithoutProjects returned the wrong number of employees without projects");
            AssertEmployeesMatch(EMPLOYEE_1, employees[0], "GetEmployeesWithoutProjects returned wrong or partial data");
        }

        private void AssertEmployeesMatch(Employee expected, Employee actual, string message)
        {
            Assert.AreEqual(expected.EmployeeId, actual.EmployeeId, message);
            Assert.AreEqual(expected.DepartmentId, actual.DepartmentId, message);
            Assert.AreEqual(expected.FirstName, actual.FirstName, message);
            Assert.AreEqual(expected.LastName, actual.LastName, message);
            Assert.AreEqual(expected.BirthDate, actual.BirthDate, message);
            Assert.AreEqual(expected.HireDate, actual.HireDate, message);
        }
    }
}
