Use Bank
GO



CREATE PROC GetAccountsForUser
(
	@UserId INT,
	@ActiveOnly BIT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT users.UserId, accounts.AccountId, accounts.AccountGuid, accounts.AccountNumber, accounts.AccountNickname, accounts.AccountStatus
	FROM dbo.Accounts accounts, dbo.AccountOwners accountOwners, dbo.Users users
	WHERE accounts.AccountId = accountOwners.AccountId
	AND accountOwners.UserId = users.UserId
	AND accounts.AccountStatus = @ActiveOnly
	AND users.UserId = @UserId

	SET NOCOUNT OFF;
END