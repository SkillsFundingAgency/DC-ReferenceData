CREATE TABLE [dbo].[Contractor]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[OrganisationIdentifier] [nvarchar](100) NULL,
	[UKPRN] [int] NULL,
	[LegalName] [nvarchar](100) NULL
)