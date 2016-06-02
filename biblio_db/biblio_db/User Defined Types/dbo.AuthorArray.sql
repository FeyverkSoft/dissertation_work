CREATE TYPE [dbo].[AuthorArray] AS TABLE (
    [AuthorFamily]  NVARCHAR(512) NOT NULL, 
    [AuthorTrails]  NVARCHAR(512) NULL);