
GO
ALTER ROLE [db_datawriter] ADD MEMBER [FCS_RW_User];
GO
ALTER ROLE [db_datareader] ADD MEMBER [FCS_RW_User];
GO
ALTER ROLE [db_datareader] ADD MEMBER [FCS_RO_User];
