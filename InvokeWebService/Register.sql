sp_configure 'clr_enabled', 1
GO 

RECONFIGURE
GO

alter database fias SET TRUSTWORTHY ON

