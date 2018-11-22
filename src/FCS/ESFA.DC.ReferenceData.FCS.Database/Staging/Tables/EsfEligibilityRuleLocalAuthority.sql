CREATE TABLE [staging].[EsfEligibilityRuleLocalAuthority]
(
	[TenderSpecReference] [varchar](255) NOT NULL,
	[LotReference] [varchar](255) NOT NULL,
	[Code] [VARCHAR](255) NOT NULL
    CONSTRAINT [PK_Staging_EsfEligibilityRuleLocalAuthority] PRIMARY KEY ([TenderSpecReference], [LotReference], [Code])
)

