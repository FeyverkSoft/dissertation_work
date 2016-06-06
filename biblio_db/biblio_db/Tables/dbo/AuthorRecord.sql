CREATE TABLE [dbo].[AuthorRecord]
(
    [AuthorRecordId] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [AuthorId] BIGINT NOT NULL, 
    [RecordId] BIGINT NOT NULL, 
);
GO

ALTER TABLE [dbo].[AuthorRecord] ADD 
    CONSTRAINT [FK_AuthorRecord_Author] FOREIGN KEY (AuthorId) REFERENCES [dbo].[Author](AuthorId);
GO
ALTER TABLE [dbo].[AuthorRecord] ADD 
    CONSTRAINT [FK_AuthorRecord_Record] FOREIGN KEY (RecordId) REFERENCES [dbo].[Record](RecordId);