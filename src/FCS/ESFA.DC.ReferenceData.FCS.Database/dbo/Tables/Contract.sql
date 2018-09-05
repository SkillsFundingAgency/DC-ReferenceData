CREATE TABLE [dbo].[Contract]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [ContractNumber] NVARCHAR(20) NULL, 
    [ContractVersionNumber] INT NULL, 
    [StartDate] DATETIME NULL, 
    [EndDate] DATETIME NULL, 
    [ContractorId] INT NOT NULL, 
    CONSTRAINT [FK_Contract_ToContractor] FOREIGN KEY ([ContractorId]) REFERENCES [Contract]([Id])
)
