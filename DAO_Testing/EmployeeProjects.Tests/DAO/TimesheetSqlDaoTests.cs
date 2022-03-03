using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using EmployeeProjects.DAO;
using EmployeeProjects.Models;

namespace EmployeeProjects.Tests.DAO
{
    [TestClass]
    public class TimesheetSqlDaoTests : BaseDaoTests
    {
        private static readonly Timesheet TIMESHEET_1 = new Timesheet(1, 1, 1, DateTime.Parse("2021-01-01"), 1.0M, true, "Timesheet 1");
        private static readonly Timesheet TIMESHEET_2 = new Timesheet(2, 1, 1, DateTime.Parse("2021-01-02"), 1.5M, true, "Timesheet 2");
        private static readonly Timesheet TIMESHEET_3 = new Timesheet(3, 2, 1, DateTime.Parse("2021-01-01"), 0.25M, true, "Timesheet 3");
        private static readonly Timesheet TIMESHEET_4 = new Timesheet(4, 2, 2, DateTime.Parse("2021-02-01"), 2.0M, false, "Timesheet 4");

        private TimesheetSqlDao dao;
        private Timesheet testTimeSheet;

        [TestInitialize]
        public override void Setup()
        {
            dao = new TimesheetSqlDao(ConnectionString);
            testTimeSheet = new Timesheet(0 ,2, 2, DateTime.Parse("2021-02-01"), 2.0M, false, "Timesheet 0");
            base.Setup();
        }

        [TestMethod]
        public void GetTimesheet_ReturnsCorrectTimesheetForId()
        {
            Timesheet timesheet = dao.GetTimesheet(1);

            AssertTimesheetsMatch(TIMESHEET_1, timesheet);

            timesheet = dao.GetTimesheet(2);

            AssertTimesheetsMatch(TIMESHEET_2, timesheet);
        }

        [TestMethod]
        public void GetTimesheet_ReturnsNullWhenIdNotFound()
        {
            Timesheet timesheet = dao.GetTimesheet(0);
            Assert.IsNull(timesheet);
        }

        [TestMethod]
        public void GetTimesheetsByEmployeeId_ReturnsListOfAllTimesheetsForEmployee()
        {
            IList<Timesheet> timesheets = dao.GetTimesheetsByEmployeeId(1);
            Assert.AreEqual(2, timesheets.Count);
            AssertTimesheetsMatch(TIMESHEET_1, timesheets[0]);
            AssertTimesheetsMatch(TIMESHEET_2, timesheets[1]);

            timesheets = dao.GetTimesheetsByEmployeeId(2);
            Assert.AreEqual(2, timesheets.Count);
            AssertTimesheetsMatch(TIMESHEET_3, timesheets[0]);
            AssertTimesheetsMatch(TIMESHEET_4, timesheets[1]);
        }

        [TestMethod]
        public void GetTimesheetsByProjectId_ReturnsListOfAllTimesheetsForProject()
        {
            IList<Timesheet> timesheets = dao.GetTimesheetsByProjectId(1);
            Assert.AreEqual(3, timesheets.Count);
            AssertTimesheetsMatch(TIMESHEET_1, timesheets[0]);
            AssertTimesheetsMatch(TIMESHEET_2, timesheets[1]);
            AssertTimesheetsMatch(TIMESHEET_3, timesheets[2]);

            timesheets = dao.GetTimesheetsByProjectId(2);
            Assert.AreEqual(1, timesheets.Count);
            AssertTimesheetsMatch(TIMESHEET_4, timesheets[0]);
        }

        [TestMethod]
        public void CreateTimesheet_ReturnsTimesheetWithIdAndExpectedValues()
        {
            Timesheet createdTimeSheet = dao.CreateTimesheet(testTimeSheet);

            Assert.IsTrue(createdTimeSheet.TimesheetId > 0);

            testTimeSheet.TimesheetId = createdTimeSheet.TimesheetId;

            AssertTimesheetsMatch(testTimeSheet, createdTimeSheet);

        }

        [TestMethod]
        public void CreatedTimesheetHasExpectedValuesWhenRetrieved()
        {
            Timesheet createdTimeSheet = dao.CreateTimesheet(testTimeSheet);
            int newId = createdTimeSheet.TimesheetId;

            Timesheet retrievedTimeSheet = dao.GetTimesheet(newId);
            AssertTimesheetsMatch(createdTimeSheet, retrievedTimeSheet);


        }

 

        [TestMethod]
        public void UpdatedTimesheetHasExpectedValuesWhenRetrieved()
        {
            Timesheet timesheetToUpdate = dao.GetTimesheet(1);

            timesheetToUpdate.EmployeeId = 2;
            timesheetToUpdate.ProjectId = 2;
            timesheetToUpdate.DateWorked = DateTime.Parse("2022-2-24");
            timesheetToUpdate.HoursWorked = 7;
            timesheetToUpdate.IsBillable = false;
            timesheetToUpdate.Description = "Updated Timesheet 1";

            dao.UpdateTimesheet(timesheetToUpdate);
            Timesheet timeSheetRetrived = dao.GetTimesheet(1);

            AssertTimesheetsMatch(timesheetToUpdate, timeSheetRetrived);
        }



        [TestMethod]
        public void DeletedTimesheetCantBeRetrieved()
        {
            dao.DeleteTimesheet(4);
            Timesheet retrievedTimeSheet = dao.GetTimesheet(4);
            Assert.IsNull(retrievedTimeSheet);

            IList<Timesheet> timesheets = dao.GetTimesheetsByEmployeeId(2);
            Assert.AreEqual(1, timesheets.Count);
            AssertTimesheetsMatch(TIMESHEET_3, timesheets[0]);
        }

        [TestMethod]
        public void GetBillableHours_ReturnsCorrectTotal()
        {
            decimal timesheetHours = dao.GetBillableHours(1,1);
            Assert.AreEqual(2.50M, timesheetHours);

           timesheetHours = dao.GetBillableHours(2, 2);
            Assert.AreEqual(0, timesheetHours);
        }
 
        private void AssertTimesheetsMatch(Timesheet expected, Timesheet actual)
        {
            Assert.AreEqual(expected.TimesheetId, actual.TimesheetId);
            Assert.AreEqual(expected.EmployeeId, actual.EmployeeId);
            Assert.AreEqual(expected.ProjectId, actual.ProjectId);
            Assert.AreEqual(expected.DateWorked.Date, actual.DateWorked.Date);
            Assert.AreEqual(expected.HoursWorked, actual.HoursWorked);
            Assert.AreEqual(expected.IsBillable, actual.IsBillable);
            Assert.AreEqual(expected.Description, actual.Description);
        }
    }
}
