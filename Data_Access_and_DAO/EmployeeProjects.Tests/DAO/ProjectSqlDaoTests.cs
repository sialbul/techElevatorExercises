using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using EmployeeProjects.DAO;
using EmployeeProjects.Models;

namespace EmployeeProjects.Tests.DAO
{
    [TestClass]
    public class ProjectSqlDaoTests : BaseDaoTests
    {
        private static readonly Project PROJECT_1 = new Project(1, "Project 1", DateTime.Parse("2000-01-02"), DateTime.Parse("2000-12-31"));
        private static readonly Project PROJECT_2 = new Project(2, "Project 2", DateTime.Parse("2001-01-02"), DateTime.Parse("2001-12-31"));

        private ProjectSqlDao dao;

        private Project testProject;


        [TestInitialize]
        public override void Setup()
        {
            dao = new ProjectSqlDao(ConnectionString);
            testProject = new Project(99, "Test Project", DateTime.Now.Date, DateTime.Now.Date.AddDays(1));
            base.Setup();
        }


        [TestMethod]
        public void GetProject_ReturnsCorrectProjectForId()
        {
            Project project = dao.GetProject(1);
            Assert.IsNotNull(project, "GetProject returned null");
            AssertProjectsMatch(PROJECT_1, project, "GetProject returned wrong or partial data");

            project = dao.GetProject(2);
            AssertProjectsMatch(PROJECT_2, project, "GetProject returned wrong or partial data");
        }

        [TestMethod]
        public void GetProject_ReturnsNullWhenIdNotFound()
        {
            Project project = dao.GetProject(99);
            Assert.IsNull(project, "GetProject failed to return null for id not in database");
        }

        [TestMethod]
        public void GetAllProjects_ReturnsListOfAllProjects()
        {
            IList<Project> projects = dao.GetAllProjects();

            Assert.AreEqual(2, projects.Count, "GetAllProjects failed to return all projects");
            AssertProjectsMatch(PROJECT_1, projects[0], "GetAllProjects returned wrong or partial data");
            AssertProjectsMatch(PROJECT_2, projects[1], "GetAllProjects returned wrong or partial data");
        }

        [TestMethod]
        public void CreateProject_ReturnsProjectWithIdAndExpectedValues()
        {
            Project createdProject = dao.CreateProject(testProject);

            Assert.IsNotNull(createdProject, "CreateProject returned null");

            int newId = createdProject.ProjectId;
            Assert.IsTrue(newId > 0, "CreateProject failed to return a project with an id");

            testProject.ProjectId = newId;
            AssertProjectsMatch(testProject, createdProject, "CreateProject returned project with wrong or partial data");
        }

        [TestMethod]
        public void CreatedProjectHasExpectedValuesWhenRetrieved()
        {
            Project createdProject = dao.CreateProject(testProject);

            Assert.IsNotNull(createdProject, "Can't test if created project has correct values until CreateProject is working");

            int newId = createdProject.ProjectId;
            Project retrievedProject = dao.GetProject(newId);

            AssertProjectsMatch(createdProject, retrievedProject, "CreateProject did not save correct data in database");
        }

        [TestMethod]
        public void DeletedProjectCantBeRetrieved()
        {
            dao.DeleteProject(1);

            Project project = dao.GetProject(1);
            Assert.IsNull(project, "DeleteProject failed to remove project from database");

            IList<Project> projects = dao.GetAllProjects();
            Assert.AreEqual(1, projects.Count, "DeleteProject left too many projects in database");
            AssertProjectsMatch(PROJECT_2, projects[0], "DeleteProject deleted wrong project");
        }

        private void AssertProjectsMatch(Project expected, Project actual, string message)
        {
            Assert.AreEqual(expected.ProjectId, actual.ProjectId, message);
            Assert.AreEqual(expected.Name, actual.Name, message);
            Assert.AreEqual(expected.FromDate, actual.FromDate, message);
            Assert.AreEqual(expected.ToDate, actual.ToDate, message);
        }
    }
}
