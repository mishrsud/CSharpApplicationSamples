create database Bank
go

use Bank
go

create Table Organisation
(
	OrgId INT IDENTITY(1,1) PRIMARY KEY NONCLUSTERED,
	OrgGuid UNIQUEIDENTIFIER NOT NULL DEFAULT(NEWID()),
	OrgName NVARCHAR(100) NOT NULL,
	OrgStatus BIT NOT NULL
)
go

create Table Accounts
(
	AccountId INT IDENTITY (1,1) PRIMARY KEY NONCLUSTERED,
	AccountGuid UNIQUEIDENTIFIER NOT NULL DEFAULT(NEWID()),
	AccountNumber NVARCHAR(50) NOT NULL,
	AccountNickname NVARCHAR(200),
	AccountStatus BIT NOT NULL DEFAULT(1)
)
go

-- THIS IS OUR BoundedContext's view of Users
create Table Users
(
	UserId INT PRIMARY KEY NONCLUSTERED,
	UserGuid UNIQUEIDENTIFIER
)
GO

create Table AccountOwners
(
	AccountId INT,
	UserId INT
	FOREIGN KEY (AccountId) REFERENCES Accounts(AccountId),
	FOREIGN KEY (UserId) REFERENCES Users(UserId),
	PRIMARY KEY (AccountId, UserId)
)
go

