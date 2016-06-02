CREATE TABLE [dbo].[Record]
(
    [RecordId] BIGINT NOT NULL PRIMARY KEY, 
    [Title] NVARCHAR(1024) NULL, 
    [Face] NVARCHAR(MAX) NULL, 
    [YearOfPublishing] INT NULL, 
    [NumOfBooks] INT NULL, 
    [BookShelfSeat] NVARCHAR(512) NULL
)
