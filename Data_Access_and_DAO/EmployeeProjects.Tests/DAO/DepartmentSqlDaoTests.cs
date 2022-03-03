using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using EmployeeProjects.DAO;
using EmployeeProjects.Models;

namespace EmployeeProjects.Tests.DAO
{
    [TestClass]
    public class DepartmentSqlDaoTests : BaseDaoTests
    {
        private static readonly Department DEPARTMENT_1 = new Department(1, "Department 1");
        private static readonly Department DEPARTMENT_2 = new Department(2, "Department 2");

        private DepartmentSqlDao dao;

        [TestInitialize]
        public override void Setup()
        {
            dao = new DepartmentSqlDao(ConnectionString);
            base.Setup();
        }

        [TestMethod]
        public void GetDepartment_ReturnsCorrectDepartmentForId()
        {
            Department department = dao.GetDepartment(1);
            Assert.IsNotNull(department, "GetDepartment returned null");
            AssertDepartmentsMatch(DEPARTMENT_1, department, "GetDepartment returned wrong or partial data");

            department = dao.GetDepartment(2);
            Assert.IsNotNull(department, "GetDepartment returned null");
            AssertDepartmentsMatch(DEPARTMENT_2, department, "GetDepartment returned wrong or partial data");
        }

        [TestMethod]
        public void GetDepartment_ReturnsNullWhenIdNotFound()
        {
            Department department = dao.GetDepartment(99);
            Assert.IsNull(department, "GetDepartment failed to return null for id not in database");
        }

        [TestMethod]
        public void GetAllDepartments_ReturnsListOfAllDepartments()
        {
            IList<Department> departments = dao.GetAllDepartments();

            Assert.AreEqual(2, departments.Count, "GetAllDepartments failed to return all departments");
            AssertDepartmentsMatch(DEPARTMENT_1, departments[0], "GetAllDepartments returned wrong or partial data");
            AssertDepartmentsMatch(DEPARTMENT_2, departments[1], "GetAllDepartments returned wrong or partial data");
        }

        [TestMethod]
        public void UpdatedDepartmentHasExpectedNameWhenRetrieved()
        {
            Department department = dao.GetDepartment(1);
            Assert.IsNotNull(department, "Can't test updateDepartment until getDepartment is working");
            department.Name = "Test";

            dao.UpdateDepartment(department);

            Department updatedDept = dao.GetDepartment(1);
            Assert.AreEqual("Test", updatedDept.Name, "UpdateDepartment failed to change department name in database");
        }

        private void AssertDepartmentsMatch(Department expected, Department actual, string message)
        {
            Assert.AreEqual(expected.DepartmentId, actual.DepartmentId, message);
            Assert.AreEqual(expected.Name, actual.Name, message);
        }
    }
}
