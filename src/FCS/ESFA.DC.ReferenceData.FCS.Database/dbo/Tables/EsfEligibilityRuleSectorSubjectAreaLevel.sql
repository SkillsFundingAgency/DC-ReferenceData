CREATE TABLE [dbo].[EsfEligibilityRuleSectorSubjectAreaLevel]
(
	[TenderSpecReference] [varchar](255) NOT NULL,
	[LotReference] [varchar](255) NOT NULL,
    [SectorSubjectAreaCode] [decimal](5, 2) NOT NULL,
	[MinLevelCode] [varchar](1) NULL,
	[MaxLevelCode] [varchar](1) NULL
	CONSTRAINT [FK_EsfEligibilityRuleSectorSubjectAreaLevelToRule] FOREIGN KEY ([TenderSpecReference], [LotReference]) REFERENCES [EsfEligibilityRule]([TenderSpecReference], [LotReference]) ON DELETE CASCADE, 
    CONSTRAINT [PK_EsfEligibilityRuleSectorSubjectAreaLevel] PRIMARY KEY ([SectorSubjectAreaCode], [TenderSpecReference], [LotReference])
)


