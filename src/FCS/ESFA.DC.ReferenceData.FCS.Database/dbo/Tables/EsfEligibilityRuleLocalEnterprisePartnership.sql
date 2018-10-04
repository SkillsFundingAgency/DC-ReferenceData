﻿CREATE TABLE [dbo].[EsfEligibilityRuleLocalEnterprisePartnership]
(
	[TenderSpecReference] [varchar](255) NOT NULL,
	[LotReference] [varchar](255) NOT NULL,
	[Code] [varchar](255) NOT NULL,
    CONSTRAINT [PK_EsfEligibilityRuleLocalEnterprisePartnership] PRIMARY KEY ([TenderSpecReference], [LotReference], [Code]),
    CONSTRAINT [FK_EsfEligibilityRuleLocalEnterprisePartnershipToRule] FOREIGN KEY ([TenderSpecReference], [LotReference]) REFERENCES [EsfEligibilityRule]([TenderSpecReference], [LotReference]) ON DELETE CASCADE
)


