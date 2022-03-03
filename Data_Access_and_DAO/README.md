# Database connectivity (DAO) exercises

For this exercise, you'll be responsible for implementing the data access objects for a CLI application that manages information for employees, departments, and projects. The purpose of this exercise is to practice writing application code that interacts with a database.

## Learning objectives

After completing this exercise, you'll understand:

* How to create database connections.
* How to execute SQL statements and use parameters.
* How the DAO pattern encapsulates database access logic.

## Evaluation criteria and functional requirements

Your code will be evaluated based on the following criteria:

* The project must not have any build errors.
* The unit tests pass as expected.
* Code is clean, concise, and readable.

You may use the provided CLI application to test your code. However, your goal is to make sure the tests pass.

## Getting started

1. In the folder with this README, there's an `EmployeeProjects.sql` SQL script that drops and recreates the `EmployeeProjects` database. You can run that script to create a copy of the database to reference while you work. Be aware, however, that the tests don't use that database. The tests use a temporary database with the same structure. You'll find the SQL for that temporary database in `EmployeeProjects.Tests/test-data.sql`.
    - *Note that the `timesheet` table is not used in today's exercise.*
2. Open the `DaoExercise.sln` solution in Visual Studio.
3. Click on the **Test Explorer** tab to run the provided unit tests and see their results.

## Step One: Explore starting code and database schema

Before you begin, review the provided classes in the `Models` and `DAO` folders.

You'll write your code to complete the data access methods in the `DepartmentSqlDao`, `ProjectSqlDao`, and `EmployeeSqlDao` classes.

You should also familiarize yourself with the database schema either by looking at the database in SQL Server Management Studio or the `EmployeeProjects.sql` script.

## Step Two: Implement the `DepartmentSqlDao` methods

Complete the data access methods in `DepartmentSqlDao`. Refer to the documentation comments in the `IDepartmentDao` interface for the expected input and result of each method.

You can remove any existing `return` statement in the method when you start working on it.

After you complete this step, the tests in `DepartmentSqlDaoTests` pass.

## Step Three: Implement the `ProjectSqlDao` methods

Complete the data access methods in `ProjectSqlDao`. Refer to the documentation comments in the `IProjectDao` interface for the expected input and result of each method.

You can remove any existing `return` statement in the method when you start working on it.

After you complete this step, the tests in `ProjectSqlDaoTests` pass.

## Step Four: Implement the `EmployeeSqlDao` methods

Complete the data access methods in `EmployeeSqlDao`. Refer to the documentation comments in the `IEmployeeDao` interface for the expected input and result of each method.

You can remove any existing `return` statement in the method when you start working on it.

After you complete this step, the tests in `EmployeeSqlDaoTests` pass.
