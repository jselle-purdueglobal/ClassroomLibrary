USE ClassroomLibrary;
GO

-- spGetUser
CREATE PROCEDURE spGetUser
    @Username NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

	SELECT Username, PasswordHash, UserRole
    FROM Users
    WHERE Username = @Username;
END

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
