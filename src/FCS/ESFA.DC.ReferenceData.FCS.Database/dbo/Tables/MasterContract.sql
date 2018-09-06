CREATE TABLE [dbo].[MasterContract]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ContractNumber] NVARCHAR(20) NULL, 
    [ContractVersionNumber] INT NULL, 
    [StartDate] DATETIME NULL, 
    [EndDate] DATETIME NULL
)
