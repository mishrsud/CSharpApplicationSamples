USE [Bank]
GO

INSERT INTO [dbo].[Accounts]
           ([AccountGuid]
           ,[AccountNumber]
           ,[AccountNickname]
           ,[AccountStatus])
     VALUES
           (NEWID()
           ,'SAV2384283472'
           ,'Incense Stick'
           ,1)
GO


INSERT INTO [dbo].[Users]
           ([UserId]
           ,[UserGuid])
     VALUES
           (1
           ,NEWID())
GO

INSERT INTO [dbo].[AccountOwners]
           ([AccountId]
           ,[UserId])
     VALUES
           (1
           ,1)
GO


