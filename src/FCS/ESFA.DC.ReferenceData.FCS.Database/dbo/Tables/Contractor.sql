CREATE TABLE [dbo].[Contractor]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[ContractNumber] [nvarchar](100) NULL,
	[ContractVersionNumber] [nvarchar](100) NULL,
	[OrganisationIdentifier] [nvarchar](100) NULL,
	[UKPRN] [int] NULL,
	[LegalName] [nvarchar](100) NULL
)