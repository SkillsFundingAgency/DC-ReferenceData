CREATE TABLE [dbo].[EsfEligibilityRuleEmploymentStatus]
(
	[TenderSpecReference] [varchar](255) NOT NULL,
	[LotReference] [varchar](255) NOT NULL,
	[Code] [int] NOT NULL
    CONSTRAINT [PK_EsfEligibilityRuleEmploymentStatus] PRIMARY KEY ([TenderSpecReference], [LotReference], [Code]), 
    CONSTRAINT [FK_EsfEligibilityRuleEmploymentStatusToRule] FOREIGN KEY ([TenderSpecReference], [LotReference]) REFERENCES [EsfEligibilityRule]([TenderSpecReference], [LotReference]) ON DELETE CASCADE

)
