CREATE TABLE [dbo].[EsfEligibilityRule]
(
	[TenderSpecReference] [varchar](255) NOT NULL,
	[LotReference] [varchar](255) NOT NULL,
	[MinAge] [int] NULL,
	[MaxAge] [int] NULL,
	[MinLengthOfUnemployment] [int] NULL,
	[MaxLengthOfUnemployment] [int] NULL,
	[MinPriorAttainment] [varchar](2) NULL,
	[MaxPriorAttainment] [varchar](2) NULL,
	[CalcMethod] [INT] NULL,
	[Benefits] [bit] NULL, 
    CONSTRAINT [PK_EsfEligibilityRule] PRIMARY KEY ([TenderSpecReference], [LotReference])
)
