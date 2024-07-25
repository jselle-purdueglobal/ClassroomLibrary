USE ClassroomLibrary;
GO

-- spGetUser
CREATE PROCEDURE spGetUser
    @Username NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

	SELECT Username, PasswordHash, UserRole, IsFirstLogin, LibraryID
    FROM Users
    WHERE Username = @Username;
END
GO

-- spGetStudent
CREATE OR ALTER PROCEDURE spGetStudent
    @StudentId INT
AS
BEGIN
    SET NOCOUNT ON;

	SELECT StudentID, FirstName, LastName, LibraryID
    FROM Students
    WHERE StudentID = @StudentId;
END
GO

--spGetLibraryStudents
CREATE PROCEDURE spGetLibraryStudents
    @LibraryID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        StudentID,
        FirstName,
        LastName
    FROM 
        Students
    WHERE 
        LibraryID = @LibraryID
    ORDER BY 
        LastName, FirstName;
END
GO

-- spGetUserByRefreshToken
CREATE PROCEDURE spGetUserByRefreshToken
    @RefreshToken NVARCHAR(255)
AS
BEGIN
    SELECT Username, PasswordHash, UserRole, IsFirstLogin, RefreshToken
    FROM Users
    WHERE RefreshToken = @RefreshToken
END
GO 

-- spSaveRefreshToken
CREATE PROCEDURE spSaveRefreshToken
    @Username NVARCHAR(50),
    @RefreshToken NVARCHAR(255),
    @ExpiryTime DATETIME
AS
BEGIN
    UPDATE Users
    SET RefreshToken = @RefreshToken,
        RefreshTokenExpiryTime = @ExpiryTime
    WHERE Username = @Username;
END
GO 

-- spUpdateUserPassword
CREATE PROCEDURE spUpdateUserPassword
    @Username NVARCHAR(50),
    @NewPasswordHash NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Users
    SET PasswordHash = @NewPasswordHash,
        IsFirstLogin = 0
    WHERE Username = @Username;
    
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

--spUpdateStudentName
CREATE PROCEDURE spUpdateStudentName
    @StudentID INT,
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Students
    SET FirstName = @FirstName,
        LastName = @LastName
    WHERE StudentID = @StudentID;

    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

--spDeleteStudents
CREATE PROCEDURE spDeleteStudents
    @StudentIds IntList READONLY
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Students
    WHERE StudentId IN (SELECT Value FROM @StudentIds);

    SELECT @@ROWCOUNT AS DeletedCount;
END
GO

--spAddStudent
CREATE OR ALTER PROCEDURE spAddStudent
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @LibraryId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Students (FirstName, LastName, LibraryId)
    VALUES (@FirstName, @LastName, @LibraryId);
    
    SELECT SCOPE_IDENTITY() AS NewStudentId;
END
GO

-- GetBookList
CREATE PROCEDURE GetBookList
AS
BEGIN
    SELECT 
        b.BookID,
        b.Title,
        STRING_AGG(a.FirstName + ' ' + a.LastName, ', ') AS Authors
    FROM 
        Books b
        LEFT JOIN BookAuthors ba ON b.BookID = ba.BookID
        LEFT JOIN Authors a ON ba.AuthorID = a.AuthorID
    GROUP BY
        b.BookID,
        b.Title
    ORDER BY
        b.Title
END
GO
