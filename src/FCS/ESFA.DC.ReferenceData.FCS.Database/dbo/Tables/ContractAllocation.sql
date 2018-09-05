CREATE TABLE [dbo].[ContractAllocation]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ContractAllocationNumber] NVARCHAR(20) NULL, 
    [FundingStreamCode] NVARCHAR(10) NULL,
    [Period] NVARCHAR(4) NULL, 
    [PeriodTypeCode] NVARCHAR(20) NULL, 
    [FundingStreamPeriodCode] NVARCHAR(20) NULL, 
    [StartDate] DATETIME NULL, 
    [EndDate] DATETIME NULL, 
    [UoPCode] NVARCHAR(20) NULL, 
    [ContractId] INT NULL, 
    CONSTRAINT [FK_ContractAllocation_ToContract] FOREIGN KEY ([ContractId]) REFERENCES [Contract]([Id])
)
