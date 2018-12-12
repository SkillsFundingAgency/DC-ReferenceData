CREATE TABLE [dbo].[Import]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Filename] NVARCHAR(255) NOT NULL, 
    [UlnsInFileCount] INT NOT NULL, 
    [NewUlnsInFileCount] INT NOT NULL,
    [StartDateTime] DATETIME NOT NULL, 
    [EndDateTime] DATETIME NULL
)
