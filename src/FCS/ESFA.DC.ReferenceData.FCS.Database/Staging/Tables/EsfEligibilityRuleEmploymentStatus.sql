CREATE TABLE [staging].[EsfEligibilityRuleEmploymentStatus]
(
	[TenderSpecReference] [varchar](255) NOT NULL,
	[LotReference] [varchar](255) NOT NULL,
	[Code] [int] NOT NULL
    CONSTRAINT [PK_Staging_EsfEligibilityRuleEmploymentStatus] PRIMARY KEY ([TenderSpecReference], [LotReference], [Code])
)
