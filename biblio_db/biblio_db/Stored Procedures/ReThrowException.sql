-- =============================================
-- Description:	Повторное возбуждение 
--              перехваченного исключения
-- =============================================
CREATE PROCEDURE [dbo].[ReThrowException] 
AS
BEGIN
    DECLARE @Number    Int;
    DECLARE @Severity  Int;
    DECLARE @State     Int;
    DECLARE @Message   NVarChar(4000);
    DECLARE @Procedure NVarChar(126);
    DECLARE @Line      Int;
    
    SELECT  @Number    = ERROR_NUMBER(),
            @Severity  = ERROR_SEVERITY(),
            @State     = ERROR_STATE(),
            @Message   = ERROR_MESSAGE(),
            @Procedure = ERROR_PROCEDURE(),
            @Line      = ERROR_LINE();

    IF @Number = 50000
    BEGIN
        RaisError(@Message, @Severity, @State);
        RETURN;
    END
    ELSE
    BEGIN
        IF (Len(@Procedure) > 0) SET @Message = @Message + ' in «' + @Procedure + '»';
        IF (@Line > 0) SET @Message = @Message + ', line ' + Cast(@Line AS NVarChar(32));
        RaisError(@Message, @Severity, 127); -- SERVER_ERROR
        RETURN;
    END
END