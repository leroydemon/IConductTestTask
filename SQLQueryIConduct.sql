CREATE TABLE dbo.Employee
(
    Id        INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name]    NVARCHAR(100)     NOT NULL,
    ManagerId INT               NULL,
    [Enable]  BIT               NOT NULL,
);

CREATE PROCEDURE dbo.GetEmployeeTreeById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH EmployeeTree AS
    (
        SELECT  Id,
                [Name],
                ManagerId,
                [Enable]
        FROM dbo.Employee
        WHERE Id = @Id

        UNION ALL

        SELECT  e.Id,
                e.[Name],
                e.ManagerId,
                e.[Enable]
        FROM dbo.Employee e
        INNER JOIN EmployeeTree et
            ON e.ManagerId = et.Id
    )
    SELECT  Id,
            [Name],
            ManagerId,
            [Enable]
    FROM EmployeeTree;
END;
GO

CREATE PROCEDURE dbo.SetEmployeeEnable
    @Id     INT,
    @Enable BIT
AS
BEGIN
    UPDATE dbo.Employee
    SET [Enable] = @Enable
    WHERE Id = @Id;
END;
GO


CREATE NONCLUSTERED INDEX IX_Employee_ManagerId_Enable
ON dbo.Employee (ManagerId, [Enable])

INSERT INTO dbo.Employee ([Name], ManagerId, [Enable]) VALUES
    (N'CEO',                NULL, 1),  
    (N'Head of Development', 1,   1),   
    (N'Head of Sales',       1,   1),   
    (N'Developer 1',         2,   1),  
    (N'Developer 2',         2,   0),  
    (N'QA Engineer',         2,   1),   
    (N'Sales Manager 1',     3,   1),   
    (N'Sales Manager 2',     3,   1), 
    (N'Intern Dev',          4,   1),  
    (N'Junior Sales',        7,   0);  

Select * from dbo.Employee

DROP TABLE dbo.Employee

DROP PROCEDURE dbo.GetEmployeeTreeById

DROP PROCEDURE dbo.SetEmployeeEnable
