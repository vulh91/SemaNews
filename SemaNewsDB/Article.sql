CREATE TABLE [dbo].[Article]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
	[Title] NVARCHAR(256) UNIQUE NOT NULL DEFAULT '',				--> Tiêu đề
	[Url] NVARCHAR(256) UNIQUE NOT NULL DEFAULT '',		--> Địa chỉ Url chứa bài viết gốc
	[ReleasedDate] SMALLDATETIME NULL,	--> Thời gian đăng tải
	[CollectedDate] SMALLDATETIME NULL,	--> Thời gian thu thập được
	[Abstract] NTEXT DEFAULT '',				--> Tóm tắt
	[Author] NVARCHAR(MAX) NULL DEFAULT '',		--> Tác giả
	[Tags] NVARCHAR(MAX) NULL DEFAULT '',			--> Các tags
	[Content] NVARCHAR(MAX) NOT NULL DEFAULT '',			--> Nội dung
	[IsIndexed] BIT NOT NULL DEFAULT 'False',
	[IsRelevantToLocal] BIT NULL,
	[IsMark] BIT DEFAULT 'False',
	
	[FieldId] INT NULL,						--> Thuộc về 1 Field nào (trang lĩnh vực nào)
	[GFieldId] INT NULL,						--> Thuộc về 1 GF nào (lĩnh vực phân loại)
	[NewspaperId] INT NULL, 

    CONSTRAINT [FK_Article_Field] FOREIGN KEY ([FieldId]) REFERENCES [Field]([Id]), 
    CONSTRAINT [FK_Article_GField] FOREIGN KEY ([GFieldId]) REFERENCES [GField]([Id]), 
    CONSTRAINT [FK_Article_Newspaper] FOREIGN KEY ([NewspaperId]) REFERENCES [Newspaper]([Id]),					--> Thuộc về Newspaper nào

)
