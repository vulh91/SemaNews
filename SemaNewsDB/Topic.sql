﻿CREATE TABLE [dbo].[Topic]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(MAX) NOT NULL,
	[Description] NVARCHAR(MAX) NULL,
	[Tags] NVARCHAR(MAX) NULL,
	[Keyphrases] NVARCHAR(MAX) NULL,
	[KeyphraseGraphs] NVARCHAR(MAX) NULL,
)
