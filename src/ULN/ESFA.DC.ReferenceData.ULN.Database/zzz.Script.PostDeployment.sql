﻿





/*
--------------------------------------------------------------------------------------
Post-Deployment Script
--------------------------------------------------------------------------------------
*/


GO
RAISERROR('		   Extended Property',10,1) WITH NOWAIT;
GO

RAISERROR('		         %s - %s',10,1,'BuildNumber','$(BUILD_BUILDNUMBER)') WITH NOWAIT;
IF NOT EXISTS (SELECT name, value FROM fn_listextendedproperty('BuildNumber', default, default, default, default, default, default))
	EXEC sp_addextendedproperty @name = N'BuildNumber', @value = '$(BUILD_BUILDNUMBER)';  
ELSE
	EXEC sp_updateextendedproperty @name = N'BuildNumber', @value = '$(BUILD_BUILDNUMBER)';  
	
GO
RAISERROR('		         %s - %s',10,1,'BuildBranch','$(BUILD_BRANCHNAME)') WITH NOWAIT;
IF NOT EXISTS (SELECT name, value FROM fn_listextendedproperty('BuildBranch', default, default, default, default, default, default))
	EXEC sp_addextendedproperty @name = N'BuildBranch', @value = '$(BUILD_BRANCHNAME)';  
ELSE
	EXEC sp_updateextendedproperty @name = N'BuildBranch', @value = '$(BUILD_BRANCHNAME)';  

GO
DECLARE @DeploymentTime VARCHAR(35) = CONVERT(VARCHAR(35),GETUTCDATE(),113);
RAISERROR('		         %s - %s',10,1,'DeploymentDatetime',@DeploymentTime) WITH NOWAIT;
IF NOT EXISTS (SELECT name, value FROM fn_listextendedproperty('DeploymentDatetime', default, default, default, default, default, default))
	EXEC sp_addextendedproperty @name = N'DeploymentDatetime', @value = @DeploymentTime;  
ELSE
	EXEC sp_updateextendedproperty @name = N'DeploymentDatetime', @value = @DeploymentTime;  
GO

RAISERROR('		         %s - %s',10,1,'ReleaseName','$(RELEASE_RELEASENAME)') WITH NOWAIT;
IF NOT EXISTS (SELECT name, value FROM fn_listextendedproperty('ReleaseName', default, default, default, default, default, default))
	EXEC sp_addextendedproperty @name = N'ReleaseName', @value = '$(RELEASE_RELEASENAME)';  
ELSE
	EXEC sp_updateextendedproperty @name = N'ReleaseName', @value = '$(BUILD_BRANCHNAME)';  
GO

DROP VIEW IF EXISTS [dbo].[DisplayDeploymentProperties_VW];
GO
--DROP VIEW IF EXISTS [dbo].[UniqueLearnerNumbers]
GO
--DROP PROCEDURE IF EXISTS [Staging].[usp_RemovedProcessedRecords];
GO
--DROP PROCEDURE IF EXISTS [Staging].[usp_RemovedProcessedRecords];
GO
--DROP TABLE IF EXISTS [Staging].[UniqueLearnerNumbers];
GO
--DROP SCHEMA IF EXISTS [Staging];
GO

RAISERROR('		   Update User Account Passwords',10,1) WITH NOWAIT;
GO

ALTER ROLE [db_datawriter] DROP MEMBER [ULN_RW_User];
GO
ALTER ROLE [db_datareader] DROP MEMBER [ULN_RW_User];
GO
ALTER ROLE [db_datareader] DROP MEMBER [ULN_RO_User];
GO

RAISERROR('		       RO User',10,1) WITH NOWAIT;
ALTER USER [ULN_RO_User] WITH PASSWORD = N'$(ROUserPassword)';
RAISERROR('		       RW User',10,1) WITH NOWAIT;
ALTER USER [ULN_RW_User] WITH PASSWORD = N'$(RWUserPassword)';

GO


GO
RAISERROR('Completed',10,1) WITH NOWAIT;
GO
