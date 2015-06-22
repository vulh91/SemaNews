CREATE TABLE [dbo].[SavedArticle]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
	[UserQueryId] INT NOT NULL,
	[ArticleId] INT NULL,

	[SavedTime] SMALLDATETIME NULL,
	[Title] NVARCHAR(MAX) NOT NULL,				--> Tiêu đề
	[Url] NVARCHAR(MAX) NOT NULL,		--> Địa chỉ Url chứa bài viết gốc
	[ReleasedDate] SMALLDATETIME NULL,	--> Thời gian đăng tải
	[CollectedDate] SMALLDATETIME NULL,	--> Thời gian thu thập được
	[Abstract] NTEXT NULL,				--> Tóm tắt
	[Author] NVARCHAR(MAX) NULL,		--> Tác giả
	[Tags] NVARCHAR(MAX) NULL,			--> Các tags
	[Content] NVARCHAR(MAX) NOT NULL,			--> Nội dung

	[FieldId] INT NULL,						
	[GFieldId] INT NULL,					
	[NewspaperId] INT NULL, 
    CONSTRAINT [FK_SavedArticle_UserQuery] FOREIGN KEY ([UserQueryId]) REFERENCES [UserQuery]([Id]) ON DELETE CASCADE ON UPDATE CASCADE, 
    CONSTRAINT [FK_SavedArticle_Field] FOREIGN KEY ([FieldId]) REFERENCES [Field]([Id]), 
    CONSTRAINT [FK_SavedArticle_GField] FOREIGN KEY ([GFieldId]) REFERENCES [GField]([Id]), 
    CONSTRAINT [FK_SavedArticle_Newspaper] FOREIGN KEY ([NewspaperId]) REFERENCES [Newspaper]([Id]), 
    CONSTRAINT [FK_SavedArticle_Article] FOREIGN KEY ([ArticleId]) REFERENCES [Article]([Id]),


)
