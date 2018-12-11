CREATE TABLE [dbo].[Import]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Filename] NVARCHAR(255) NOT NULL, 
    [ULNsInFileCount] INT NOT NULL, 
    [NewULNsInFileCount] INT NOT NULL,
    [DateTime] DATETIME NOT NULL
)
