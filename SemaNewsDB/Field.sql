CREATE TABLE [dbo].Field
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(MAX) NOT NULL,
	[Url] NVARCHAR(MAX) NOT NULL,
	[Description] NVARCHAR(MAX) NULL,
	[IsActivated] BIT DEFAULT 'FALSE',
	[Group] INT NULL,
	[LastUpdateTime] SMALLDATETIME NULL,
	[DefinedTime] SMALLDATETIME NULL,
	[NewspaperId] INT NULL,					
	[GFieldId] INT NULL, 

    CONSTRAINT [FK_Field_Newspaper] FOREIGN KEY ([NewspaperId]) REFERENCES [Newspaper]([Id]), 
    CONSTRAINT [FK_Field_GField] FOREIGN KEY ([GFieldId]) REFERENCES [GField]([Id]),


)
