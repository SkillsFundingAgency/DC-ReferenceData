CREATE PROCEDURE [Staging].[usp_Process]
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
				
			EXEC [Staging].[usp_Process_EsfEligibilityRule];
			EXEC [Staging].[usp_Process_EsfEligibilityRuleEmploymentStatus];
			EXEC [Staging].[usp_Process_EsfEligibilityRuleLocalAuthority];
			EXEC [Staging].[usp_Process_EsfEligibilityRuleLocalEnterprisePartnership];
			EXEC [Staging].[usp_Process_EsfEligibilityRuleSectorSubjectAreaLevel];

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

GO

GRANT EXECUTE ON [Staging].[usp_Process] TO [FCS_RW_User]
GO
