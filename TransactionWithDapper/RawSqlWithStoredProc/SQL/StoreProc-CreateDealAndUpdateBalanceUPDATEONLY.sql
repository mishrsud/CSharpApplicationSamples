CREATE PROC dbo.CreateDealAndHoldBalance
(
		@Amount decimal,
		@DealUserId uniqueidentifier,
		@ReceivedDate datetime,
		@PaidDate datetime,
		@UVA_GUID uniqueidentifier,
		@UVAD_Entered datetime,
		@UVAD_EnteredBy int,
		@UVAD_Updated datetime,
		@UVAD_UpdatedBy int
)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL REPEATABLE READ
		BEGIN TRAN UpdateBalanceTran
		DECLARE @RD_GUID uniqueidentifier, @UVAD_Status bit

		-- CREATE DEAL
		SET @RD_GUID = NEWID()		
		INSERT INTO [dbo].[Deal] ([DealId],[Amount],[DealUserId],[ReceivedDate],[PaidDate])
		VALUES (@RD_GUID, @Amount, @DealUserId, @ReceivedDate, @PaidDate)

		-- LINK TO VA
		SET @UVAD_Status = 1
		INSERT INTO [dbo].[U_VirtualAccountDeals] ([UVAD_GUID],[UVA_GUID],[RD_GUID],[UVAD_Entered],[UVAD_EnteredBy],[UVAD_Updated],[UVAD_UpdatedBy],[UVAD_Status])
		VALUES (NEWID(), @UVA_GUID, @RD_GUID, @UVAD_Entered, @UVAD_EnteredBy, @UVAD_Updated, @UVAD_UpdatedBy, @UVAD_Status)

		 --UPDATE BALANCE
		UPDATE dbo.VirtualAccountBalance SET Amount = Amount + @Amount WHERE VirtualAccountId = @UVA_GUID

		COMMIT TRAN
END