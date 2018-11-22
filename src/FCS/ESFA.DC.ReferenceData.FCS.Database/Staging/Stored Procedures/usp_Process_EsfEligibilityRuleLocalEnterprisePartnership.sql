CREATE PROCEDURE [Staging].[usp_Process_EsfEligibilityRuleLocalEnterprisePartnership]
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		
		MERGE INTO [dbo].[EsfEligibilityRuleLocalEnterprisePartnership] AS Target
		USING (
				SELECT   [TenderSpecReference]
						,[LotReference]
						,[Code]
				  FROM [Staging].[EsfEligibilityRuleLocalEnterprisePartnership]
			  )
			  AS Source 
		    ON Target.[TenderSpecReference] = Source.[TenderSpecReference]
	      AND Target.[LotReference] = Source.[LotReference]	
		  AND Target.[Code] = Source.[Code]			
		WHEN NOT MATCHED BY TARGET THEN
		INSERT (     [TenderSpecReference]
					,[LotReference]
					,[Code]
					)
			VALUES ( Source.[TenderSpecReference]
					,Source.[LotReference]
					,Source.[Code]
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
