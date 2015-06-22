CREATE TABLE [dbo].[FieldWebElement]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
	[Address] NVARCHAR(MAX) NOT NULL,								--> Địa chỉ dạng Xpath của WebElement trong trang nó thuộc về
	[Group] INT NULL,											--> Nhóm của WebElement
	[Priority] INT NULL,
	[WebElementTypeId] INT NOT NULL,							--> WebElement loại gì, chức năng gì
	[NewspaperId] INT NOT NULL, 
    CONSTRAINT [FK_FieldWebElement_Newspaper] FOREIGN KEY ([NewspaperId]) REFERENCES [Newspaper]([Id]), 
    CONSTRAINT [FK_FieldWebElement_WebElementType] FOREIGN KEY ([WebElementTypeId]) REFERENCES [WebElementType]([Id]),									--> Là WebElement của trang báo điện tử nào


)
