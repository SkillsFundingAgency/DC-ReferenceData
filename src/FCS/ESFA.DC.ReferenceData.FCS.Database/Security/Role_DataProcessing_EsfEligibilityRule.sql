
CREATE ROLE DataProcessing_EsfEligibilityRule AUTHORIZATION [dbo]
GO

-- Grant access rights to a specific schema in the database
GRANT 
	DELETE, 
	EXECUTE, 
	INSERT, 
	REFERENCES, 
	SELECT, 
	UPDATE, 
	VIEW DEFINITION 
ON SCHEMA::dbo
	TO DataProcessing_EsfEligibilityRule
GO

-- Grant access rights to a specific schema in the database
GRANT 
	DELETE, 
	EXECUTE, 
	INSERT, 
	REFERENCES, 
	SELECT, 
	UPDATE, 
	VIEW DEFINITION 
ON SCHEMA::Staging
	TO DataProcessing_EsfEligibilityRule
GO

