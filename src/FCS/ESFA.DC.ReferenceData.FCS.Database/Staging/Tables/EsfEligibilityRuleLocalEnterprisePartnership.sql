CREATE TABLE [staging].[EsfEligibilityRuleLocalEnterprisePartnership]
(
	[TenderSpecReference] [varchar](255) NOT NULL,
	[LotReference] [varchar](255) NOT NULL,
	[Code] [varchar](255) NOT NULL,
    CONSTRAINT [PK_Staging_EsfEligibilityRuleLocalEnterprisePartnership] PRIMARY KEY ([TenderSpecReference], [LotReference], [Code])
)


