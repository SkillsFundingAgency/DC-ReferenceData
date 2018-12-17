CREATE VIEW [dbo].[UniqueLearnerNumbers]
AS 
SELECT [Uln] as [ULN]
      ,GETDATE() AS [Created_On]
      ,'DataLoader' AS [Created_By]
      ,GETDATE() AS [Modified_On]
      ,'DataLoader' AS [Modified_By]
  FROM [dbo].[UniqueLearnerNumber]
GO
