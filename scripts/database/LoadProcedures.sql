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

--spDeleteStudent
CREATE OR ALTER PROCEDURE spDeleteStudent
    @StudentId Int
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Students
    SET IsArchived = 1
    WHERE StudentID = @StudentId;

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

-- spGetBooks
CREATE OR ALTER PROCEDURE spGetBooks
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        b.BookID,
        b.Title,
        b.ImagePath,
        a.AuthorID,
        a.FirstName,
        a.LastName,
        i.IllustratorID,
        i.FirstName,
        i.LastName
    FROM 
        Books b
    LEFT JOIN 
        BookAuthors ba ON b.BookID = ba.BookID
    LEFT JOIN 
        Authors a ON ba.AuthorID = a.AuthorID
    LEFT JOIN 
        BookIllustrators bi ON b.BookID = bi.BookID
    LEFT JOIN 
        Illustrators i ON bi.IllustratorID = i.IllustratorID
    ORDER BY
        b.Title, a.LastName, a.FirstName, i.LastName, i.FirstName;
END
GO

--spGetCheckouts
CREATE OR ALTER PROCEDURE spGetCheckouts
    @LibraryID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        c.CheckoutID,
        c.StudentID,
        c.BookID,
        c.CheckoutDate,
        c.ReturnDate
    FROM 
        Checkouts c
    WHERE 
        (@LibraryID IS NULL OR c.LibraryID = @LibraryID)
    ORDER BY 
        c.CheckoutDate DESC;
END
GO

--spAddCheckout
CREATE OR ALTER PROCEDURE spAddCheckout
    @StudentID INT,
    @BookID INT,
    @LibraryID INT,
    @CheckoutDate DATE
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO Checkouts (StudentID, BookID, LibraryID, CheckoutDate)
    VALUES (@StudentID, @BookID, @LibraryID, @CheckoutDate);

    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

--spGetLibraries
CREATE OR ALTER PROCEDURE spGetLibraries
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		LibraryID,
		LibraryCode,
		LibraryName
	FROM
		Libraries
END
GO

--spReturnCheckout
CREATE OR ALTER PROCEDURE spReturnCheckout
    @CheckoutId INT,
    @ReturnDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Checkouts 
    SET ReturnDate = @ReturnDate
    WHERE CheckoutID = @CheckoutId;

    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

