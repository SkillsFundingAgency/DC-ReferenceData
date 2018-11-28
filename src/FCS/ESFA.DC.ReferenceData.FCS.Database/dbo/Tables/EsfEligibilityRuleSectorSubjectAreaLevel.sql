CREATE TABLE [dbo].[EsfEligibilityRuleSectorSubjectAreaLevel]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[TenderSpecReference] [varchar](255) NULL,
	[LotReference] [varchar](255) NULL,
    [SectorSubjectAreaCode] [decimal](5, 2) NULL,
	[MinLevelCode] [varchar](1) NULL,
	[MaxLevelCode] [varchar](1) NULL,
	CONSTRAINT [FK_EsfEligibilityRuleSectorSubjectAreaLevelToRule] FOREIGN KEY ([TenderSpecReference], [LotReference]) REFERENCES [EsfEligibilityRule]([TenderSpecReference], [LotReference]) ON DELETE CASCADE
)


