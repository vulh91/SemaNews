CREATE TABLE [dbo].[WebElementType]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
	[WENotation] NVARCHAR(MAX) NULL,				--> Viết tắt loại WebElement
	[Name] NVARCHAR(MAX) NULL,								--> Tên viết tắt của loại WebElement
	[Description] NTEXT NULL,									--> Mô tả chức năng của loại WebELement
)
