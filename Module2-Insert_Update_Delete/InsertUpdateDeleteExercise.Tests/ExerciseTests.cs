using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InsertUpdateDeleteExercise.Tests
{
    [TestClass]
    public class ExerciseTests
    {
        private static string mainConnectionString;
        private static string movieConnectionString;
        private SqlConnection conn;
        private TransactionScope transaction;
        private static string exerciseFolder;
        private static List<string> exerciseFiles;

        [ClassInitialize]
        public static void InitializeTestClass(TestContext context)
        {
            SetConnectionStrings("MovieDBTemp");
            exerciseFiles = GetExerciseSqlFiles();
            if (exerciseFiles.Count == 0)
            {
                Assert.Fail("Exercise folder and files not found. Please check that the exercise folder is in the same directory or in a directory above where these tests are running from.");
            }

            // create a smaller temporary database with the same schema but less irrelevant data
            string sqlCreate = File.ReadAllText("MovieDBTemp-create.sql");
            string sqlLoad = File.ReadAllText("MovieDBTemp-load.sql");

            using (SqlConnection mainConn = new SqlConnection(mainConnectionString))
            using (SqlConnection movieConn = new SqlConnection(movieConnectionString))
            {
                mainConn.Open();
                SqlCommand cmdCreate = new SqlCommand(sqlCreate, mainConn);
                cmdCreate.ExecuteNonQuery();

                movieConn.Open();
                SqlCommand cmdLoad = new SqlCommand(sqlLoad, movieConn);
                cmdLoad.ExecuteNonQuery();
            }
        }

        [ClassCleanup]
        public static void AfterAllTests()
        {
            // drop the temporary database
            string sql = File.ReadAllText("MovieDBTemp-drop.sql");

            using (SqlConnection conn = new SqlConnection(mainConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Initializes the database for the test.
        /// </summary>
        [TestInitialize]
        public void BeforeEachTest()
        {
            // Begin the transaction
            transaction = new TransactionScope();
        }

        /// <summary>
        /// Cleans up the database after each test.
        /// </summary>
        [TestCleanup]
        public void AfterEachTest()
        {
            if (transaction != null)
            {
                transaction.Dispose();
            }
        }

        [TestMethod]
        public void ExerciseProblem01()
        {
            int expectedRowsAffected = 1;

            string sqlVerify = "SELECT person_name, birthday FROM person WHERE person_name = 'Lisa Byway';";

            DataTable dtExpected = new DataTable();
            dtExpected.Columns.Add("person_name");
            dtExpected.Columns.Add("birthday", typeof(DateTime));
            dtExpected.Rows.Add(new object[] { "Lisa Byway", new DateTime(1984, 4, 15) });

            CheckAnswerForProblem("01", expectedRowsAffected, sqlVerify, dtExpected);
        }

        [TestMethod]
        public void ExerciseProblem02()
        {
            int expectedRowsAffected = 1;

            string sqlVerify = "SELECT title, overview, release_date, length_minutes FROM movie WHERE title = 'Euclidean Pi';";

            DataTable dtExpected = new DataTable();
            dtExpected.Columns.Add("title");
            dtExpected.Columns.Add("overview");
            dtExpected.Columns.Add("release_date", typeof(DateTime));
            dtExpected.Columns.Add("length_minutes", typeof(int));
            dtExpected.Rows.Add(new object[] { "Euclidean Pi", "The epic story of Euclid as a pizza delivery boy in ancient Greece", new DateTime(2015, 3, 14), 194 });

            CheckAnswerForProblem("02", expectedRowsAffected, sqlVerify, dtExpected);
        }

        [TestMethod]
        public void ExerciseProblem03()
        {
            int expectedRowsAffected = 1;

            string sqlVerify = "SELECT count(*) as count FROM movie_actor WHERE movie_id = 105 AND actor_id = 7036;";

            DataTable dtExpected = new DataTable();
            dtExpected.Columns.Add("count", typeof(int));
            dtExpected.Rows.Add(new object[] { 1 });

            CheckAnswerForProblem("03", expectedRowsAffected, sqlVerify, dtExpected);
        }

        [TestMethod]
        public void ExerciseProblem04()
        {
            int expectedRowsAffected = 2;

            string sqlVerify = "SELECT (SELECT count(*) FROM genre WHERE genre_name = 'Sports') as genre_count, (SELECT count(*) FROM movie_genre WHERE genre_id IN (SELECT genre_id FROM genre WHERE genre_name = 'Sports') AND movie_id = 7214) as movie_genre_count;";

            DataTable dtExpected = new DataTable();
            dtExpected.Columns.Add("genre_count", typeof(int));
            dtExpected.Columns.Add("movie_genre_count", typeof(int));
            dtExpected.Rows.Add(new object[] { 1, 1 });

            CheckAnswerForProblem("04", expectedRowsAffected, sqlVerify, dtExpected);
        }

        [TestMethod]
        public void ExerciseProblem05()
        {
            int expectedRowsAffected = 1;

            string sqlVerify = "SELECT title FROM movie WHERE movie_id = 11;";

            DataTable dtExpected = new DataTable();
            dtExpected.Columns.Add("title");
            dtExpected.Rows.Add(new object[] { "Star Wars: A New Hope" });

            CheckAnswerForProblem("05", expectedRowsAffected, sqlVerify, dtExpected);
        }

        [TestMethod]
        public void ExerciseProblem06()
        {
            int expectedRowsAffected = 3;

            string sqlVerify = "SELECT overview FROM movie WHERE length_minutes > 210;";

            DataTable dtExpected = new DataTable();
            dtExpected.Columns.Add("overview");
            dtExpected.Rows.Add(new object[] { "A former Prohibition-era Jewish gangster returns to the Lower East Side of Manhattan over thirty years later, where he once again must confront the ghosts and regrets of his old life. This is a long movie." });
            dtExpected.Rows.Add(new object[] { "The story of British officer T.E. Lawrence's mission to aid the Arab tribes in their revolt against the Ottoman Empire during the First World War. Lawrence becomes a flamboyant, messianic figure in the cause of Arab unity but his psychological instability threatens to undermine his achievements. This is a long movie." });
            dtExpected.Rows.Add(new object[] { "Set in Bertolucci's ancestral region of Emilia, the film chronicles the lives of two men during the political turmoils that took place in Italy in the first half of the 20th century. This is a long movie." });

            CheckAnswerForProblem("06", expectedRowsAffected, sqlVerify, dtExpected);
        }

        [TestMethod]
        public void ExerciseProblem07()
        {
            int expectedRowsAffected = 14;

            string sqlVerify = "SELECT count(*) as actor_count FROM movie_actor WHERE movie_id = 299536;";

            DataTable dtExpected = new DataTable();
            dtExpected.Columns.Add("actor_count", typeof(int));
            dtExpected.Rows.Add(new object[] { 0 });

            CheckAnswerForProblem("07", expectedRowsAffected, sqlVerify, dtExpected);
        }

        [TestMethod]
        public void ExerciseProblem08()
        {
            int expectedRowsAffected = 6;

            string sqlVerify = "SELECT (SELECT count(*) FROM movie_actor WHERE actor_id = 37221) as actor_count, (SELECT count(*) FROM person WHERE person_id = 37221) as person_count;";

            DataTable dtExpected = new DataTable();
            dtExpected.Columns.Add("actor_count", typeof(int));
            dtExpected.Columns.Add("person_count", typeof(int));
            dtExpected.Rows.Add(new object[] { 0, 0 });

            CheckAnswerForProblem("08", expectedRowsAffected, sqlVerify, dtExpected);
        }

        [TestMethod]
        public void ExerciseProblem09()
        {
            int expectedRowsAffected = 16;

            string sqlVerify = "SELECT (SELECT count(*) FROM movie_actor WHERE movie_id = 77) as actor_count, (SELECT count(*) FROM movie_genre WHERE movie_id = 77) as genre_count, (SELECT count(*) FROM movie WHERE movie_id = 77) as movie_count;";

            DataTable dtExpected = new DataTable();
            dtExpected.Columns.Add("actor_count", typeof(int));
            dtExpected.Columns.Add("genre_count", typeof(int));
            dtExpected.Columns.Add("movie_count", typeof(int));
            dtExpected.Rows.Add(new object[] { 0, 0, 0 });

            CheckAnswerForProblem("09", expectedRowsAffected, sqlVerify, dtExpected);
        }

        [TestMethod]
        public void ExerciseProblem10()
        {
            int expectedRowsAffected = 9;

            string sqlVerify = "SELECT biography FROM person WHERE person_id IN (6, 130, 3799, 24343, 24590, 33185, 34027, 74296, 1230989);";

            DataTable dtExpected = new DataTable();
            dtExpected.Columns.Add("biography");
            dtExpected.Rows.Add(new object[] { $"Anthony Daniels (born 21 February 1946) is an English actor. He is best known for his role as the droid C-3PO in the Star Wars series of films made between 1977 and 2005. Description above from the Wikipedia article Anthony Daniels, licensed under CC-BY-SA, full list of contributors on Wikipedia.​ http://www.anthonydaniels.com/" });
            dtExpected.Rows.Add(new object[] { $"From Wikipedia, the free encyclopedia Kenneth George \"Kenny\" Baker (born 24 August 1934) was a British actor and musician, best known as the man inside R2-D2 in the popular Star Wars film series. Description above from the Wikipedia article Kenny Baker, licensed under CC-BY-SA, full list of contributors on Wikipedia. http://www.kennybaker.co.uk" });
            dtExpected.Rows.Add(new object[] { $"William December \"Billy Dee\" Williams Jr. (born April 6, 1937) is an American actor, voice actor, and artist. He is best known as Lando Calrissian in the Star Wars franchise, first in the early 1980s, and nearly forty years later in The Rise of Skywalker (2019), marking one of the longest intervals between onscreen portrayals of a character by the same actor in American film history. Description above from the Wikipedia article Billy Dee Williams, licensed under CC-BY-SA, full list of contributors on Wikipedia. http://www.bdwworldart.com/" });
            dtExpected.Rows.Add(new object[] { $"Peter Mayhew (19 May 1944-30 April 2019) was a British-American actor, best known for playing Chewbacca in the Star Wars films. Mayhew was born on 19 May 1944 in Barnes, Surrey, where he was also raised. His height was a product not of gigantism — \"I don't have the big head\" — but of an overactive pituitary gland secondary to a genetic disease called Marfan Syndrome. The overactive pituitary gland was treated successfully in his mid-teens. His peak height was 7 feet 3 inches (2.21 m) http://www.petermayhew.com/" });
            dtExpected.Rows.Add(new object[] { $"Dominique Sanda (born 11 March 1948) is a French actress and former fashion model. Sanda was born as Dominique Marie-Françoise Renée Varaigne in Paris to Lucienne (née Pinchon) and Gérard Sanda. She appeared in such noted European films of the 1970s as Vittorio de Sica's Il Giardino dei Finzi-Contini, Bernardo Bertolucci's The Conformist and Novecento, and Liliana Cavani's Beyond Good and Evil. She also appeared in The Mackintosh Man (with Paul Newman) and Steppenwolf (with Max von Sydow). She won the award for Best Actress at the 1976 Cannes Film Festival for her role in the film The Inheritance. Description above from the Wikipedia article Dominique Sanda, licensed under CC-BY-SA, full list of contributors on Wikipedia.​ http://www.dominiquesanda.com" });
            dtExpected.Rows.Add(new object[] { $"Jeremy Bulloch (16 February 1945 - 17 December 2020) was an English actor, best known for the role of the bounty hunter Boba Fett in the original Star Wars trilogy. He has appeared in numerous British television and film productions, including James Bond movies, Doctor Who and Robin of Sherwood. http://www.jeremybulloch.com/" });
            dtExpected.Rows.Add(new object[] { $"Stefania Sandrelli (born 5 June 1946 in Viareggio, Province of Lucca) is an Italian actress, famous for her many roles in the commedia all'Italiana, starting from 1960s. She was 15 years old when she starred in Divorce, Italian Style, as Marcello Mastroianni's cousin, Angela. She was born in Viareggio, Tuscany. She had a long relationship with Italian singer-songwriter Gino Paoli. Their daughter Amanda Sandrelli, born in 1964, is also an actress. Description above from the Wikipedia article Stefania Sandrelli, licensed under CC-BY-SA, full list of contributors on Wikipedia. http://www.stefaniasandrelli.net/" });
            dtExpected.Rows.Add(new object[] { $"From Wikipedia, the free encyclopedia. Teller (born Raymond Joseph Teller on February 14, 1948) is an American magician, illusionist, comedian, writer, and the frequently silent half of the comedy magic duo known as Penn & Teller, along with Penn Jillette. He is known for his advocacy of atheism, libertarianism, free-market economics, and scientific skepticism. He legally changed his name from \"Raymond Joseph Teller\" to just \"Teller\". He is an atheist, debunker, skeptic, and Fellow of the Cato Institute (a libertarian think-tank organization which also lists his partner Penn Jillette as a Fellow). The Cato Institute Association is featured prominently in the Penn and Teller Showtime TV series Bullshit!. Description above from the Wikipedia article Teller (entertainer), licensed under CC-BY-SA, full list of contributors on Wikipedia. http://pennandteller.net/" });
            dtExpected.Rows.Add(new object[] { $"Michael Vivian Fyfe Pennington is a British director and actor who, together with director Michael Bogdanov, founded the English Shakespeare Company. Although primarily a stage actor, he is best known to wider audiences for his role as Moff Jerjerrod, commanding officer of the Death Star in the film Star Wars Episode VI: Return of the Jedi and as Michael Foot in The Iron Lady, opposite Meryl Streep. http://www.michaelpennington.me.uk/" });

            CheckAnswerForProblem("10", expectedRowsAffected, sqlVerify, dtExpected);
        }

        [TestMethod]
        public void ExerciseProblem11()
        {
            int expectedRowsAffected = 2;

            string sqlVerify = "SELECT CASE WHEN director_id IS NOT NULL THEN 1 ELSE 0 END as director_set FROM movie WHERE movie_id = 367220;";

            DataTable dtExpected = new DataTable();
            dtExpected.Columns.Add("director_set", typeof(int));
            dtExpected.Rows.Add(new object[] { 1 });

            CheckAnswerForProblem("11", expectedRowsAffected, sqlVerify, dtExpected);
        }

        [TestMethod]
        public void ExerciseProblem12()
        {
            int expectedRowsAffected = 6;

            string sqlVerify = "SELECT (SELECT count(*) FROM collection WHERE collection_name = 'Bill Murray Collection') as collection_count, (SELECT count(*) FROM movie m JOIN movie_actor ma ON m.movie_id = ma.movie_id WHERE actor_id = 1532 AND collection_id = (SELECT collection_id FROM collection WHERE collection_name = 'Bill Murray Collection')) as movie_count;";

            DataTable dtExpected = new DataTable();
            dtExpected.Columns.Add("collection_count", typeof(int));
            dtExpected.Columns.Add("movie_count", typeof(int));
            dtExpected.Rows.Add(new object[] { 1, 5 });

            CheckAnswerForProblem("12", expectedRowsAffected, sqlVerify, dtExpected);
        }

        private void CheckAnswerForProblem(string problemNumber, int expectedRowsAffected, string sqlVerify, DataTable dtExpected)
        {
            string sqlActual = GetStudentQuery(problemNumber);

            sqlActual = Regex.Replace(sqlActual, "--[^\n]*(\n|$)", "");
            Assert.IsFalse(string.IsNullOrWhiteSpace(sqlActual), "No query found for this exercise. Make sure you've saved your changes to the exercise file.");

            int actualRowsAffected = ExecuteNonQuery(sqlActual);
            Assert.IsTrue(expectedRowsAffected == actualRowsAffected, "Your query didn't affect the expected number of rows.");

            DataTable dtVerify = LoadDataTable(sqlVerify);

            // compare expected and actual data
            CompareData(dtExpected, dtVerify);
        }

        private static void CompareData(DataTable dtExpected, DataTable dtVerify)
        {
            Assert.AreEqual(dtExpected.Rows.Count, dtVerify.Rows.Count, "Returned row count doesn't match expected results. Please check your SQL for errors.");

            for (int i = 0; i < dtExpected.Rows.Count; i++)
            {
                foreach (DataColumn dc in dtExpected.Columns)
                {
                    Assert.AreEqual(dtExpected.Rows[i][dc.ColumnName], dtVerify.Rows[i][dc.ColumnName], $"Data returned doesn't match data expected for row {i + 1} in column \"{dc.ColumnName}\"");
                }
            }
        }

        private string GetStudentQuery(string problemNumber)
        {
            string exerciseFile = $"{exerciseFiles.FirstOrDefault(f => f.StartsWith(problemNumber))}";
            if (exerciseFile == null)
            {
                Assert.Fail($"No exercise file found. Check that the file name begins with {problemNumber}.");
            }

            string exerciseFilePath = $"{exerciseFolder}/{exerciseFile}";
            if (!File.Exists(exerciseFilePath))
            {
                Assert.Fail("Exercise file doesn't exist.");
            }

            string sql = File.ReadAllText(exerciseFilePath);

            return sql;
        }

        private int ExecuteNonQuery(string sql)
        {
            int rowsAffected = -1;
            using (conn = new SqlConnection(movieConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected;
        }

        private DataTable LoadDataTable(string sql)
        {
            using (conn = new SqlConnection(movieConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader sdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(sdr);
                return dt;
            }
        }

        private static List<string> GetExerciseSqlFiles()
        {
            string folderToFind = "Exercises";
            string currPath = System.Environment.CurrentDirectory;
            List<string> exerFiles = new List<string>();

            if (currPath.Contains("\\"))
            {
                currPath = currPath.Replace("\\", "/");
            }

            while (exerFiles.Count == 0)
            {
                if (Directory.GetDirectories(currPath).Any(d => d.EndsWith(folderToFind)))
                {
                    currPath = Directory.GetDirectories(currPath).FirstOrDefault(d => d.EndsWith(folderToFind));
                    exerciseFolder = currPath.Replace("\\", "/");

                    if (Directory.GetFiles(exerciseFolder).Count(f => f.EndsWith(".sql")) > 0)
                    {
                        List<string> tempExerciseFiles = Directory.GetFiles(exerciseFolder).Where(f => f.EndsWith(".sql")).ToList();

                        // get just the filenames so that we don't have to hard code the exercise file names and can find just by number
                        foreach (string ef in tempExerciseFiles)
                        {
                            exerFiles.Add(ef.Replace(exerciseFolder, "").Replace("\\", "").Replace("/", ""));
                        }
                        break;
                    }
                }
                else
                {
                    if (currPath == "C:" || currPath == "/") //ran out of locations to check
                    {
                        break;
                    }

                    //go up one level
                    currPath = currPath.Substring(0, currPath.LastIndexOf("/"));
                }
            }

            return exerFiles;
        }

        private static void SetConnectionStrings(string defaultDbName)
        {
            string host = Environment.GetEnvironmentVariable("DB_HOST") ?? @".\SQLEXPRESS";
            string dbName = Environment.GetEnvironmentVariable("DB_DATABASE") ?? defaultDbName;
            string username = Environment.GetEnvironmentVariable("DB_USERNAME");
            string password = Environment.GetEnvironmentVariable("DB_PASSWORD");

            if (username != null && password != null)
            {
                mainConnectionString = $"Data Source={host};Initial Catalog=master;User Id={username};Password={password};";
                movieConnectionString = $"Data Source={host};Initial Catalog={dbName};User Id={username};Password={password};";
            }
            else
            {
                mainConnectionString = $"Data Source={host};Initial Catalog=master;Integrated Security=SSPI;";
                movieConnectionString = $"Data Source={host};Initial Catalog={dbName};Integrated Security=SSPI;";
            }
        }
    }
}
