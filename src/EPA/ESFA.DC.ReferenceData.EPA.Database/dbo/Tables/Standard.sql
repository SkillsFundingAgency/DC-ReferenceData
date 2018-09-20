CREATE TABLE [dbo].[Standard]
(
	[OrganisationId] NVARCHAR(50) NOT NULL , 
    [StandardCode] NVARCHAR(50) NOT NULL, 
    PRIMARY KEY ([OrganisationId], [StandardCode]), 
    CONSTRAINT [FK_Standard_ToOrganisation] FOREIGN KEY ([OrganisationId]) REFERENCES [Organisation]([Id])
)
