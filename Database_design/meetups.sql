USE master; --master is system database
GO -- FInish perivous statements before continue

DROP DATABASE IF EXISTS MeetUps;

CREATE DATABASE MeetUps;
GO
USE MeetUps;
GO



BEGIN TRANSACTION;

CREATE TABLE member
(
--column name		data type				constraints
member_id			int						identity(1,1),
lastName				nvarchar(64)		not null,
firstName			nvarchar(64)		not null,
email					nvarchar(64)		not null,
phone					varchar(11)			NULL,
dateOfBirth			date						not null,
reminderEmails	bit						not null

--additional constraints
constraint pk_member primary key (member_id)

);

CREATE TABLE interest_groups
(
group_id				int								identity(1,1),
group_name			nvarchar(100) 					UNIQUE,
constraint pk_interest_groups primary key (group_id),
)

CREATE TABLE event
(
event_id						int							identity(1,1),
eventName					nvarchar(100)			UNIQUE,
eventDescription			nvarchar(255)			null,
start_date_and_time	smalldatetime			not null,
event_duration				int							DEFAULT 30,
group_id						int							null,

constraint pk_event primary key (event_id),
constraint fk_event_groupNumber foreign key(group_id) references  interest_groups(group_id),
)

CREATE TABLE member_event
(
event_id						int					not null,
member_id					int					not null

constraint pk_member_event primary key (event_id, member_id),
constraint fk_member_event_event foreign key(event_id) references event(event_id),
constraint fk_member_event_member foreign key(member_id) references member(member_id)
)

CREATE TABLE member_group
(
group_id				int					not null,
member_id			int					not null

constraint pk_event_group primary key (group_id, member_id),
constraint fk_event_group_member foreign key(member_id) references member(member_id),
constraint fk_event_group_groupId foreign key(group_id	) references interest_groups(group_id),
)

COMMIT TRANSACTION;

INSERT INTO  interest_groups(group_name) VALUES('Save the birds'),('Save the turtles'),('Save the trees'),('Save the world');

INSERT INTO member (lastName, firstName, email, phone, dateOfBirth, reminderEmails) VALUES ('Pitt', 'Brad', 'bradpitt@email.com','2121234567', '1/1/1970', 1);
INSERT INTO member (lastName, firstName, email, phone, dateOfBirth, reminderEmails) VALUES('Jolie','Angelina','angelinajolie@email.com','2125454545','1/1/1971', 1);
INSERT INTO member (lastName, firstName, email, phone, dateOfBirth, reminderEmails) VALUES('Ford', 'Harrison', 'harrisonford@email.com','2123456789','1/1/1972',0);
INSERT INTO member (lastName, firstName, email, phone, dateOfBirth, reminderEmails) VALUES('Cruise', 'Tom', 'tomcruise@email.com','2123456789','1/1/1973',0);
INSERT INTO member (lastName, firstName, email, phone, dateOfBirth, reminderEmails) VALUES('Jackson','Michael','michaeljackson@email.com','2123242534','1/1/1974',1);
INSERT INTO member (lastName, firstName, email, phone, dateOfBirth, reminderEmails) VALUES('Bowie','David', 'davidbovie@email.com','2125678903','1/1/1975',0);
INSERT INTO member (lastName, firstName, email, phone, dateOfBirth, reminderEmails) VALUES('Paul','Rich','richpaul@email.com','2123214354','1/1/1976',1);
INSERT INTO member (lastName, firstName, email, phone, dateOfBirth, reminderEmails) VALUES('Morgan','Piers','piersmorgan@email.com','2123456789','1/1/1977',1);


INSERT INTO   event(eventName , eventDescription , start_date_and_time , event_duration , group_id) VALUES('Birds in OH', 'Birds types in OH and their habitat', '2022-02-11 1:00', '60',  (SELECT group_id  FROM interest_groups WHERE group_name='Save the birds'));
INSERT INTO   event(eventName , eventDescription , start_date_and_time , event_duration , group_id) VALUES('Turtles in OH', 'Turtle types in OH and their habitat', '2022-02-23 1:00', '60',  (SELECT group_id  FROM interest_groups WHERE group_name='Save the turtles'));
INSERT INTO   event(eventName , eventDescription , start_date_and_time , event_duration , group_id) VALUES('Trees in OH', 'Tree types in OH', '2022-02-25 3:00',  '30',  (SELECT group_id  FROM interest_groups WHERE group_name='Save the trees'));
INSERT INTO   event(eventName , eventDescription , start_date_and_time , event_duration , group_id) VALUES('Our lonely world', 'Polution in the world', '2022-02-24 1:00', '120',  (SELECT group_id  FROM interest_groups WHERE group_name='Save the world'));

INSERT INTO member_group(group_id,member_id) VALUES ((SELECT group_id  FROM interest_groups WHERE group_name='Save the birds'),(SELECT member_id FROM member WHERE lastName='Morgan'));
INSERT INTO member_group(group_id,member_id) VALUES((SELECT group_id  FROM interest_groups WHERE group_name='Save the birds'),(SELECT member_id FROM member WHERE lastName='Paul'));
INSERT INTO member_group(group_id,member_id) VALUES((SELECT group_id  FROM interest_groups WHERE group_name='Save the turtles'),(SELECT member_id FROM member WHERE lastName='Bowie'));
INSERT INTO member_group(group_id,member_id) VALUES((SELECT group_id  FROM interest_groups WHERE group_name='Save the turtles'),(SELECT member_id FROM member WHERE lastName='Jackson'));
INSERT INTO member_group(group_id,member_id) VALUES((SELECT group_id  FROM interest_groups WHERE group_name='Save the trees'),(SELECT member_id FROM member WHERE lastName='Cruise'));
INSERT INTO member_group(group_id,member_id) VALUES((SELECT group_id  FROM interest_groups WHERE group_name='Save the trees'),(SELECT member_id FROM member WHERE lastName='Pitt'));
INSERT INTO member_group(group_id,member_id) VALUES((SELECT group_id  FROM interest_groups WHERE group_name='Save the world'),(SELECT member_id FROM member WHERE lastName='Paul'));
INSERT INTO member_group(group_id,member_id) VALUES((SELECT group_id  FROM interest_groups WHERE group_name='Save the world'),(SELECT member_id FROM member WHERE lastName='Bowie'));

INSERT INTO member_event(event_id,member_id)
VALUES((SELECT event_id FROM event WHERE eventName='Turtles in OH'), (SELECT member_id FROM member WHERE lastName='Pitt'))
INSERT INTO member_event(event_id,member_id)
VALUES((SELECT event_id FROM event WHERE eventName='Turtles in OH'), (SELECT member_id FROM member WHERE lastName='Morgan'))
INSERT INTO member_event(event_id,member_id)
VALUES((SELECT event_id FROM event WHERE eventName='Birds in OH'), (SELECT member_id FROM member WHERE lastName='Paul'))
INSERT INTO member_event(event_id,member_id)
VALUES((SELECT event_id FROM event WHERE eventName='Birds in OH'), (SELECT member_id FROM member WHERE lastName='Bowie'))
INSERT INTO member_event(event_id,member_id)
VALUES((SELECT event_id FROM event WHERE eventName='Trees in OH'), (SELECT member_id FROM member WHERE lastName='Jackson'))
INSERT INTO member_event(event_id,member_id)
VALUES((SELECT event_id FROM event WHERE eventName='Trees in OH'), (SELECT member_id FROM member WHERE lastName='Cruise'))
INSERT INTO member_event(event_id,member_id)
VALUES((SELECT event_id FROM event WHERE eventName='Our lonely world'), (SELECT member_id FROM member WHERE lastName='Ford'))
INSERT INTO member_event(event_id,member_id)
VALUES((SELECT event_id FROM event WHERE eventName='Our lonely world'), (SELECT member_id FROM member WHERE lastName='Jolie'))


--- ------------------------------------------Queries to check tables------------------------------------------------------------------------------------------

SELECT * FROM interest_groups;
SELECT * FROM member;
SELECT * FROM event;

SELECT lastName, group_name FROM member 
JOIN member_group ON member.member_id=member_group.member_id
JOIN interest_groups ON member_group.group_id=interest_groups.group_id
JOIN event ON event.group_id = interest_groups.group_id
WHERE event.eventName =  'Our lonely world';

SELECT lastName FROM member 
JOIN member_event ON member.member_id= member_event.member_id
JOIN event ON member_event.event_id=event.event_id
WHERE event.eventName =  'Our lonely world';

SELECT eventName FROM event 
JOIN member_event ON event.event_id=member_event.event_id 
JOIN member ON member.member_id= member_event.member_id
WHERE member.lastName =  'Pitt';

--------------------------------------------------------------------------------------------------------------------------------------------------