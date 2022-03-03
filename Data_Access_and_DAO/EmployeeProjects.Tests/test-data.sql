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

INSERT INTO department (name)
VALUES ('Department 1'), -- department_id will be 1
       ('Department 2'); -- department_id will be 2

INSERT INTO project (name, from_date, to_date)
VALUES ('Project 1', '2000-01-02', '2000-12-31'), -- project_id will be 1
       ('Project 2', '2001-01-02', '2001-12-31'); -- project_id will be 2

INSERT INTO employee (department_id, first_name, last_name, birth_date, hire_date)
VALUES (1, 'First1', 'Last1', '1981-01-01', '2001-01-02'), -- employee_id will be 1
       (2, 'First2', 'Last2', '1982-02-01', '2002-02-03'), -- employee_id will be 2
       (1, 'First3', 'Last3', '1983-03-01', '2003-03-04'); -- employee_id will be 3

INSERT INTO project_employee (project_id, employee_id)
VALUES (1, 1),
       (2, 2), (2, 3); -- Don't assign Employee #4 to any project

COMMIT TRANSACTION;
