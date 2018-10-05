CREATE PROCEDURE [Staging].[usp_Process_EsfEligibilityRule]
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
	
		MERGE INTO [dbo].[EsfEligibilityRule] AS Target
		USING (
				SELECT   [TenderSpecReference] 
						,[LotReference] 
						,[MinAge] 
						,[MaxAge] 
						,[MinLengthOfUnemployment] 
						,[MaxLengthOfUnemployment] 
						,[MinPriorAttainment] 
						,[MaxPriorAttainment] 
						,[CalcMethod] 
					    ,[Benefits] 
				  FROM [Staging].[EsfEligibilityRule]
			  )
			  AS Source 
		    ON Target.[TenderSpecReference] = Source.[TenderSpecReference]
	      AND Target.[LotReference] = Source.[LotReference]	
			WHEN MATCHED 
				AND EXISTS 
					(	SELECT 
							 Target.[MinAge]
							,Target.[MaxAge]
							,Target.[MinLengthOfUnemployment]
							,Target.[MaxLengthOfUnemployment]
							,Target.[MinPriorAttainment]
							,Target.[MaxPriorAttainment]
							,Target.[CalcMethod]
							,Target.[Benefits]
					EXCEPT 
						SELECT 
							 Source.[MinAge]
							,Source.[MaxAge]
							,Source.[MinLengthOfUnemployment]
							,Source.[MaxLengthOfUnemployment]
							,Source.[MinPriorAttainment]
							,Source.[MaxPriorAttainment]
							,Source.[CalcMethod]
							,Source.[Benefits]
					)
		  THEN
			UPDATE SET   [MinAge] = Source.[MinAge]
						,[MaxAge] = Source.[MaxAge]
						,[MinLengthOfUnemployment] = Source.[MinLengthOfUnemployment]
						,[MaxLengthOfUnemployment] = Source.[MaxLengthOfUnemployment]
						,[MinPriorAttainment] = Source.[MinPriorAttainment]
						,[MaxPriorAttainment] = Source.[MaxPriorAttainment]
						,[CalcMethod] = Source.[CalcMethod]
						,[Benefits] = Source.[Benefits]
		WHEN NOT MATCHED BY TARGET THEN
		INSERT (     [TenderSpecReference]
					,[LotReference]
					,[MinAge]
					,[MaxAge]
					,[MinLengthOfUnemployment]
					,[MaxLengthOfUnemployment]
					,[MinPriorAttainment]
					,[MaxPriorAttainment]
					,[CalcMethod]
					,[Benefits]
					)
			VALUES ( Source.[TenderSpecReference]
					,Source.[LotReference]
					,Source.[MinAge]
					,Source.[MaxAge]
					,Source.[MinLengthOfUnemployment]
					,Source.[MaxLengthOfUnemployment]
					,Source.[MinPriorAttainment]
					,Source.[MaxPriorAttainment]
					,Source.[CalcMethod]
					,Source.[Benefits]
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
