USE ClassroomLibrary;

-- UpdateAdminPIN
CREATE PROCEDURE UpdateAdminPIN
    @NewPIN CHAR(4)
AS
BEGIN
    UPDATE AdminSettings
    SET SettingValue = @NewPIN
    WHERE SettingName = 'AdminPIN';

    IF @@ROWCOUNT = 0
    BEGIN
        INSERT INTO AdminSettings (SettingName, SettingValue)
        VALUES ('AdminPIN', @NewPIN);
    END
END;

-- AuthenticateAdminPIN
CREATE PROCEDURE AuthenticateAdminPIN
    @InputPIN CHAR(4)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM AdminSettings WHERE SettingName = 'AdminPIN' AND SettingValue = @InputPIN)
        SELECT 1 AS Result;
    ELSE
        SELECT 0 AS Result;
END;

-- GetBookList
CREATE PROCEDURE GetBookList
AS
BEGIN
    SELECT 
        b.BookID,
        b.Title,
        a.AuthorID,
        a.FirstName,
        a.LastName
    FROM 
        Books b
        LEFT JOIN BookAuthors ba ON b.BookID = ba.BookID
        LEFT JOIN Authors a ON ba.AuthorID = a.AuthorID
    ORDER BY
        b.Title
END
