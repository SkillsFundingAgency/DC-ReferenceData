CREATE TABLE [dbo].[ContractDeliverableCodeMapping]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FundingStreamPeriodCode] NVARCHAR(255) NULL, 
    [ExternalDeliverableCode] NVARCHAR(255) NULL, 
    [FCSDeliverableCode] NVARCHAR(255) NULL, 
    [DeliverableName] NVARCHAR(255) NULL, 
    [Claimable] BIT NULL,
)
