CREATE PROCEDURE [Internal].[AddAuthor]
    @AuthorFamily   NVARCHAR(512) NULL, 
    @AuthorTrails   NVARCHAR(512) NULL, 
    @AuthorId       INT OUTPUT 
AS
BEGIN
    DECLARE @ProcName VARCHAR(64);
    SELECT  @ProcName = '['+OBJECT_SCHEMA_NAME(@@PROCID)+'].['+OBJECT_NAME(@@PROCID) + ']: ';

    DECLARE @TranCount Int;
    SET @TranCount = @@TranCount;
            
    SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
    IF (@TranCount = 0)
    BEGIN
        BEGIN TRANSACTION;
    END
    ELSE BEGIN
        SAVE TRANSACTION @ProcName;
    END
    BEGIN TRY

    SELECT @AuthorId = AuthorId 
    FROM [dbo].[Author] 
    WHERE AuthorFamily = @AuthorFamily AND AuthorTrails = @AuthorTrails

    IF @@ROWCOUNT <> 1
        BEGIN
            INSERT INTO [dbo].[Author](AuthorFamily, AuthorTrails)
            VALUES(@AuthorFamily, @AuthorTrails)
            SET @AuthorId = @@IDENTITY;
        END


        IF (@@TranCount > @TranCount) 
            COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF (@@TranCount > 0)
        BEGIN
            IF (XACT_STATE() <> -1)
            BEGIN
                IF (@@TranCount = @TranCount)
                    ROLLBACK TRANSACTION @ProcName;
                ELSE
                    ROLLBACK TRANSACTION;
                END 
        END
        EXEC ReThrowException; 
    END CATCH
END