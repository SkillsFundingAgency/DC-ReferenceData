CREATE TABLE [dbo].[ContractDeliverable]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Description] NVARCHAR(255) NULL, 
    [DeliverableCode] INT NULL, 
    [UnitCode] DECIMAL(13, 2) NULL,
    [UnitCost] DECIMAL(13, 2) NULL,
    [PlannedVolume] INT NULL, 
    [PlannedValue] DECIMAL(13, 2) NULL, 
    [ContractAllocationId] INT NULL, 
    CONSTRAINT [FK_ContractDeliverable_ToContractAllocation] FOREIGN KEY ([ContractAllocationId]) REFERENCES [ContractAllocation]([Id]) ON DELETE CASCADE
)
