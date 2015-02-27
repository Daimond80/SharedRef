exec shared_GetMatrixValues 'tariffs', 'cId=IN(3, 4); tId=2', 'cName, tName'

exec shared_GetMatrixValues 'tariffs', 'tId=2', 'cName'

exec shared_GetMatrixValues 'tariffs', 'tId=1;cId=4;oId=2', 'tName,cName,oName,Rate'


select 
	r.Id as Id,
	t.Id as tId, 
	t.Name as tName, 
	c.Id as cId, 
	c.Name as cName, 
	o.Id as oId, 
	o.Name as oName, 
	r.Rate as Rate 
from dbo.TariffRates r
inner join dbo.Cards c on c.Id = r.CardId
inner join dbo.Tariffs t on t.Id = r.TariffId
inner join dbo.CardOperations o on o.Id = r.CardOperationId
where t.Id=11 AND c.Id=4 AND o.Id=2
