CREATE DATABASE ClassroomLibrary;
USE ClassroomLibrary;

CREATE TABLE Libraries (
	LibraryID INT PRIMARY KEY IDENTITY,
	LibraryCode VARCHAR(6) UNIQUE,
	LibraryName VARCHAR(50) NOT NULL
);

CREATE TABLE Students (
    StudentID INT PRIMARY KEY IDENTITY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
	LibraryID INT FOREIGN KEY REFERENCES Libraries(LibraryID)
);

CREATE TABLE Genres (
    GenreID INT PRIMARY KEY IDENTITY,
    GenreName VARCHAR(100) NOT NULL
);

CREATE TABLE Authors (
    AuthorID INT PRIMARY KEY IDENTITY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL
);

CREATE TABLE Illustrators (
    IllustratorID INT PRIMARY KEY IDENTITY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL
);

CREATE TABLE Books (
    BookID INT PRIMARY KEY IDENTITY,
    Title VARCHAR(255) NOT NULL,
    BookType VARCHAR(20) NOT NULL CHECK (BookType IN ('Informational', 'Fiction')),
    ISBN VARCHAR(20),
    GenreID INT FOREIGN KEY REFERENCES Genres(GenreID)
);

CREATE TABLE LibraryBooks (
    LibraryID INT FOREIGN KEY REFERENCES Libraries(LibraryID),
    BookID INT FOREIGN KEY REFERENCES Books(BookID),
    Quantity INT NOT NULL DEFAULT 1,
    PRIMARY KEY (LibraryID, BookID)
);

CREATE TABLE BookAuthors (
    PRIMARY KEY (BookID,AuthorID),
    BookID INT FOREIGN KEY REFERENCES Books(BookID),
    AuthorID INT FOREIGN KEY REFERENCES Authors(AuthorID)
);

CREATE TABLE BookIllustrators (
    PRIMARY KEY (BookID,IllustratorID),
    BookID INT FOREIGN KEY REFERENCES Books(BookID),
    IllustratorID INT FOREIGN KEY REFERENCES Illustrators(IllustratorID)
);

CREATE TABLE Checkouts (
    CheckoutID INT PRIMARY KEY IDENTITY,
    StudentID INT FOREIGN KEY REFERENCES Students(StudentID),
    BookID INT FOREIGN KEY REFERENCES Books(BookID),
	LibraryID INT FOREIGN KEY REFERENCES Libraries(LibraryID),
    FOREIGN KEY (LibraryID, BookID) REFERENCES LibraryBooks(LibraryID, BookID),
    CheckoutDate DATE NOT NULL,
    ReturnDate DATE
);

CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY,
    Username VARCHAR(50) UNIQUE NOT NULL,
    PasswordHash VARCHAR(60) NOT NULL,
    UserRole VARCHAR(20) NOT NULL CHECK (UserRole IN ('Admin'))
);