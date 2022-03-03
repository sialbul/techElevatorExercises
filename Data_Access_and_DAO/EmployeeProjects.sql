-- Switch to the system (aka master) database
USE master;
GO

-- Delete the EmployeeProjects database if it exists
IF DB_ID('EmployeeProjects') IS NOT NULL
BEGIN
	ALTER DATABASE EmployeeProjects SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE EmployeeProjects;
END

-- Create a new EmployeeProjects database
CREATE DATABASE EmployeeProjects;
GO

-- Switch to the EmployeeProjects database
USE EmployeeProjects;
GO

BEGIN TRANSACTION;

CREATE TABLE department (
	department_id 	INT NOT NULL IDENTITY (1,1),
	name 			VARCHAR(40) UNIQUE NOT NULL,
	CONSTRAINT pk_department PRIMARY KEY (department_id)
);

CREATE TABLE employee (
	employee_id 	INT NOT NULL IDENTITY (1,1),
	department_id 	INT NOT NULL,
	first_name 		VARCHAR(20) NOT NULL,
	last_name 		VARCHAR(30) NOT NULL,
	birth_date 		DATE NOT NULL,
	hire_date 		DATE NOT NULL,
	CONSTRAINT pk_employee PRIMARY KEY (employee_id),
	CONSTRAINT fk_employee_department FOREIGN KEY (department_id) REFERENCES department(department_id)
);

CREATE TABLE project (
	project_id 	INT NOT NULL IDENTITY (1,1),
	name 		VARCHAR(40) UNIQUE NOT NULL,
	from_date 	DATE NOT NULL,
	to_date 	DATE NOT NULL,
	CONSTRAINT pk_project PRIMARY KEY (project_id)
);

CREATE TABLE project_employee (	
	project_id 	INT NOT NULL,
	employee_id INT NOT NULL,
	CONSTRAINT pk_project_employee PRIMARY KEY (project_id, employee_id),
	CONSTRAINT fk_project_employee_project FOREIGN KEY (project_id) REFERENCES project(project_id),
	CONSTRAINT fk_project_employee_employee FOREIGN KEY (employee_id) REFERENCES employee(employee_id)
);

CREATE TABLE timesheet (
    timesheet_id	INT NOT NULL IDENTITY (1,1),
    employee_id 	INT NOT NULL,
    project_id		INT NOT NULL,
    date_worked		DATE NOT NULL,
    hours_worked	DECIMAL(5,2) NOT NULL,
    is_billable		BIT NOT NULL,
    description 	VARCHAR(100),
    CONSTRAINT PK_timesheet PRIMARY KEY (timesheet_id),
    CONSTRAINT FK_timesheet_employee FOREIGN KEY (employee_id) REFERENCES employee(employee_id),
    CONSTRAINT FK_timesheet_project FOREIGN KEY (project_id) REFERENCES project(project_id)
);

-- Fill department and project before employee or project_employee because they have no foreign key dependencies
INSERT INTO department (name) VALUES ('Department of Redundancy');
INSERT INTO department (name) VALUES ('Network Administration');
INSERT INTO department (name) VALUES ('Research and Development');
INSERT INTO department (name) VALUES ('Store Support');

INSERT INTO project (name, from_date, to_date) VALUES ('Project X', '1961-03-01', '2002-08-31');
INSERT INTO project (name, from_date, to_date) VALUES ('Forlorn Cupcake', '2021-04-15', '2021-07-10');
INSERT INTO project (name, from_date, to_date) VALUES ('The Never-ending Project', '2010-09-01', '2038-01-19');
INSERT INTO project (name, from_date, to_date) VALUES ('Absolutely Done By', '07-21-2019', '2020-06-30');
INSERT INTO project (name, from_date, to_date) VALUES ('Royal Shakespeare', '2015-10-15', '2016-03-14');
INSERT INTO project (name, from_date, to_date) VALUES ('Plan 9', '2014-10-01', '2020-12-31');

INSERT INTO employee (department_id, first_name, last_name, birth_date, hire_date)
VALUES (1, 'Fredrick', 'Keppard', '1953-07-15', '2001-04-01');
INSERT INTO employee (department_id, first_name, last_name, birth_date, hire_date)
VALUES (1, 'Flo', 'Henderson', '1990-12-28', '2011-08-01');
INSERT INTO employee (department_id, first_name, last_name, birth_date, hire_date)
VALUES (2, 'Franklin', 'Trumbauer', '1980-07-14', '1998-09-01');
INSERT INTO employee (department_id, first_name, last_name, birth_date, hire_date)
VALUES (2, 'Delora', 'Coty', '1976-07-04', '2009-03-01');
INSERT INTO employee (department_id, first_name, last_name, birth_date, hire_date)
VALUES (2, 'Sid', 'Goodman', '1972-06-04', '1998-09-01');
INSERT INTO employee (department_id, first_name, last_name, birth_date, hire_date)
VALUES (3, 'Mary Lou', 'Wolinski', '1983-04-08', '2012-04-01');
INSERT INTO employee (department_id, first_name, last_name, birth_date, hire_date)
VALUES (3, 'Jammie', 'Mohl', '1987-11-11', '2013-02-16');
INSERT INTO employee (department_id, first_name, last_name, birth_date, hire_date)
VALUES (3, 'Neville', 'Zellers', '1983-04-04', '2013-07-01');
INSERT INTO employee (department_id, first_name, last_name, birth_date, hire_date)
VALUES (3, 'Meg', 'Buskirk', '1987-05-13', '2007-11-01');
INSERT INTO employee (department_id, first_name, last_name, birth_date, hire_date)
VALUES (3, 'Matthew', 'Duford', '1968-04-05', '2016-02-16');
INSERT INTO employee (department_id, first_name, last_name, birth_date, hire_date)
VALUES (4, 'Jarred', 'Lukach', '1981-09-25', '2003-05-01');
INSERT INTO employee (department_id, first_name, last_name, birth_date, hire_date)
VALUES (4, 'Gabreila', 'Christie', '1980-03-17', '1999-08-01');

INSERT INTO project_employee (project_id, employee_id) VALUES (1, 3);
INSERT INTO project_employee (project_id, employee_id) VALUES (1, 5);
INSERT INTO project_employee (project_id, employee_id) VALUES (3, 1);
INSERT INTO project_employee (project_id, employee_id) VALUES (3, 5);
INSERT INTO project_employee (project_id, employee_id) VALUES (3, 7);
INSERT INTO project_employee (project_id, employee_id) VALUES (4, 2);
INSERT INTO project_employee (project_id, employee_id) VALUES (4, 6);
INSERT INTO project_employee (project_id, employee_id) VALUES (5, 8);
INSERT INTO project_employee (project_id, employee_id) VALUES (5, 9);
INSERT INTO project_employee (project_id, employee_id) VALUES (6, 5);
INSERT INTO project_employee (project_id, employee_id) VALUES (6, 10);
INSERT INTO project_employee (project_id, employee_id) VALUES (6, 11);

COMMIT TRANSACTION;