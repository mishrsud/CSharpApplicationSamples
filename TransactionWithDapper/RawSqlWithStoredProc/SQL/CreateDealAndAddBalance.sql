SET TRANSACTION ISOLATION LEVEL REPEATABLE READ

  BEGIN TRY
	BEGIN TRAN UpdateBalanceTran
	DECLARE 
		@Amount decimal,
		@DealUserId uniqueidentifier,
		@ReceivedDate datetime,
		@PaidDate datetime,
		@UVA_GUID uniqueidentifier,
		@RD_GUID uniqueidentifier,
		@UVAD_Entered datetime,
		@UVAD_EnteredBy int,
		@UVAD_Updated datetime,
		@UVAD_UpdatedBy int,
		@UVAD_Status bit,
		@VirtualAccountId uniqueidentifier

	-- CREATE DEAL
	SET @RD_GUID = NEWID()
	INSERT INTO [dbo].[Deal]
							   ([DealId]
							   ,[Amount]
							   ,[DealUserId]
							   ,[ReceivedDate]
							   ,[PaidDate])
							VALUES
							   (@RD_GUID
							   ,@Amount
							   ,@DealUserId
							   ,@ReceivedDate
							   ,@PaidDate)

	-- LINK TO VA
	INSERT INTO [dbo].[U_VirtualAccountDeals]
							   ([UVAD_GUID]
							   ,[UVA_GUID]
							   ,[RD_GUID]
							   ,[UVAD_Entered]
							   ,[UVAD_EnteredBy]
							   ,[UVAD_Updated]
							   ,[UVAD_UpdatedBy]
							   ,[UVAD_Status])
							 VALUES
							   (
								NEWID()
							   ,@UVA_GUID
							   ,@RD_GUID
							   ,@UVAD_Entered
							   ,@UVAD_EnteredBy
							   ,@UVAD_Updated
							   ,@UVAD_UpdatedBy
							   ,@UVAD_Status
							  )

	-- UPDATE BALANCE
	IF EXISTS(SELECT 1 FROM [dbo].[VirtualAccountBalance] WHERE [VirtualAccountId] = @VirtualAccountId)
	BEGIN
		-- GET BALANCE
		DECLARE @CurrentBalance DECIMAL
		SELECT @CurrentBalance = [Amount] FROM [dbo].[VirtualAccountBalance] WHERE [VirtualAccountId] = @VirtualAccountId

		UPDATE [dbo].[VirtualAccountBalance] 
		SET [Amount] = (@Amount + @CurrentBalance), [Updated] = @UVAD_Updated, [UpdatedBy] = @UVAD_UpdatedBy
		WHERE [VirtualAccountId] = @VirtualAccountId
	END
	ELSE
	BEGIN 
		INSERT INTO [dbo].[VirtualAccountBalance]
						   ([VirtualAccountBalanceId]
						   ,[VirtualAccountId]
						   ,[Amount]
						   ,[Entered]
						   ,[EnteredBy]
						   ,[Updated]
						   ,[UpdatedBy])
							VALUES
						   (NEWID()
						   ,@VirtualAccountId
						   ,@Amount
						   ,@UVAD_Entered
						   ,@UVAD_EnteredBy
						   ,@UVAD_Updated
						   ,@UVAD_UpdatedBy)
	END

	COMMIT TRAN
  END TRY
  BEGIN CATCH
	DECLARE @ErrorNumber INT, @ErrorMessage NVARCHAR(1), @ErrorSeverity INT, @ErrorState INT
	SELECT @ErrorNumber = ERROR_NUMBER(), @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
	
	RAISERROR (@ErrorMessage, -- Message text.  
               @ErrorSeverity, -- Severity.  
               @ErrorState -- State.  
               ); 
  END CATCH
--TODO Update or Rollback