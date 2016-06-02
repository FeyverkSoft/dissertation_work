CREATE TABLE [dbo].[Author]
(
    [AuthorId]      BIGINT NOT NULL PRIMARY KEY, 
    [AuthorFamily]  NVARCHAR(512) NULL, 
    [AuthorTrails]  NVARCHAR(512) NULL, 
    [Alias]         NVARCHAR(512) NULL
)
