use [aspnet-ToDoList-20180126091049]

go


--1. get all statuses, not repeating, alphabetically ordered
SELECT DISTINCT Status 
FROM tasks
ORDER BY Status


--2. get the count of all tasks in each project, order by tasks count descending
SELECT COUNT(*) AS Tasks
FROM tasks
GROUP BY ProjectId
ORDER BY Tasks


--3. get the count of all tasks in each project, order by projects names
SELECT COUNT(tasks.ProjectId) AS [Tasks count], Projects.Name AS [Project name]
FROM tasks, Projects
WHERE tasks.ProjectId = Projects.Id
GROUP BY tasks.ProjectId, Projects.Name
ORDER BY [Project name]


--4. get the tasks for all projects having the name beginning with “N” letter
SELECT *
FROM tasks
WHERE Name LIKE '[N]%' COLLATE Latin1_General_CS_AS


--5. get the list of all projects containing the ‘a’ letter in the middle of the name, and
--show the tasks count near each project. Mention that there can exist projects without
--tasks and tasks with project_id=NULL
SELECT Projects.Name AS [Project name],
	   (SELECT COUNT(*) FROM tasks
	   WHERE tasks.ProjectId = Projects.Id)
FROM Projects
WHERE Name LIKE '%[a-o]%' COLLATE Latin1_General_CS_AS


--6. get the list of tasks with duplicate names. Order alphabetically
SELECT tasks.Name AS [Name]
FROM tasks
GROUP BY [Name]
HAVING COUNT(*) > 1
ORDER BY Name


--7. get the list of tasks having several exact matches of both name and status, from
--the project ‘Garage’. Order by matches count
SELECT tasks.Name, tasks.Status
FROM tasks RIGHT JOIN projects
ON tasks.ProjectId = projects.Id
WHERE Projects.Name = 'Garage'
GROUP BY tasks.Name, tasks.Status
HAVING COUNT(tasks.Name) > 1
ORDER BY COUNT(tasks.Name)


--8. get the list of project names having more than 10 tasks in status ‘completed’. Order
--by project_id
--SELECT Projects.Name AS [Project name]
--FROM Projects LEFT JOIN	tasks
--ON tasks.ProjectId = Projects.Id
--WHERE tasks.Status = 'completed'
--GROUP BY Projects.Name
--HAVING COUNT(*) > 10

--* in my projecy column status is boolean, this query won't work

SELECT Projects.Name AS [Project name]
FROM Projects LEFT JOIN	tasks
ON tasks.ProjectId = Projects.Id
WHERE tasks.Status = 1
GROUP BY Projects.Name
HAVING COUNT(*) > 10