CREATE PROCEDURE [dbo].[GetRawDocList]
    @Count      BIGINT
AS
BEGIN
    SELECT TOP(@Count) * FROM [dbo].[Raws] r
    --order by NEWID()
END