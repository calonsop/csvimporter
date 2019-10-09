
SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo'  AND  TABLE_NAME = 'Element'

CREATE TABLE [dbo].[Element]
(
	[Id] INT NOT NULL PRIMARY KEY Identity,
	[PointOfSale] int not null,
	[Product] varchar(100) not null,
	[Date] datetime not null,
	[Stock] int not null
)

TRUNCATE TABLE [dbo].[Element]

drop table [dbo].[Element]



select count(*) from  [dbo].[Element]
