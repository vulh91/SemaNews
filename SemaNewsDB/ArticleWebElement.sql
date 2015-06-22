CREATE TABLE [dbo].[ArticleWebElement]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
	[Address] NVARCHAR(MAX) NULL,									--> Địa chỉ dạng Xpath của WebElement trong trang nó thuộc về
	[Group] INT NULL,										--> Nhóm của WebElement
	[WebElementTypeId] INT NOT NULL,
	[NewspaperId] INT NOT NULL, 

    CONSTRAINT [FK_ArticleWebElement_Newspaper] FOREIGN KEY ([NewspaperId]) REFERENCES [Newspaper]([Id]), 
    CONSTRAINT [FK_ArticleWebElement_WebElementType] FOREIGN KEY ([WebElementTypeId]) REFERENCES [WebElementType]([Id]),									--> Là WebElement của trang bài viết thuộc trang báo điện tử nào

)
