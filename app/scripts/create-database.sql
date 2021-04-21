-- Create a database
CREATE DATABASE CodesanookEFNote;
GO

-- Switch to a created database
USE CodesanookEFNote;
GO

-- Create Notebooks table
CREATE TABLE [dbo].[Notebooks] (
    [Id] INT IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(64) NOT NULL,
    CONSTRAINT [PK_Notebooks_Id] PRIMARY KEY CLUSTERED ([Id])
);

-- Create a unique index
CREATE UNIQUE NONCLUSTERED INDEX [UX_Notebooks_Name] ON Notebooks([Name]);

-- Create Tags table
CREATE TABLE [dbo].[Tags] (
    [Id] INT IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(64) NOT NULL,
    CONSTRAINT [PK_Tags_Id] PRIMARY KEY CLUSTERED ([Id])
);

-- Create an unique index
CREATE UNIQUE NONCLUSTERED INDEX [UX_Tags_Name] ON Tags([Name]);

-- Create Notes table
CREATE TABLE [dbo].[Notes](
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Title] NVARCHAR(256) NOT NULL,
	[Content] NVARCHAR(MAX) NOT NULL,
	[CreatedUtc] DATETIME NOT NULL CONSTRAINT [DF_Notes_CreatedUtc] DEFAULT GETUTCDATE(), -- Default to UTC now
	[UpdatedUtc] DATETIME NULL,
	[IsDeleted] BIT NOT NULL CONSTRAINT [DF_Notes_IsDeleted] DEFAULT 0, -- Default to false
	[ViewCount] INT NOT NULL CONSTRAINT [DF_Notes_ViewCount] DEFAULT 0, -- Default to 0
	[NotebookId] INT NOT NULL,
    CONSTRAINT [PK_Notes_Id] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_Notes_NotebookId] FOREIGN KEY ([NotebookId]) REFERENCES Notebooks([Id]) -- Foreign key to Notebooks table
);

-- Create an index on foreign key NotebookId
CREATE NONCLUSTERED INDEX [IX_Notes_NotebookId] ON Notes(NotebookId);

-- Create an unique index
CREATE NONCLUSTERED INDEX [IX_Notes_Title] ON Notes([Title]);

CREATE TABLE [dbo].[NoteTags](
	[NoteId] INT NOT NULL,
	[TagId] INT NOT NULL,
    CONSTRAINT [PK_NoteTags_NoteIdTagId] PRIMARY KEY CLUSTERED ([NoteId], [TagId]), -- Composited primary key plus clustered index
	CONSTRAINT [FK_NoteTags_NoteId] FOREIGN KEY ([NoteId]) REFERENCES Notes([Id]), -- Foreign key to Notes table
	CONSTRAINT [FK_NoteTags_TagId] FOREIGN KEY ([TagId]) REFERENCES Tags([Id]) -- Foreign key to Tags table
);
