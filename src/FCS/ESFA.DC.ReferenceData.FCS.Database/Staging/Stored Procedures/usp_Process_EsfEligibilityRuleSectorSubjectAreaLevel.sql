CREATE PROCEDURE [Staging].[usp_Process_EsfEligibilityRuleSectorSubjectAreaLevel]
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		
		MERGE INTO [dbo].[EsfEligibilityRuleSectorSubjectAreaLevel] AS Target
		USING (
				SELECT   [TenderSpecReference]
						,[LotReference]
						,[SectorSubjectAreaCode]
						,[MinLevelCode]
						,[MaxLevelCode]
				  FROM [Staging].[EsfEligibilityRuleSectorSubjectAreaLevel]
			  )
			  AS Source 
		    ON Target.[TenderSpecReference] = Source.[TenderSpecReference]
	      AND Target.[LotReference] = Source.[LotReference]	
			WHEN MATCHED 
				AND EXISTS 
					(	SELECT 
							 Target.[SectorSubjectAreaCode]
							,Target.[MinLevelCode]
							,Target.[MaxLevelCode]
					EXCEPT 
						SELECT 
							 Source.[SectorSubjectAreaCode]
							,Source.[MinLevelCode]
							,Source.[MaxLevelCode]
					)
		  THEN
			UPDATE SET   [SectorSubjectAreaCode] = Source.[SectorSubjectAreaCode]
						,[MinLevelCode] = Source.[MinLevelCode]
						,[MaxLevelCode] = Source.[MaxLevelCode]
		WHEN NOT MATCHED BY TARGET THEN
		INSERT (     [TenderSpecReference]
					,[LotReference]
					,[SectorSubjectAreaCode]
					,[MinLevelCode]
					,[MaxLevelCode]
					)
			VALUES ( Source.[TenderSpecReference]
					,Source.[LotReference]
					,Source.[SectorSubjectAreaCode]
					,Source.[MinLevelCode]
					,Source.[MaxLevelCode]
				  )
		WHEN NOT MATCHED BY SOURCE THEN DELETE
		;

		RETURN 0;

	END TRY
-- 
-------------------------------------------------------------------------------------- 
-- Handle any problems
-------------------------------------------------------------------------------------- 
-- 
	BEGIN CATCH

		DECLARE   @ErrorMessage		NVARCHAR(4000)
				, @ErrorSeverity	INT 
				, @ErrorState		INT
				, @ErrorNumber		INT
						
		SELECT	  @ErrorNumber		= ERROR_NUMBER()
				, @ErrorMessage		= 'Error in :' + ISNULL(OBJECT_NAME(@@PROCID),'') + ' - Error was :' + ERROR_MESSAGE()
				, @ErrorSeverity	= ERROR_SEVERITY()
				, @ErrorState		= ERROR_STATE();
	
		RAISERROR (
					  @ErrorMessage		-- Message text.
					, @ErrorSeverity	-- Severity.
					, @ErrorState		-- State.
				  );
			  
		RETURN @ErrorNumber;

	END CATCH
-- 
-------------------------------------------------------------------------------------- 
-- All done
-------------------------------------------------------------------------------------- 
-- 
END
