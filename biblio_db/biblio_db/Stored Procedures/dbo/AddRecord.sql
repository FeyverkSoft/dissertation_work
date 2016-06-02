CREATE PROCEDURE [Record].[AddRecord]
    @Title              NVARCHAR(1024), 
    @Face               NVARCHAR(MAX), 
    @YearOfPublishing   INT NULL, 
    @NumOfBooks         INT NULL, 
    @BookShelfSeat      NVARCHAR(512) NULL,
    @Authors            AuthorArray readonly
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

    DECLARE @RecordId BIGINT;--Идентификатор записи

    --Пока что проверять на наличие такой же записи не будем

    INSERT INTO [dbo].[Record](Title, Face, YearOfPublishing, NumOfBooks, BookShelfSeat)
    VALUES(@Title, @Face, @YearOfPublishing, @NumOfBooks, @BookShelfSeat)
    SET @RecordId = @@IDENTITY;

    --Курсор для перебора авторов и добавления связки автор-запись
    DECLARE CUR CURSOR FOR
        SELECT * FROM @Authors --выбираем из таблицы автор
    OPEN CUR -- открываем курсор
    DECLARE @AuthorFamily NVARCHAR(256), @AuthorTrails NVARCHAR(256)

    FETCH NEXT FROM CUR 
    INTO @AuthorFamily, @AuthorTrails
    WHILE @@FETCH_STATUS = 0
        BEGIN

            DECLARE @AuthorId INT;

            EXEC [Internal].[AddAuthor] --добавляем автора в базу, если его небыло
                @AuthorFamily   = @AuthorFamily,
                @AuthorTrails   = @AuthorTrails, 
                @AuthorId       = @AuthorId OUTPUT 

            INSERT INTO [dbo].[AuthorRecord](AuthorId, RecordId)-- вставляем связку автор-запись
            VALUES (@AuthorId, @RecordId)

        END
    CLOSE CUR;
    DEALLOCATE CUR;

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