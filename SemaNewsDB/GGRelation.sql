﻿CREATE TABLE [dbo].[GGRelation]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
	[Notation] NVARCHAR(MAX) NULL,
	[Name] NVARCHAR(MAX) NULL,
	[Description] NTEXT NULL,
	[MetaData] NVARCHAR(MAX) NULL,
)
