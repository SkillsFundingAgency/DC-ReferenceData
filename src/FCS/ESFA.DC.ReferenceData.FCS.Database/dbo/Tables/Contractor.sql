CREATE TABLE [dbo].[Contractor]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[OrganisationIdentifier] [nvarchar](100) NULL,
	[UKPRN] [int] NULL,
	[LegalName] [nvarchar](100) NULL, 
    CONSTRAINT [FK_Contractor_ToMasterContract] FOREIGN KEY ([Id]) REFERENCES [MasterContract]([Id])
)