CREATE TYPE [dbo].[StringDictionary] AS TABLE (
    [Key]   NVARCHAR (64)      NOT NULL,
    [Value] NVARCHAR (256) NOT NULL,
    PRIMARY KEY CLUSTERED ([Key] ASC));