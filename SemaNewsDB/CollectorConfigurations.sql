CREATE TABLE [dbo].[CollectorConfigurations]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(MAX),
	[Value] NVARCHAR(MAX),
	[Description] NVARCHAR(MAX),
)
