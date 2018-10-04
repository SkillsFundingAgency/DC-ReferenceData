CREATE TABLE [dbo].[EsfEligibilityRuleLocalAuthority]
(
	[TenderSpecReference] [varchar](255) NOT NULL,
	[LotReference] [varchar](255) NOT NULL,
	[Code] [VARCHAR](255) NOT NULL
    CONSTRAINT [PK_EsfEligibilityRuleLocalAuthority] PRIMARY KEY ([TenderSpecReference], [LotReference], [Code])	,
    CONSTRAINT [FK_EsfEligibilityRuleLocalAuthorityToRule] FOREIGN KEY ([TenderSpecReference], [LotReference]) REFERENCES [EsfEligibilityRule]([TenderSpecReference], [LotReference]) ON DELETE CASCADE
)

