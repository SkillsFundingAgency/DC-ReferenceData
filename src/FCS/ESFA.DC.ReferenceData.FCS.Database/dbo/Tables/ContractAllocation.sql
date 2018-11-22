CREATE TABLE [dbo].[ContractAllocation]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ContractAllocationNumber] NVARCHAR(20) NULL, 
    [FundingStreamCode] NVARCHAR(10) NULL,
    [Period] NVARCHAR(4) NULL, 
    [PeriodTypeCode] NVARCHAR(20) NULL, 
    [FundingStreamPeriodCode] NVARCHAR(20) NULL, 
    [LearningRatePremiumFactor] Decimal(10, 2) NULL, 
    [StartDate] DATETIME NULL, 
    [EndDate] DATETIME NULL, 
    [UoPCode] NVARCHAR(20) NULL, 
    [TenderSpecReference] NVARCHAR(100) NULL, 
    [LotReference] NVARCHAR(100) NULL, 
	[DeliveryUKPRN] INT NULL,
	[DeliveryOrganisation] NVARCHAR(100) NULL, 
	[TerminationDate] DateTime NULL, 
	[StopNewStartsFromDate] DateTime NULL, 
    [ContractId] INT NULL, 
    CONSTRAINT [FK_ContractAllocation_ToContract] FOREIGN KEY ([ContractId]) REFERENCES [Contract]([Id]) ON DELETE CASCADE
)
