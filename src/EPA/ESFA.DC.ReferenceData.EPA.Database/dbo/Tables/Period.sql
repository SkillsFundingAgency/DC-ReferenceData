CREATE TABLE [dbo].[Period]
(
	[OrganisationId] NVARCHAR(50) NOT NULL , 
    [StandardCode] NVARCHAR(50) NOT NULL, 
    [EffectiveFrom] DATETIME NOT NULL, 
    [EffectiveTo] DATETIME NULL, 
    PRIMARY KEY ([OrganisationId], [StandardCode], [EffectiveFrom]), 
    CONSTRAINT [FK_Period_ToStandard] FOREIGN KEY ([OrganisationId], [StandardCode]) REFERENCES [Standard]([OrganisationId], [StandardCode])
)
