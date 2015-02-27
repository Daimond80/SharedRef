drop procedure dbo.shared_GetMatrixValues
go

drop Assembly InvokeWebServiceXML
go

drop Assembly InvokeWebService
go



Create Assembly InvokeWebService from
	'C:\Sobin\SharedRef\InvokeWebService\bin\Release\InvokeWebService.dll'
	WITH PERMISSION_SET = EXTERNAL_ACCESS
	
Create Assembly InvokeWebServiceXML from
	'C:\Sobin\SharedRef\InvokeWebService\bin\Release\InvokeWebService.XmlSerializers.dll'
	WITH PERMISSION_SET = EXTERNAL_ACCESS
	
go

CREATE PROCEDURE dbo.shared_GetMatrixValues
(
	@name nvarchar(250),
	@conditionals nvarchar(250),
	@returns  nvarchar(250)
)
AS EXTERNAL Name [InvokeWebService].[StoredProcedures].[shared_GetMatrixValues]