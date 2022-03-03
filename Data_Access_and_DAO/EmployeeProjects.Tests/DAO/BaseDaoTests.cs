using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;

namespace EmployeeProjects.Tests.DAO
{
    [TestClass]
    public class BaseDaoTests
    {
        private const string DatabaseName = "EmployeeProjectsTemp";

        private static string AdminConnectionString;
        protected static string ConnectionString;

        /// <summary>
        /// The transaction for each test.
        /// </summary>
        private TransactionScope transaction;

        [AssemblyInitialize]
        public static void BeforeAllTests(TestContext context)
        {
            SetConnectionString(DatabaseName);

            string sql = File.ReadAllText("create-test-db.sql").Replace("test_db_name", DatabaseName);

            using (SqlConnection conn = new SqlConnection(AdminConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.ExecuteNonQuery();
            }

            sql = File.ReadAllText("test-data.sql");
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
            }
        }

        [AssemblyCleanup]
        public static void AfterAllTests()
        {
            // drop the temporary database
            string sql = File.ReadAllText("drop-test-db.sql").Replace("test_db_name", DatabaseName);

            using (SqlConnection conn = new SqlConnection(AdminConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }


        [TestInitialize]
        public virtual void Setup()
        {
            // Begin the transaction
            transaction = new TransactionScope();

        }

        [TestCleanup]
        public void Cleanup()
        {
            // Roll back the transaction
            transaction.Dispose();
        }

        private static void SetConnectionString(string defaultDbName)
        {
            string host = System.Environment.GetEnvironmentVariable("DB_HOST") ?? @".\SQLEXPRESS";
            string dbName = System.Environment.GetEnvironmentVariable("DB_DATABASE") ?? defaultDbName;
            string username = System.Environment.GetEnvironmentVariable("DB_USERNAME");
            string password = System.Environment.GetEnvironmentVariable("DB_PASSWORD");

            if (username != null && password != null)
            {
                ConnectionString = $"Data Source={host};Initial Catalog={dbName};User Id={username};Password={password};";
            }
            else
            {
                ConnectionString = $"Data Source={host};Initial Catalog={dbName};Integrated Security=SSPI;";
            }
            AdminConnectionString = ConnectionString.Replace(dbName, "master");
        }

    }
}
