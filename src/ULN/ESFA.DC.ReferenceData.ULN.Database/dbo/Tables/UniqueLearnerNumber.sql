CREATE TABLE [dbo].[UniqueLearnerNumber] (
    [ULN] BIGINT NOT NULL PRIMARY KEY, 
    [ImportId] INT NOT NULL, 
    CONSTRAINT [FK_UniqueLearnerNumber_ToImport] FOREIGN KEY ([ImportId]) REFERENCES [Import]([Id])
);

