USE [SemaNewsDB]
GO
/****** Object:  Table [dbo].[AARelationInstance]    Script Date: 12/17/2014 11:11:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AARelationInstance](
	[ArticleId1] [int] NOT NULL,
	[ArticleId2] [int] NOT NULL,
	[NRelationId] [int] NOT NULL,
	[RelationWeight] [float] NULL,
 CONSTRAINT [PK_AARelationInstance] PRIMARY KEY CLUSTERED 
(
	[ArticleId1] ASC,
	[ArticleId2] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Article]    Script Date: 12/17/2014 11:11:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Article](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](256) NOT NULL,
	[Url] [nvarchar](256) NOT NULL,
	[ReleasedDate] [smalldatetime] NULL,
	[CollectedDate] [smalldatetime] NULL,
	[Abstract] [ntext] NULL,
	[Author] [nvarchar](max) NULL,
	[Tags] [nvarchar](max) NULL,
	[Content] [nvarchar](max) NOT NULL,
	[IsIndexed] [bit] NOT NULL,
	[IsRelevantToLocal] [bit] NULL,
	[IsMark] [bit] NULL,
	[FieldId] [int] NULL,
	[GFieldId] [int] NULL,
	[NewspaperId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ArticleKG]    Script Date: 12/17/2014 11:11:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArticleKG](
	[Id] [int] NOT NULL,
	[LDVL_Graph] [nvarchar](max) NULL,
	[DT_Graph] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ArticleWebElement]    Script Date: 12/17/2014 11:11:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArticleWebElement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Address] [nvarchar](max) NULL,
	[Group] [int] NULL,
	[WebElementTypeId] [int] NOT NULL,
	[NewspaperId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CollectorConfigurations]    Script Date: 12/17/2014 11:11:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollectorConfigurations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Value] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FFRelationInstance]    Script Date: 12/17/2014 11:11:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FFRelationInstance](
	[FieldId1] [int] NOT NULL,
	[FieldId2] [int] NOT NULL,
	[NRelationId] [int] NOT NULL,
 CONSTRAINT [PK_FFRelation] PRIMARY KEY CLUSTERED 
(
	[FieldId1] ASC,
	[FieldId2] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Field]    Script Date: 12/17/2014 11:11:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Field](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Url] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[IsActivated] [bit] NULL,
	[Group] [int] NULL,
	[LastUpdateTime] [smalldatetime] NULL,
	[DefinedTime] [smalldatetime] NULL,
	[NewspaperId] [int] NULL,
	[GFieldId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FieldWebElement]    Script Date: 12/17/2014 11:11:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FieldWebElement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[Group] [int] NULL,
	[Priority] [int] NULL,
	[WebElementTypeId] [int] NOT NULL,
	[NewspaperId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GField]    Script Date: 12/17/2014 11:11:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GField](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GGRelation]    Script Date: 12/17/2014 11:11:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GGRelation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Notation] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [ntext] NULL,
	[MetaData] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GGRelationInstance]    Script Date: 12/17/2014 11:11:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GGRelationInstance](
	[GFieldId1] [int] NOT NULL,
	[GFieldId2] [int] NOT NULL,
	[GGRelationId] [int] NOT NULL,
 CONSTRAINT [PK_GGRelation] PRIMARY KEY CLUSTERED 
(
	[GFieldId1] ASC,
	[GFieldId2] ASC,
	[GGRelationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Newspaper]    Script Date: 12/17/2014 11:11:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Newspaper](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Url] [nvarchar](max) NOT NULL,
	[IsLocal] [bit] NULL,
	[Description] [nvarchar](max) NULL,
	[DefinedTime] [smalldatetime] NULL,
	[IsActivated] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NNRelationInstance]    Script Date: 12/17/2014 11:11:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NNRelationInstance](
	[NewspaperId1] [int] NOT NULL,
	[NewspaperId2] [int] NOT NULL,
	[NRelationId] [int] NOT NULL,
 CONSTRAINT [PK_NNRelationInstance] PRIMARY KEY CLUSTERED 
(
	[NewspaperId1] ASC,
	[NewspaperId2] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NRelation]    Script Date: 12/17/2014 11:11:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NRelation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Notation] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Role]    Script Date: 12/17/2014 11:11:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SavedArticle]    Script Date: 12/17/2014 11:11:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SavedArticle](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserQueryId] [int] NOT NULL,
	[ArticleId] [int] NULL,
	[SavedTime] [smalldatetime] NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Url] [nvarchar](max) NOT NULL,
	[ReleasedDate] [smalldatetime] NULL,
	[CollectedDate] [smalldatetime] NULL,
	[Abstract] [ntext] NULL,
	[Author] [nvarchar](max) NULL,
	[Tags] [nvarchar](max) NULL,
	[Content] [nvarchar](max) NOT NULL,
	[FieldId] [int] NULL,
	[GFieldId] [int] NULL,
	[NewspaperId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SystemMessage]    Script Date: 12/17/2014 11:11:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemMessage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](50) NULL,
	[Time] [smalldatetime] NOT NULL,
	[UserId] [int] NOT NULL,
	[Title] [nvarchar](200) NULL,
	[Content] [nvarchar](max) NULL,
	[URL] [nvarchar](200) NULL,
	[IsRead] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Topic]    Script Date: 12/17/2014 11:11:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Topic](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Tags] [nvarchar](max) NULL,
	[Keyphrases] [nvarchar](max) NULL,
	[KeyphraseGraphs] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 12/17/2014 11:11:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[RoleId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserProfile]    Script Date: 12/17/2014 11:11:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfile](
	[Id] [int] NOT NULL,
	[DisplayName] [nvarchar](max) NULL,
	[Avatar] [nvarchar](max) NULL,
	[DateCreated] [smalldatetime] NULL,
	[Signature] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserQuery]    Script Date: 12/17/2014 11:11:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserQuery](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[SearchQuery] [nvarchar](max) NULL,
	[SavedTime] [smalldatetime] NULL,
	[IsSaved] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VisitedLink]    Script Date: 12/17/2014 11:11:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VisitedLink](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[URL] [nvarchar](250) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[Time] [smalldatetime] NULL,
	[VisitCount] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WebElementType]    Script Date: 12/17/2014 11:11:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WebElementType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WENotation] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [ntext] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[ArticleWebElement] ON 

INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (25, N'/html/body/form{method=post}{action=}{id=form1}/div[3]{id=pagewrap}/div[2]{id=content}/div{class=fl w660}/div{class=detail}/div{class=shadow1}/div[2]{class=box10}/h1{fromHead=1}{fromTail=14}', 1, 3, 4)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (26, N'/html/body/form{method=post}{action=}{id=form1}/div[3]{id=pagewrap}/div[2]{id=content}/div{class=fl w660}/div{class=detail}/div{class=shadow1}/div[2]{class=box10}/p{class=detail-time}{fromHead=2}{fromTail=13}', 1, 6, 4)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (27, N'/html/body/form{method=post}{action=}{id=form1}/div[3]{id=pagewrap}/div[2]{id=content}/div{class=fl w660}/div{class=detail}/div{class=shadow1}/div[2]{class=box10}/div[6]{class=tags}{style=background:none;padding:0px;width:627px;margin-bottom:5px}{fromHead=10}{fromTail=5}', 1, 8, 4)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (28, NULL, 1, 2, 4)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (29, N'/html/body/form{method=post}{action=}{id=form1}/div[3]{id=pagewrap}/div[2]{id=content}/div{class=fl w660}/div{class=detail}/div{class=shadow1}/div[2]{class=box10}/h2{style=margin: 5px 15px}{fromHead=3}{fromTail=12}', 1, 4, 4)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (30, N'/html/body/form{method=post}{action=}{id=form1}/div[3]{id=pagewrap}/div[2]{id=content}/div{class=fl w660}/div{class=detail}/div{class=shadow1}/div[2]{class=box10}/div[2]{id=divNewsContent}{class=content news_detail}{fromHead=4}{fromTail=11}', 1, 5, 4)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (31, N'/html/body/form{method=post}{action=}{id=form1}/div[3]{id=pagewrap}/div[2]{id=content}/div{class=fl w660}/div{class=detail}/div{class=shadow1}/div[2]{class=box10}/h1{fromHead=1}{fromTail=14}', 2, 3, 4)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (32, N'/html/body/form{method=post}{action=}{id=form1}/div[3]{id=pagewrap}/div[2]{id=content}/div{class=fl w660}/div{class=detail}/div{class=shadow1}/div[2]{class=box10}/p{class=detail-time}{fromHead=2}{fromTail=13}', 2, 6, 4)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (33, N'/html/body/form{method=post}{action=}{id=form1}/div[3]{id=pagewrap}/div[2]{id=content}/div{class=fl w660}/div{class=detail}/div{class=shadow1}/div[2]{class=box10}/div[6]{class=tags}{style=background:none;padding:0px;width:627px;margin-bottom:5px}{fromHead=10}{fromTail=5}', 2, 8, 4)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (34, NULL, 2, 2, 4)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (35, N'/html/body/form{method=post}{action=}{id=form1}/div[3]{id=pagewrap}/div[2]{id=content}/div{class=fl w660}/div{class=detail}/div{class=shadow1}/div[2]{class=box10}/h2{style=margin: 5px 15px}{fromHead=3}{fromTail=12}', 2, 4, 4)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (36, N'/html/body/form{method=post}{action=}{id=form1}/div[3]{id=pagewrap}/div[2]{id=content}/div{class=fl w660}/div{class=detail}/div{class=shadow1}/div[2]{class=box10}/div[2]{id=divNewsContent}{class=content news_detail}{fromHead=4}{fromTail=11}', 2, 5, 4)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (49, N'/html/body/div{id=MainWraper}/div[4]{id=BodyWraper}/div{class=clearfix}/div{class=BodyLayout690 left m-r-10}/div{class=clearfix}/div{class=BodyLayout460 left m-r-10}/div[2]{class=ArticleDetail}/h1{class=title}{fromHead=0}{fromTail=12}', 1, 3, 7)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (50, N'/html/body/div{id=MainWraper}/div[4]{id=BodyWraper}/div{class=clearfix}/div{class=BodyLayout690 left m-r-10}/div{class=clearfix}/div{class=BodyLayout460 left m-r-10}/div{class=PageTitleBar}/div{class=clearfix}/div[2]{class=right}/div{class=clearfix}/div{class=left}/span{class=ArticleDate}{fromHead=0}{fromTail=1}', 1, 6, 7)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (51, N'/html/body/div{id=MainWraper}/div[4]{id=BodyWraper}/div{class=clearfix}/div{class=BodyLayout690 left m-r-10}/div{class=clearfix}/div{class=BodyLayout460 left m-r-10}/div[2]{class=ArticleDetail}/div[4]{class=tagBox}/div{class=tagBoxContent}/ul{class=clearfix}{fromHead=0}{fromTail=1}', 1, 8, 7)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (52, NULL, 1, 2, 7)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (53, N'/html/body/div{id=MainWraper}/div[4]{id=BodyWraper}/div{class=clearfix}/div{class=BodyLayout690 left m-r-10}/div{class=clearfix}/div{class=BodyLayout460 left m-r-10}/div[2]{class=ArticleDetail}/div{class=ArticleContent}/p/strong{fromHead=0}{fromTail=1}', 1, 4, 7)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (54, N'/html/body/div{id=MainWraper}/div[4]{id=BodyWraper}/div{class=clearfix}/div{class=BodyLayout690 left m-r-10}/div{class=clearfix}/div{class=BodyLayout460 left m-r-10}/div[2]{class=ArticleDetail}/div{class=ArticleContent}{fromHead=1}{fromTail=11}', 1, 5, 7)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (55, N'/html/body/div{id=MainWraper}/div[4]{id=BodyWraper}/div{class=clearfix}/div{class=BodyLayout690 left m-r-10}/div{class=clearfix}/div{class=BodyLayout460 left m-r-10}/div[2]{class=ArticleDetail}/h1{class=title}{fromHead=0}{fromTail=10}', 2, 3, 7)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (56, N'/html/body/div{id=MainWraper}/div[4]{id=BodyWraper}/div{class=clearfix}/div{class=BodyLayout690 left m-r-10}/div{class=clearfix}/div{class=BodyLayout460 left m-r-10}/div{class=PageTitleBar}/div{class=clearfix}/div[2]{class=right}/div{class=clearfix}/div{class=left}/span{class=ArticleDate}{fromHead=0}{fromTail=1}', 2, 6, 7)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (57, N'/html/body/div{id=MainWraper}/div[4]{id=BodyWraper}/div{class=clearfix}/div{class=BodyLayout690 left m-r-10}/div{class=clearfix}/div{class=BodyLayout460 left m-r-10}/div[2]{class=ArticleDetail}/div[3]{class=tagBox}/div{class=tagBoxContent}/ul{class=clearfix}{fromHead=0}{fromTail=1}', 2, 8, 7)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (58, NULL, 2, 2, 7)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (59, N'/html/body/div{id=MainWraper}/div[4]{id=BodyWraper}/div{class=clearfix}/div{class=BodyLayout690 left m-r-10}/div{class=clearfix}/div{class=BodyLayout460 left m-r-10}/div[2]{class=ArticleDetail}/div{class=ArticleContent}/p/strong{fromHead=0}{fromTail=1}', 2, 4, 7)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (60, N'/html/body/div{id=MainWraper}/div[4]{id=BodyWraper}/div{class=clearfix}/div{class=BodyLayout690 left m-r-10}/div{class=clearfix}/div{class=BodyLayout460 left m-r-10}/div[2]{class=ArticleDetail}/div{class=ArticleContent}{fromHead=1}{fromTail=9}', 2, 5, 7)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (61, N'/html/body/div{id=MainWraper}/div[4]{id=BodyWraper}/div{class=clearfix}/div{class=BodyLayout690 left m-r-10}/div{class=clearfix}/div{class=BodyLayout460 left m-r-10}/div[2]{class=ArticleDetail}/h1{class=title}{fromHead=0}{fromTail=10}', 3, 3, 7)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (62, N'/html/body/div{id=MainWraper}/div[4]{id=BodyWraper}/div{class=clearfix}/div{class=BodyLayout690 left m-r-10}/div{class=clearfix}/div{class=BodyLayout460 left m-r-10}/div{class=PageTitleBar}/div{class=clearfix}/div[2]{class=right}/div{class=clearfix}/div{class=left}/span{class=ArticleDate}{fromHead=0}{fromTail=1}', 3, 6, 7)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (63, N'/html/body/div{id=MainWraper}/div[4]{id=BodyWraper}/div{class=clearfix}/div{class=BodyLayout690 left m-r-10}/div{class=clearfix}/div{class=BodyLayout460 left m-r-10}/div[2]{class=ArticleDetail}/div[3]{class=tagBox}/div{class=tagBoxContent}/ul{class=clearfix}{fromHead=0}{fromTail=1}', 3, 8, 7)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (64, NULL, 3, 2, 7)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (65, N'/html/body/div{id=MainWraper}/div[4]{id=BodyWraper}/div{class=clearfix}/div{class=BodyLayout690 left m-r-10}/div{class=clearfix}/div{class=BodyLayout460 left m-r-10}/div[2]{class=ArticleDetail}/div{class=ArticleContent}/p/strong{fromHead=0}{fromTail=1}', 3, 4, 7)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (66, N'/html/body/div{id=MainWraper}/div[4]{id=BodyWraper}/div{class=clearfix}/div{class=BodyLayout690 left m-r-10}/div{class=clearfix}/div{class=BodyLayout460 left m-r-10}/div[2]{class=ArticleDetail}/div{class=ArticleContent}{fromHead=1}{fromTail=9}', 3, 5, 7)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (97, N'/html/body/form{name=aspnetForm}{method=post}{action=/doanh-nhan/pho-thu-tuong-dung-ep-doanh-nhan-bo-tien-mua-thu-tuc-201408310226042.htm}{id=aspnetForm}/div[3]{class=wp980}/div[4]{class=content}/div{class=contentleft}/div[2]/h1{class=h1titleheaderbvt}{fromHead=0}{fromTail=2}', 1, 3, 8)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (98, N'/html/body/form{name=aspnetForm}{method=post}{action=/doanh-nhan/pho-thu-tuong-dung-ep-doanh-nhan-bo-tien-mua-thu-tuc-201408310226042.htm}{id=aspnetForm}/div[3]{class=wp980}/div[4]{class=content}/div{class=contentleft}/div{style=float:left;width:100%;margin-bottom:5px}/span{class=timverbvvth}{fromHead=1}{fromTail=1}', 1, 6, 8)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (99, N'/html/body/form{name=aspnetForm}{method=post}{action=/doanh-nhan/pho-thu-tuong-dung-ep-doanh-nhan-bo-tien-mua-thu-tuc-201408310226042.htm}{id=aspnetForm}/div[3]{class=wp980}/div[4]{class=content}/div{class=contentleft}/div[3]{class=maincontentleft}/div{class=leftmaincontentleft}/div[4]{class=tagbaiviet}/ul{class=lsttagbaiviet}{fromHead=1}{fromTail=2}', 1, 8, 8)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (100, NULL, 1, 2, 8)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (101, N'/html/body/form{name=aspnetForm}{method=post}{action=/doanh-nhan/pho-thu-tuong-dung-ep-doanh-nhan-bo-tien-mua-thu-tuc-201408310226042.htm}{id=aspnetForm}/div[3]{class=wp980}/div[4]{class=content}/div{class=contentleft}/div[2]/h2{class=h2titleheaderbvt}{fromHead=1}{fromTail=1}', 1, 4, 8)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (102, N'/html/body/form{name=aspnetForm}{method=post}{action=/doanh-nhan/pho-thu-tuong-dung-ep-doanh-nhan-bo-tien-mua-thu-tuc-201408310226042.htm}{id=aspnetForm}/div[3]{class=wp980}/div[4]{class=content}/div{class=contentleft}/div[3]{class=maincontentleft}/div{class=leftmaincontentleft}/div{class=detailsbaiviet}{fromHead=3}{fromTail=4}', 1, 5, 8)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (103, N'/html/body/form{name=aspnetForm}{method=post}{action=/doanh-nhan/sep-evn-chi-duoc-thuong-toi-da-15-thang-luong-20140828110053478.htm}{id=aspnetForm}/div[3]{class=wp980}/div[4]{class=content}/div{class=contentleft}/div[2]/h1{class=h1titleheaderbvt}{fromHead=0}{fromTail=2}', 2, 3, 8)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (104, N'/html/body/form{name=aspnetForm}{method=post}{action=/doanh-nhan/sep-evn-chi-duoc-thuong-toi-da-15-thang-luong-20140828110053478.htm}{id=aspnetForm}/div[3]{class=wp980}/div[4]{class=content}/div{class=contentleft}/div{style=float:left;width:100%;margin-bottom:5px}/span{class=timverbvvth}{fromHead=1}{fromTail=1}', 2, 6, 8)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (105, N'/html/body/form{name=aspnetForm}{method=post}{action=/doanh-nhan/sep-evn-chi-duoc-thuong-toi-da-15-thang-luong-20140828110053478.htm}{id=aspnetForm}/div[3]{class=wp980}/div[4]{class=content}/div{class=contentleft}/div[3]{class=maincontentleft}/div{class=leftmaincontentleft}/div[4]{class=tagbaiviet}/ul{class=lsttagbaiviet}{fromHead=1}{fromTail=2}', 2, 8, 8)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (106, NULL, 2, 2, 8)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (107, N'/html/body/form{name=aspnetForm}{method=post}{action=/doanh-nhan/sep-evn-chi-duoc-thuong-toi-da-15-thang-luong-20140828110053478.htm}{id=aspnetForm}/div[3]{class=wp980}/div[4]{class=content}/div{class=contentleft}/div[2]/h2{class=h2titleheaderbvt}{fromHead=1}{fromTail=1}', 2, 4, 8)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (108, N'/html/body/form{name=aspnetForm}{method=post}{action=/doanh-nhan/sep-evn-chi-duoc-thuong-toi-da-15-thang-luong-20140828110053478.htm}{id=aspnetForm}/div[3]{class=wp980}/div[4]{class=content}/div{class=contentleft}/div[3]{class=maincontentleft}/div{class=leftmaincontentleft}/div{class=detailsbaiviet}{fromHead=3}{fromTail=4}', 2, 5, 8)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (109, N'/html/body/div[2]{id=page}/div[3]{id=wrapper_container}/div{id=container}/div{class=width_common}/div[3]{id=detail_page}{class=width_common line_col}/div{id=col_660}{class=left}/div{class=box_width_common}{id=box_details_news}/div{class=w670 left}/div{class=main_content_detail width_common}/div{class=width_common line_col_480 space_bottom_20}/div{class=block_col_480}/div[3]{class=title_news}/h1{fromHead=0}{fromTail=1}', 3, 3, 8)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (110, N'/html/body/div[2]{id=page}/div[3]{id=wrapper_container}/div{id=container}/div{class=width_common}/div[3]{id=detail_page}{class=width_common line_col}/div{id=col_660}{class=left}/div{class=box_width_common}{id=box_details_news}/div{class=w670 left}/div{class=main_content_detail width_common}/div{class=width_common line_col_480 space_bottom_20}/div{class=block_col_480}/div{class=block_timer_share}{fromHead=0}{fromTail=7}', 3, 6, 8)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (111, N'/html/body/div[2]{id=page}/div[3]{id=wrapper_container}/div{id=container}/div{class=width_common}/div[3]{id=detail_page}{class=width_common line_col}/div{id=col_660}{class=left}/div{class=box_width_common}{id=box_details_news}/div{class=w670 left}/div{class=main_content_detail width_common}/div[3]{class=block_tag width_common space_bottom_20}{fromHead=4}{fromTail=4}', 3, 8, 8)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (112, NULL, 3, 2, 8)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (113, N'/html/body/div[2]{id=page}/div[3]{id=wrapper_container}/div{id=container}/div{class=width_common}/div[3]{id=detail_page}{class=width_common line_col}/div{id=col_660}{class=left}/div{class=box_width_common}{id=box_details_news}/div{class=w670 left}/div{class=main_content_detail width_common}/div{class=width_common line_col_480 space_bottom_20}/div{class=block_col_480}/div[4]{class=short_intro txt_666}{fromHead=3}{fromTail=4}', 3, 4, 8)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (114, N'/html/body/div[2]{id=page}/div[3]{id=wrapper_container}/div{id=container}/div{class=width_common}/div[3]{id=detail_page}{class=width_common line_col}/div{id=col_660}{class=left}/div{class=box_width_common}{id=box_details_news}/div{class=w670 left}/div{class=main_content_detail width_common}/div{class=width_common line_col_480 space_bottom_20}/div{class=block_col_480}/div[6]{id=left_calculator}/div{class=fck_detail width_common}{fromHead=0}{fromTail=4}', 3, 5, 8)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (115, N'/html/body/div[2]{id=page}/div[3]{id=wrapper_container}/div{id=container}/div{class=width_common}/div[3]{id=detail_page}{class=width_common line_col}/div{id=col_660}{class=left}/div{class=box_width_common}{id=box_details_news}/div{class=w670 left}/div{class=main_content_detail width_common}/div{class=width_common line_col_480 space_bottom_20}/div{class=block_col_480}/div[3]{class=title_news}/h1{fromHead=0}{fromTail=1}', 1, 3, 3)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (116, N'/html/body/div[2]{id=page}/div[3]{id=wrapper_container}/div{id=container}/div{class=width_common}/div[3]{id=detail_page}{class=width_common line_col}/div{id=col_660}{class=left}/div{class=box_width_common}{id=box_details_news}/div{class=w670 left}/div{class=main_content_detail width_common}/div{class=width_common line_col_480 space_bottom_20}/div{class=block_col_480}/div{class=block_timer_share}{fromHead=0}{fromTail=7}', 1, 6, 3)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (117, N'/html/body/div[2]{id=page}/div[3]{id=wrapper_container}/div{id=container}/div{class=width_common}/div[3]{id=detail_page}{class=width_common line_col}/div{id=col_660}{class=left}/div{class=box_width_common}{id=box_details_news}/div{class=w670 left}/div{class=main_content_detail width_common}/div[2]{class=block_tag width_common space_bottom_20}{fromHead=1}{fromTail=4}', 1, 8, 3)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (118, NULL, 1, 2, 3)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (119, N'/html/body/div[2]{id=page}/div[3]{id=wrapper_container}/div{id=container}/div{class=width_common}/div[3]{id=detail_page}{class=width_common line_col}/div{id=col_660}{class=left}/div{class=box_width_common}{id=box_details_news}/div{class=w670 left}/div{class=main_content_detail width_common}/div{class=width_common line_col_480 space_bottom_20}/div{class=block_col_480}/div[4]{class=short_intro txt_666}{fromHead=3}{fromTail=4}', 1, 4, 3)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (120, N'/html/body/div[2]{id=page}/div[3]{id=wrapper_container}/div{id=container}/div{class=width_common}/div[3]{id=detail_page}{class=width_common line_col}/div{id=col_660}{class=left}/div{class=box_width_common}{id=box_details_news}/div{class=w670 left}/div{class=main_content_detail width_common}/div{class=width_common line_col_480 space_bottom_20}/div{class=block_col_480}/div[6]{id=left_calculator}/div{class=fck_detail width_common}{fromHead=0}{fromTail=4}', 1, 5, 3)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (151, N'/html/body/form{name=aspnetForm}{method=post}{action=/cong-doan/viet-tiep-vu-28-giao-vien-hop-dong-o-chuong-my-ha-noi-can-dam-bao-quyen-loi-cho-giao-vien-253293.bld}{id=aspnetForm}/div[2]{id=container}{class=clearfix wrap-site}/div[2]{class=content}/section{class=article-content clearfix}/div{class=main-contents}/header/h1{fromHead=1}{fromTail=1}', 1, 3, 9)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (152, N'/html/body/form{name=aspnetForm}{method=post}{action=/cong-doan/viet-tiep-vu-28-giao-vien-hop-dong-o-chuong-my-ha-noi-can-dam-bao-quyen-loi-cho-giao-vien-253293.bld}{id=aspnetForm}/div[2]{id=container}{class=clearfix wrap-site}/div[2]{class=content}/section{class=article-content clearfix}/div{class=main-contents}/div{class=meta}{fromHead=1}{fromTail=8}', 1, 6, 9)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (153, N'/html/body/form{name=aspnetForm}{method=post}{action=/cong-doan/viet-tiep-vu-28-giao-vien-hop-dong-o-chuong-my-ha-noi-can-dam-bao-quyen-loi-cho-giao-vien-253293.bld}{id=aspnetForm}/div[2]{id=container}{class=clearfix wrap-site}/div[2]{class=content}/section{class=article-content clearfix}/footer{class=clearfix keywords}/ul{fromHead=0}{fromTail=1}', 1, 8, 9)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (154, NULL, 1, 2, 9)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (155, N'/html/body/form{name=aspnetForm}{method=post}{action=/cong-doan/viet-tiep-vu-28-giao-vien-hop-dong-o-chuong-my-ha-noi-can-dam-bao-quyen-loi-cho-giao-vien-253293.bld}{id=aspnetForm}/div[2]{id=container}{class=clearfix wrap-site}/div[2]{class=content}/section{class=article-content clearfix}/div{class=main-contents}/h2{class=sapo}{fromHead=3}{fromTail=6}', 1, 4, 9)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (156, N'/html/body/form{name=aspnetForm}{method=post}{action=/cong-doan/viet-tiep-vu-28-giao-vien-hop-dong-o-chuong-my-ha-noi-can-dam-bao-quyen-loi-cho-giao-vien-253293.bld}{id=aspnetForm}/div[2]{id=container}{class=clearfix wrap-site}/div[2]{class=content}/section{class=article-content clearfix}/div{class=main-contents}/div[3]{class=article-body}/div{fromHead=1}{fromTail=1}', 1, 5, 9)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (157, N'/html/body/form{name=aspnetForm}{method=post}{action=/xa-hoi/tau-sunrise-689-bi-mat-tich-tro-ve-tu-tay-cuop-bien-254592.bld}{id=aspnetForm}/div[2]{id=container}{class=clearfix wrap-site}/div[2]{class=content}/section{class=article-content clearfix}/div{class=main-contents}/header/h1{fromHead=1}{fromTail=1}', 2, 3, 9)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (158, N'/html/body/form{name=aspnetForm}{method=post}{action=/xa-hoi/tau-sunrise-689-bi-mat-tich-tro-ve-tu-tay-cuop-bien-254592.bld}{id=aspnetForm}/div[2]{id=container}{class=clearfix wrap-site}/div[2]{class=content}/section{class=article-content clearfix}/div{class=main-contents}/div{class=meta}{fromHead=1}{fromTail=8}', 2, 6, 9)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (159, N'/html/body/form{name=aspnetForm}{method=post}{action=/xa-hoi/tau-sunrise-689-bi-mat-tich-tro-ve-tu-tay-cuop-bien-254592.bld}{id=aspnetForm}/div[2]{id=container}{class=clearfix wrap-site}/div[2]{class=content}/section{class=article-content clearfix}/footer{class=clearfix keywords}/ul{fromHead=0}{fromTail=1}', 2, 8, 9)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (160, NULL, 2, 2, 9)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (161, N'/html/body/form{name=aspnetForm}{method=post}{action=/xa-hoi/tau-sunrise-689-bi-mat-tich-tro-ve-tu-tay-cuop-bien-254592.bld}{id=aspnetForm}/div[2]{id=container}{class=clearfix wrap-site}/div[2]{class=content}/section{class=article-content clearfix}/div{class=main-contents}/h2{class=sapo}{fromHead=3}{fromTail=6}', 2, 4, 9)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (162, N'/html/body/form{name=aspnetForm}{method=post}{action=/xa-hoi/tau-sunrise-689-bi-mat-tich-tro-ve-tu-tay-cuop-bien-254592.bld}{id=aspnetForm}/div[2]{id=container}{class=clearfix wrap-site}/div[2]{class=content}/section{class=article-content clearfix}/div{class=main-contents}/div[3]{class=article-body}{fromHead=5}{fromTail=4}', 2, 5, 9)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (175, N'/html/body/div{class=wrap}/div[2]{class=nld-body}/div{class=nld-container}/div[2]{class=nld-content}/div{class=page page-news}/div{class=page-header}/h3{fromHead=0}{fromTail=1}', 1, 3, 10)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (176, N'/html/body/div{class=wrap}/div[2]{class=nld-body}/div{class=nld-container}/div[2]{class=nld-content}/div{class=page page-news}/div[2]{class=page-body}{fromHead=1}{fromTail=1}', 1, 6, 10)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (177, NULL, 1, 8, 10)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (178, NULL, 1, 2, 10)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (179, NULL, 1, 4, 10)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (180, N'/html/body/div{class=wrap}/div[2]{class=nld-body}/div{class=nld-container}/div[2]{class=nld-content}/div{class=page page-news}/div[2]{class=page-body}{fromHead=1}{fromTail=1}', 1, 5, 10)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (193, N'/html/body/form{name=aspnetForm}{method=post}{action=/Details.aspx?News_ID=957771&PageType=4}{id=aspnetForm}/div[6]{class=wrapper}/div{class=container}/div{class=clearfix}/div[2]{class=fl wid470}/div{id=ctl00_IDContent_Tin_Chi_Tiet}/div{id=ctl00_IDContent_ctl00_divContent}/h1{class=fon31 mt2}{fromHead=2}{fromTail=4}', 1, 3, 2)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (194, N'/html/body/form{name=aspnetForm}{method=post}{action=/Details.aspx?News_ID=957771&PageType=4}{id=aspnetForm}/div[6]{class=wrapper}/div{class=container}/div{class=clearfix}/div[2]{class=fl wid470}/div{id=ctl00_IDContent_Tin_Chi_Tiet}/div{id=ctl00_IDContent_ctl00_divContent}/div{class=box26}{fromHead=0}{fromTail=6}', 1, 6, 2)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (195, NULL, 1, 8, 2)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (196, NULL, 1, 2, 2)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (197, N'/html/body/form{name=aspnetForm}{method=post}{action=/Details.aspx?News_ID=957771&PageType=4}{id=aspnetForm}/div[6]{class=wrapper}/div{class=container}/div{class=clearfix}/div[2]{class=fl wid470}/div{id=ctl00_IDContent_Tin_Chi_Tiet}/div{id=ctl00_IDContent_ctl00_divContent}/h2{class=fon33 mt1}{fromHead=4}{fromTail=2}', 1, 4, 2)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (198, N'/html/body/form{name=aspnetForm}{method=post}{action=/Details.aspx?News_ID=957771&PageType=4}{id=aspnetForm}/div[6]{class=wrapper}/div{class=container}/div{class=clearfix}/div[2]{class=fl wid470}/div{id=ctl00_IDContent_Tin_Chi_Tiet}/div{id=ctl00_IDContent_ctl00_divContent}/div[3]{class=fon34 mt3 mr2 fon43}{fromHead=5}{fromTail=1}', 1, 5, 2)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (199, N'/html/body/div[2]{class=wrapper detail}/section{class=main}/section{class=content }/div{class=detail-content}/div{class=block-feature block-feature-1}/h1{class=title-2}/a{id=object_title}{href=javascript:;}{fromHead=0}{fromTail=1}', 1, 3, 6)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (200, N'/html/body/div[2]{class=wrapper detail}/section{class=main}/section{class=content }/div{class=detail-content}/div{class=block-feature block-feature-1}/div{class=tool-bar}{fromHead=1}{fromTail=2}', 1, 6, 6)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (201, N'/html/body/div[2]{class=wrapper detail}/section{class=main}/section{class=content }/ul{class=block-key}{fromHead=2}{fromTail=13}', 1, 8, 6)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (202, NULL, 1, 2, 6)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (203, N'/html/body/div[2]{class=wrapper detail}/section{class=main}/section{class=content }/div{class=detail-content}/div{class=block-feature block-feature-1}/p{class=txt-head}{fromHead=2}{fromTail=1}', 1, 4, 6)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (204, N'/html/body/div[2]{class=wrapper detail}/section{class=main}/section{class=content }/div{class=detail-content}/div[2]{class=fck}{fromHead=3}{fromTail=3}', 1, 5, 6)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (211, N'/html/body/div{class=wapper}/div{class=padding10}/div{class=left w506 marginright10}/div{id=listcate}/h1{class=marginbottom10}{fromHead=1}{fromTail=14}', 1, 3, 1)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (212, N'/html/body/div{class=wapper}/div{class=padding10}/div{class=left w506 marginright10}/div{id=listcate}/div[2]{class=ngaycapnhat}{fromHead=2}{fromTail=13}', 1, 6, 1)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (213, NULL, 1, 8, 1)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (214, NULL, 1, 2, 1)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (215, N'/html/body/div{class=wapper}/div{class=padding10}/div{class=left w506 marginright10}/div{id=listcate}/div[5]{id=newscontents}/p{class=MsoNormal}/b{fromHead=0}{fromTail=1}', 1, 4, 1)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (216, N'/html/body/div{class=wapper}/div{class=padding10}/div{class=left w506 marginright10}/div{id=listcate}/div[5]{id=newscontents}{fromHead=5}{fromTail=10}', 1, 5, 1)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (217, N'/html/body/form{name=aspnetForm}{method=post}{action=truc-thang-bay-ra-truong-sa-cap-cuu-ngu-dan.aspx}{onsubmit=javascript:return WebForm_OnSubmit();}{id=aspnetForm}/div[6]{id=s4-workspace02}{class=WC}/div{id=s4-bodyContainer}/div[2]{id=s4-mainarea}{class=s4-pr s4-widecontentarea}/div{id=MSO_ContentTable}/div{class=s4-ba}/div{class=ms-bodyareacell}/div{id=ctl00_MSO_ContentDiv}{class=Main-TNO}/div{class=divOut}{style=width:1003px}/div{class=divLayout}/div[4]{class=wcCont}/table{class=tt-mainlayout}{style=width: 100%; background:#fff}{cellspacing=0}{cellpadding=0}/tbody/tr/td{valign=top}{align=left}/div/div[3]{class=tt-Layout news-detail}/div{class=tt-HomeTop}/table{cellpadding=0}{cellspacing=0}{width=100%}/tbody/tr/td{class=tt-left-content}{valign=top}/div[2]{class=article article-left}/div[3]{id=print-news}/div{class=article-header}/div[2]{class=title-new}/div{id=ctl00_PlaceHolderMain_ctl00_g_94e83511_a007_4f6a_bda1_a3c50067fb56}{__markuptype=vsattributemarkup}{__webpartid={94e83511-a007-4f6a-bda1-a3c50067fb56}}{webpart=true}/div{class=showcloumn bottom}/div[2]{class=article-header}{style=padding-top:10px;}/h1{class=mainTitle}{fromHead=0}{fromTail=2}', 1, 3, 5)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (218, N'/html/body/form{name=aspnetForm}{method=post}{action=truc-thang-bay-ra-truong-sa-cap-cuu-ngu-dan.aspx}{onsubmit=javascript:return WebForm_OnSubmit();}{id=aspnetForm}/div[6]{id=s4-workspace02}{class=WC}/div{id=s4-bodyContainer}/div[2]{id=s4-mainarea}{class=s4-pr s4-widecontentarea}/div{id=MSO_ContentTable}/div{class=s4-ba}/div{class=ms-bodyareacell}/div{id=ctl00_MSO_ContentDiv}{class=Main-TNO}/div{class=divOut}{style=width:1003px}/div{class=divLayout}/div[4]{class=wcCont}/table{class=tt-mainlayout}{style=width: 100%; background:#fff}{cellspacing=0}{cellpadding=0}/tbody/tr/td{valign=top}{align=left}/div/div[3]{class=tt-Layout news-detail}/div{class=tt-HomeTop}/table{cellpadding=0}{cellspacing=0}{width=100%}/tbody/tr/td{class=tt-left-content}{valign=top}/div[2]{class=article article-left}/div[3]{id=print-news}/div{class=article-header}/div[2]{class=title-new}/div{id=ctl00_PlaceHolderMain_ctl00_g_94e83511_a007_4f6a_bda1_a3c50067fb56}{__markuptype=vsattributemarkup}{__webpartid={94e83511-a007-4f6a-bda1-a3c50067fb56}}{webpart=true}/div{class=showcloumn bottom}/div[2]{class=article-header}{style=padding-top:10px;}/span{class=date-line}{fromHead=1}{fromTail=1}', 1, 6, 5)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (219, N'/html/body/form{name=aspnetForm}{method=post}{action=truc-thang-bay-ra-truong-sa-cap-cuu-ngu-dan.aspx}{onsubmit=javascript:return WebForm_OnSubmit();}{id=aspnetForm}/div[6]{id=s4-workspace02}{class=WC}/div{id=s4-bodyContainer}/div[2]{id=s4-mainarea}{class=s4-pr s4-widecontentarea}/div{id=MSO_ContentTable}/div{class=s4-ba}/div{class=ms-bodyareacell}/div{id=ctl00_MSO_ContentDiv}{class=Main-TNO}/div{class=divOut}{style=width:1003px}/div{class=divLayout}/div[4]{class=wcCont}/table{class=tt-mainlayout}{style=width: 100%; background:#fff}{cellspacing=0}{cellpadding=0}/tbody/tr/td{valign=top}{align=left}/div/div[3]{class=tt-Layout news-detail}/div{class=tt-HomeTop}/table{cellpadding=0}{cellspacing=0}{width=100%}/tbody/tr/td{class=tt-left-content}{valign=top}/div[2]{class=article article-left}/div[6]{class=tukhoaitem}/table{class=s4-wpTopTable}{border=0}{cellpadding=0}{cellspacing=0}{width=100%}/tbody/tr/td{valign=top}/div{webpartid=00000000-0000-0000-0000-000000000000}{haspers=true}{id=WebPartWPQ8}{width=100%}{class=ms-WPBody noindex}{onlyformepart=true}{allowdelete=false}{style=}/div{id=ctl00_PlaceHolderMain_ctl00_g_4a226e33_d02e_4edb_83e5_438580547644}/div{class=hienthiTags}/div{class=item}/div{class=tags}{fromHead=1}{fromTail=1}', 1, 8, 5)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (220, NULL, 1, 2, 5)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (221, N'/html/body/form{name=aspnetForm}{method=post}{action=truc-thang-bay-ra-truong-sa-cap-cuu-ngu-dan.aspx}{onsubmit=javascript:return WebForm_OnSubmit();}{id=aspnetForm}/div[6]{id=s4-workspace02}{class=WC}/div{id=s4-bodyContainer}/div[2]{id=s4-mainarea}{class=s4-pr s4-widecontentarea}/div{id=MSO_ContentTable}/div{class=s4-ba}/div{class=ms-bodyareacell}/div{id=ctl00_MSO_ContentDiv}{class=Main-TNO}/div{class=divOut}{style=width:1003px}/div{class=divLayout}/div[4]{class=wcCont}/table{class=tt-mainlayout}{style=width: 100%; background:#fff}{cellspacing=0}{cellpadding=0}/tbody/tr/td{valign=top}{align=left}/div/div[3]{class=tt-Layout news-detail}/div{class=tt-HomeTop}/table{cellpadding=0}{cellspacing=0}{width=100%}/tbody/tr/td{class=tt-left-content}{valign=top}/div[2]{class=article article-left}/div[3]{id=print-news}/div[2]{class=article-ds}/div[2]{class=article-content article-content03}/h2{class=ms-rteElement-H2}{fromHead=0}{fromTail=9}', 1, 4, 5)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (222, N'/html/body/form{name=aspnetForm}{method=post}{action=truc-thang-bay-ra-truong-sa-cap-cuu-ngu-dan.aspx}{onsubmit=javascript:return WebForm_OnSubmit();}{id=aspnetForm}/div[6]{id=s4-workspace02}{class=WC}/div{id=s4-bodyContainer}/div[2]{id=s4-mainarea}{class=s4-pr s4-widecontentarea}/div{id=MSO_ContentTable}/div{class=s4-ba}/div{class=ms-bodyareacell}/div{id=ctl00_MSO_ContentDiv}{class=Main-TNO}/div{class=divOut}{style=width:1003px}/div{class=divLayout}/div[4]{class=wcCont}/table{class=tt-mainlayout}{style=width: 100%; background:#fff}{cellspacing=0}{cellpadding=0}/tbody/tr/td{valign=top}{align=left}/div/div[3]{class=tt-Layout news-detail}/div{class=tt-HomeTop}/table{cellpadding=0}{cellspacing=0}{width=100%}/tbody/tr/td{class=tt-left-content}{valign=top}/div[2]{class=article article-left}/div[3]{id=print-news}/div[2]{class=article-ds}/div[2]{class=article-content article-content03}{fromHead=2}{fromTail=1}', 1, 5, 5)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (223, N'/html/body/div[6]{class=wrapper}/div{class=container}/div/div{class=row}/div{class=col-sm-12 col-md-8 col-home-left}{style=border-right: 1px solid #eeeeee}/h1{style=margin: 10px 0; color: #111111; font-size: 30px; line-height: 38px; font-weight: bold;}{fromHead=1}{fromTail=11}', 1, 3, 11)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (224, N'/html/body/div[6]{class=wrapper}/div{class=container}/div/div{class=row}/div{class=col-sm-12 col-md-8 col-home-left}{style=border-right: 1px solid #eeeeee}/div[3]/div{class=article-controls}{fromHead=0}{fromTail=1}', 1, 6, 11)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (225, N'/html/body/div[6]{class=wrapper}/div{class=container}/div/div{class=row}/div{class=col-sm-12 col-md-8 col-home-left}{style=border-right: 1px solid #eeeeee}/div[6]{class=news-tags}{fromHead=6}{fromTail=6}', 1, 8, 11)
GO
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (226, NULL, 1, 2, 11)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (227, N'/html/body/div[6]{class=wrapper}/div{class=container}/div/div{class=row}/div{class=col-sm-12 col-md-8 col-home-left}{style=border-right: 1px solid #eeeeee}/div[4]{class=listpost-container}{style=clear:both}/div{class=row stickem-container}{style=clear:both}/div[2]{class=col-md-9}/div{class=post-body}/p{class=news-content-excerpt}/strong{fromHead=0}{fromTail=1}', 1, 4, 11)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (228, N'/html/body/div[6]{class=wrapper}/div{class=container}/div/div{class=row}/div{class=col-sm-12 col-md-8 col-home-left}{style=border-right: 1px solid #eeeeee}/div[4]{class=listpost-container}{style=clear:both}/div{class=row stickem-container}{style=clear:both}/div[2]{class=col-md-9}/div{class=post-body}{fromHead=0}{fromTail=1}', 1, 5, 11)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (229, N'/html/body/div[6]{class=wrapper}/div{class=container}/div/div{class=row}/div{class=col-sm-12 col-md-8 col-home-left}{style=border-right: 1px solid #eeeeee}/h1{style=margin: 10px 0; color: #111111; font-size: 30px; line-height: 38px; font-weight: bold;}{fromHead=1}{fromTail=9}', 2, 3, 11)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (230, N'/html/body/div[6]{class=wrapper}/div{class=container}/div/div{class=row}/div{class=col-sm-12 col-md-8 col-home-left}{style=border-right: 1px solid #eeeeee}/div[3]/div{class=article-controls}{fromHead=0}{fromTail=1}', 2, 6, 11)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (231, N'/html/body/div[6]{class=wrapper}/div{class=container}/div/div{class=row}/div{class=col-sm-12 col-md-8 col-home-left}{style=border-right: 1px solid #eeeeee}/div[6]{class=box-info mg}{id=comments}/div{class=tab-content}/div{class=tab-pane active}{id=news-comments}/div{id=commentList}/div{align=center}{fromHead=0}{fromTail=1}', 2, 8, 11)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (232, NULL, 2, 2, 11)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (233, N'/html/body/div[6]{class=wrapper}/div{class=container}/div/div{class=row}/div{class=col-sm-12 col-md-8 col-home-left}{style=border-right: 1px solid #eeeeee}/div[4]{class=listpost-container}{style=clear:both}/div{class=row stickem-container}{style=clear:both}/div[2]{class=col-md-9}/div{class=post-body}/p{class=news-content-excerpt}/strong{fromHead=0}{fromTail=1}', 2, 4, 11)
INSERT [dbo].[ArticleWebElement] ([Id], [Address], [Group], [WebElementTypeId], [NewspaperId]) VALUES (234, N'/html/body/div[6]{class=wrapper}/div{class=container}/div/div{class=row}/div{class=col-sm-12 col-md-8 col-home-left}{style=border-right: 1px solid #eeeeee}/div[4]{class=listpost-container}{style=clear:both}/div{class=row stickem-container}{style=clear:both}/div[2]{class=col-md-9}/div{class=post-body}{fromHead=0}{fromTail=1}', 2, 5, 11)
SET IDENTITY_INSERT [dbo].[ArticleWebElement] OFF
SET IDENTITY_INSERT [dbo].[CollectorConfigurations] ON 

INSERT [dbo].[CollectorConfigurations] ([Id], [Name], [Value], [Description]) VALUES (1, N'CollectingApproach', N'PointOfTimes', N'')
INSERT [dbo].[CollectorConfigurations] ([Id], [Name], [Value], [Description]) VALUES (2, N'CollectingTimes', N'06:00:00;18:00:00', N'')
INSERT [dbo].[CollectorConfigurations] ([Id], [Name], [Value], [Description]) VALUES (3, N'CollectingInterval', N'01:02:03', N'')
INSERT [dbo].[CollectorConfigurations] ([Id], [Name], [Value], [Description]) VALUES (4, N'CollectingDelay', N'03:02:01', N'')
INSERT [dbo].[CollectorConfigurations] ([Id], [Name], [Value], [Description]) VALUES (5, N'CollectingMode', N'Manual', N'')
INSERT [dbo].[CollectorConfigurations] ([Id], [Name], [Value], [Description]) VALUES (6, N'IsCollectorBusy', N'False', N'')
INSERT [dbo].[CollectorConfigurations] ([Id], [Name], [Value], [Description]) VALUES (7, N'IsPagingCollecting', N'False', N'')
INSERT [dbo].[CollectorConfigurations] ([Id], [Name], [Value], [Description]) VALUES (8, N'CollectorInfo', N'{"Status":0,"Progress":0.0,"StartTime":"2014-12-15T18:58:35.3940519+07:00","EndTime":"2014-12-15T18:58:49.375299+07:00","ArticlesCount":0,"RecentCollectedArticle":null,"IsCollectorBusy":false}', NULL)
INSERT [dbo].[CollectorConfigurations] ([Id], [Name], [Value], [Description]) VALUES (9, N'RequestStopCollectNews', N'False', NULL)
INSERT [dbo].[CollectorConfigurations] ([Id], [Name], [Value], [Description]) VALUES (10, N'RequestCollectNews', N'False', NULL)
INSERT [dbo].[CollectorConfigurations] ([Id], [Name], [Value], [Description]) VALUES (11, N'NumberOfPageToCrawl', N'1', NULL)
SET IDENTITY_INSERT [dbo].[CollectorConfigurations] OFF
INSERT [dbo].[FFRelationInstance] ([FieldId1], [FieldId2], [NRelationId]) VALUES (31, 32, 1)
SET IDENTITY_INSERT [dbo].[Field] ON 

INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (1, N'Chính trị', N'http://baobinhduong.vn/chinh-tri/', NULL, 1, 2, NULL, CAST(0xA3980294 AS SmallDateTime), 1, 1)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (2, N'Kinh tế', N'http://baobinhduong.vn/kinh-te/', NULL, 1, 2, NULL, CAST(0xA3980294 AS SmallDateTime), 1, 2)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (3, N'Quốc tế', N'http://baobinhduong.vn/quoc-te/', NULL, 1, 2, NULL, CAST(0xA3980295 AS SmallDateTime), 1, 3)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (4, N'Xã hội', N'http://baobinhduong.vn/xa-hoi/', NULL, 1, 2, NULL, CAST(0xA3980295 AS SmallDateTime), 1, 4)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (5, N'Pháp luật', N'http://baobinhduong.vn/phap-luat/', NULL, 1, 2, NULL, CAST(0xA3980296 AS SmallDateTime), 1, 5)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (6, N'Thể thao', N'http://baobinhduong.vn/the-thao/', NULL, 1, 2, NULL, CAST(0xA3980296 AS SmallDateTime), 1, 6)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (7, N'Phân tích', N'http://baobinhduong.vn/phan-tich/', NULL, 1, 2, NULL, CAST(0xA3980297 AS SmallDateTime), 1, 7)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (8, N'Xã hội', N'http://dantri.com.vn/xa-hoi.htm', NULL, 1, 1, NULL, CAST(0xA398029A AS SmallDateTime), 2, 4)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (9, N'Thế giới', N'http://dantri.com.vn/the-gioi.htm', NULL, 1, 1, NULL, CAST(0xA398029B AS SmallDateTime), 2, 3)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (10, N'Giáo dục', N'http://dantri.com.vn/giao-duc-khuyen-hoc.htm', NULL, 1, 1, NULL, CAST(0xA398029C AS SmallDateTime), 2, 8)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (11, N'Kinh doanh', N'http://dantri.com.vn/kinh-doanh.htm', NULL, 1, 1, NULL, CAST(0xA398029C AS SmallDateTime), 2, 9)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (12, N'Nhân ái', N'http://dantri.com.vn/tam-long-nhan-ai.htm', NULL, 1, 1, NULL, CAST(0xA398029D AS SmallDateTime), 2, 10)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (13, N'Văn hóa', N'http://dantri.com.vn/van-hoa.htm', NULL, 1, 1, NULL, CAST(0xA398029D AS SmallDateTime), 2, 11)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (14, N'Pháp luật', N'http://dantri.com.vn/phap-luat.htm', NULL, 1, 1, NULL, CAST(0xA398029D AS SmallDateTime), 2, 5)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (15, N'Thời sự', N'http://vnexpress.net/tin-tuc/thoi-su', NULL, 1, 1, NULL, CAST(0xA39802AB AS SmallDateTime), 3, 15)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (16, N'Thế giới', N'http://vnexpress.net/tin-tuc/the-gioi', NULL, 1, 1, NULL, CAST(0xA39802AC AS SmallDateTime), 3, 3)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (17, N'Kinh doanh', N'http://kinhdoanh.vnexpress.net/', NULL, 1, 2, NULL, CAST(0xA39802AD AS SmallDateTime), 3, 9)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (18, N'Giải trí', N'http://giaitri.vnexpress.net/', NULL, 1, 2, NULL, CAST(0xA39802AF AS SmallDateTime), 3, 16)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (19, N'Thể thao', N'http://thethao.vnexpress.net/', NULL, 1, 2, NULL, CAST(0xA39802B0 AS SmallDateTime), 3, 6)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (20, N'Pháp luật', N'http://vnexpress.net/tin-tuc/phap-luat/', NULL, 1, 2, NULL, CAST(0xA39802B1 AS SmallDateTime), 3, 5)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (21, N'Kinh tế', N'http://nld.com.vn/kinh-te.htm', NULL, 1, 2, NULL, CAST(0xA39802B2 AS SmallDateTime), 4, 2)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (22, N'Giáo dục', N'http://nld.com.vn/giao-duc-khoa-hoc.htm', NULL, 1, 2, NULL, CAST(0xA39802B3 AS SmallDateTime), 4, 8)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (23, N'Pháp luật', N'http://nld.com.vn/phap-luat.htm', NULL, 1, 2, NULL, CAST(0xA39802B3 AS SmallDateTime), 4, 5)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (24, N'Công đoàn', N'http://nld.com.vn/cong-doan.htm', NULL, 1, 2, NULL, CAST(0xA39802B3 AS SmallDateTime), 4, 17)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (25, N'Quốc tế', N'http://nld.com.vn/thoi-su-quoc-te.htm', NULL, 1, 2, NULL, CAST(0xA39802CE AS SmallDateTime), 4, 3)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (26, N'Chính trị - Xã hội', N'http://www.thanhnien.com.vn/pages/chinh-tri-xa-hoi.aspx', NULL, 1, 1, NULL, CAST(0xA39802D0 AS SmallDateTime), 5, 18)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (27, N'Quốc phòng', N'http://www.thanhnien.com.vn/pages/quoc-phong.aspx', NULL, 1, 1, NULL, CAST(0xA39802D2 AS SmallDateTime), 5, 19)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (28, N'Kinh tế', N'http://www.thanhnien.com.vn/pages/kinh-te.aspx', NULL, 1, 1, NULL, CAST(0xA39802D3 AS SmallDateTime), 5, 2)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (29, N'Thế giới', N'http://www.thanhnien.com.vn/pages/the-gioi.aspx', NULL, 1, 1, NULL, CAST(0xA39802D4 AS SmallDateTime), 5, 3)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (31, N'Chính trị - Xã hội', N'http://tuoitre.vn/tin/chinh-tri-xa-hoi', NULL, 1, 1, NULL, CAST(0xA39802D7 AS SmallDateTime), 6, 18)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (32, N'Pháp luật', N'http://tuoitre.vn/tin/phap-luat', NULL, 1, 1, NULL, CAST(0xA39802D9 AS SmallDateTime), 6, 5)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (33, N'Thế giới', N'http://tuoitre.vn/tin/the-gioi', NULL, 1, 1, NULL, CAST(0xA39802DA AS SmallDateTime), 6, 3)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (34, N'Kinh tế', N'http://tuoitre.vn/tin/kinh-te', NULL, 1, 1, NULL, CAST(0xA39802DA AS SmallDateTime), 6, 2)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (35, N'Giáo dục', N'http://tuoitre.vn/tin/giao-duc', NULL, 1, 1, NULL, CAST(0xA39802DB AS SmallDateTime), 6, 8)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (36, N'Xã hội', N'http://vietnamnet.vn/vn/xa-hoi/', NULL, 1, 1, NULL, CAST(0xA39802DF AS SmallDateTime), 7, 4)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (37, N'Công nghệ thông tin', N'http://vietnamnet.vn/vn/cong-nghe-thong-tin-vien-thong/', NULL, 1, 1, NULL, CAST(0xA39802E0 AS SmallDateTime), 7, 20)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (38, N'Giáo dục', N'http://vietnamnet.vn/vn/giao-duc/', NULL, 1, 1, NULL, CAST(0xA39802E0 AS SmallDateTime), 7, 8)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (39, N'Chính trị', N'http://vietnamnet.vn/vn/chinh-tri/', NULL, 1, 1, NULL, CAST(0xA39802E1 AS SmallDateTime), 7, 1)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (40, N'Đời sống', N'http://vietnamnet.vn/vn/doi-song/', NULL, 1, 1, NULL, CAST(0xA39802E1 AS SmallDateTime), 7, 21)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (41, N'Kinh tế', N'http://vietnamnet.vn/vn/kinh-te/', NULL, 1, 1, NULL, CAST(0xA39802E1 AS SmallDateTime), 7, 2)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (42, N'Quốc tế', N'http://vietnamnet.vn/vn/kinh-te/', NULL, 1, 1, NULL, CAST(0xA39802E2 AS SmallDateTime), 7, 3)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (43, N'Văn hóa', N'http://vietnamnet.vn/vn/van-hoa/', NULL, 1, 2, NULL, CAST(0xA39802E2 AS SmallDateTime), 7, 11)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (44, N'Thời sự', N'http://vneconomy.vn/thoi-su.htm', NULL, 1, 1, NULL, CAST(0xA39802E8 AS SmallDateTime), 8, 15)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (46, N'Văn hóa - Giải trí', N'http://www.thanhnien.com.vn/pages/van-hoa-nghe-thuat.aspx', NULL, 1, 1, NULL, CAST(0xA3980470 AS SmallDateTime), 5, 11)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (47, N'Tài chính', N'http://vneconomy.vn/tai-chinh.htm', NULL, 1, 1, NULL, CAST(0xA3980472 AS SmallDateTime), 8, 22)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (48, N'Chứng khoán', N'http://vneconomy.vn/chung-khoan.htm', NULL, 1, 1, NULL, CAST(0xA3980473 AS SmallDateTime), 8, 23)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (49, N'Doanh nhân', N'http://vneconomy.vn/doanh-nhan.htm', NULL, 1, 1, NULL, CAST(0xA3980473 AS SmallDateTime), 8, 24)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (50, N'Địa ốc', N'http://vneconomy.vn/bat-dong-san.htm', NULL, 1, 1, NULL, CAST(0xA3980473 AS SmallDateTime), 8, 25)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (51, N'Thị trường', N'http://vneconomy.vn/thi-truong.htm', NULL, 1, 1, NULL, CAST(0xA3980474 AS SmallDateTime), 8, 26)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (52, N'Thế giới', N'http://vneconomy.vn/the-gioi.htm', NULL, 0, 1, NULL, CAST(0xA3980474 AS SmallDateTime), 8, 3)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (53, N'Kinh tế', N'http://laodong.com.vn/kinh-doanh/', NULL, 1, 1, NULL, CAST(0xA3BC048C AS SmallDateTime), 9, 2)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (54, N'Công đoàn', N'http://laodong.com.vn/cong-doan/', NULL, 1, 1, NULL, CAST(0xA3BC0490 AS SmallDateTime), 9, 12)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (55, N'Việc làm', N'http://dantri.com.vn/viec-lam.htm', NULL, 1, 2, NULL, CAST(0xA3BF024B AS SmallDateTime), 2, 12)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (56, N'Việc làm', N'http://vieclam.nld.com.vn/cam-nang/viec-lam.html', NULL, 1, 2, NULL, CAST(0xA3BF0250 AS SmallDateTime), 4, 12)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (57, N'Việc làm sinh viên', N'http://vieclam.nld.com.vn/cam-nang/Dua-viec-lam-den-sinh-vien-1-nc.html', NULL, 1, 1, NULL, CAST(0xA3BF0258 AS SmallDateTime), 10, 12)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (58, N'Doanh nghiệp', N'http://vieclam.nld.com.vn/cam-nang/Gioi-thieu-doanh-nghiep-2-nc.html', NULL, 1, 1, NULL, CAST(0xA3BF025A AS SmallDateTime), 10, 12)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (59, N'Xuất khẩu lao động', N'http://vieclam.nld.com.vn/cam-nang/Xuat-khau-lao-dong-3-nc.html', NULL, 1, 1, NULL, CAST(0xA3BF025B AS SmallDateTime), 10, 12)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (60, N'Khóa đào tạo', N'http://vieclam.nld.com.vn/cam-nang/Khoa-dao-tao-9-nc.html', NULL, 1, 1, NULL, CAST(0xA3BF025B AS SmallDateTime), 10, 12)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (61, N'Thị trường lao động', N'http://vieclam.nld.com.vn/cam-nang/Thi-truong-lao-dong-4-nc.html', NULL, 1, 1, NULL, CAST(0xA3BF025C AS SmallDateTime), 10, 12)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (62, N'Nghệ thuật quản trị', N'http://vieclam.nld.com.vn/cam-nang/Nghe-thuat-quan-tri-5-nc.html', NULL, 1, 1, NULL, CAST(0xA3BF025C AS SmallDateTime), 10, 12)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (63, N'Kỹ năng nghề nghiệp', N'http://vieclam.nld.com.vn/cam-nang/Ky-nang-nghe-nghiep-6-nc.html', NULL, 1, 1, NULL, CAST(0xA3BF025D AS SmallDateTime), 10, 12)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (64, N'Pháp luật lao động', N'http://vieclam.nld.com.vn/cam-nang/Tu-van-phap-luat-7-nc.html', NULL, 1, 1, NULL, CAST(0xA3BF025D AS SmallDateTime), 10, 12)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (65, N'Văn bản pháp luật', N'http://vieclam.nld.com.vn/cam-nang/Van-ban-phap-luat-8-nc.html', NULL, 1, 1, NULL, CAST(0xA3BF025E AS SmallDateTime), 10, 12)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (66, N'Kinh tế', N'http://baolaodongthudo.com.vn/kinh-doanh', NULL, 1, 1, NULL, CAST(0xA3C00219 AS SmallDateTime), 11, 2)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (67, N'Công đoàn', N'http://baolaodongthudo.com.vn/cong-doan', NULL, 1, 1, NULL, CAST(0xA3C10257 AS SmallDateTime), 11, 17)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (68, N'Thời sự', N'http://baolaodongthudo.com.vn/thoi-su', NULL, 1, 1, NULL, CAST(0xA3C10258 AS SmallDateTime), 11, 15)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (69, N'Xã hội', N'http://baolaodongthudo.com.vn/xa-hoi/65', NULL, 0, NULL, NULL, CAST(0xA3C1025A AS SmallDateTime), 11, 4)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (70, N'Pháp luật', N'http://baolaodongthudo.com.vn/phap-luat', NULL, 1, 1, NULL, CAST(0xA3C1025A AS SmallDateTime), 11, 5)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (71, N'Quốc tế', N'http://baolaodongthudo.com.vn/quoc-te/61', NULL, 0, NULL, NULL, CAST(0xA3C1025B AS SmallDateTime), 11, 3)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (72, N'Gia đình', N'http://baolaodongthudo.com.vn/gia-dinh/5', NULL, 0, NULL, NULL, CAST(0xA3C1025B AS SmallDateTime), 11, 27)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (73, N'Sứ khỏe', N'http://baolaodongthudo.com.vn/suc-khoe/45', NULL, 0, NULL, NULL, CAST(0xA3C1025C AS SmallDateTime), 11, 28)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (74, N'Văn hóa', N'http://baolaodongthudo.com.vn/van-hoa/25', NULL, 0, NULL, NULL, CAST(0xA3C1025C AS SmallDateTime), 11, 11)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (75, N'Thể thao', N'http://baolaodongthudo.com.vn/the-thao/18', NULL, 0, NULL, NULL, CAST(0xA3C1025D AS SmallDateTime), 11, 6)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (76, N'Giáo dục', N'http://baolaodongthudo.com.vn/giao-duc/58', NULL, 0, NULL, NULL, CAST(0xA3C1025D AS SmallDateTime), 11, 8)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (77, N'Việc làm', N'http://baolaodongthudo.com.vn/viec-lam', NULL, 1, 1, NULL, CAST(0xA3C1025D AS SmallDateTime), 11, 12)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (78, N'Bạn đọc', N'http://baolaodongthudo.com.vn/ban-doc/63', NULL, 0, NULL, NULL, CAST(0xA3C1025E AS SmallDateTime), 11, 29)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (79, N'Giải trí', N'http://baolaodongthudo.com.vn/giai-tri', NULL, 1, 1, NULL, CAST(0xA3D1026F AS SmallDateTime), 11, 16)
INSERT [dbo].[Field] ([Id], [Name], [Url], [Description], [IsActivated], [Group], [LastUpdateTime], [DefinedTime], [NewspaperId], [GFieldId]) VALUES (80, N'Xã hội', N'http://vtc.vn/xa-hoi.2.0.html', N'Trang lĩnh vực xã hội', 1, 1, NULL, CAST(0xA3FB024C AS SmallDateTime), 12, 4)
SET IDENTITY_INSERT [dbo].[Field] OFF
SET IDENTITY_INSERT [dbo].[FieldWebElement] ON 

INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (3, N'/html/body/form{name=aspnetForm}{method=post}{action=/Lists.aspx?Cat_ParentName=&Cat_ID=20&Cat_Name=xa-hoi&PageIndex=1&Cat_ParentID=20&PageType=3}{id=aspnetForm}/div[6]{class=wrapper}/div{class=container}/div{class=clearfix}/div{class=fl wid470}{fromHead=0}{fromTail=4}', 1, NULL, 1, 2)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (4, N'/html/body/form{name=aspnetForm}{method=post}{action=/Lists.aspx?Cat_ParentName=&Cat_ID=20&Cat_Name=xa-hoi&PageIndex=1&Cat_ParentID=20&PageType=3}{id=aspnetForm}/div[6]{class=wrapper}/div{class=container}/div{class=clearfix}/div{class=fl wid470}/div[27]{class=fr mt1}/div[2]{class=fl}{fromHead=1}{fromTail=1}', 1, NULL, 2, 2)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (5, N'/html/body/div[3]{id=page}/div[3]{id=wrapper_container}/div{id=container}/div{class=width_common}/div[3]{class=width_common line_col}/div{id=col_1}/div{class=block_mid_new}{fromHead=0}{fromTail=1}', 1, NULL, 1, 3)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (6, N'/html/body/div[3]{id=page}/div[3]{id=wrapper_container}/div{id=container}/div{class=width_common}/div[3]{class=width_common line_col}/div{id=col_1}/div{class=block_mid_new}/div{class=bottom_pagination width_common}{fromHead=1}{fromTail=2}', 1, NULL, 2, 3)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (7, N'/html/body/div[2]{id=page}/div[3]{id=wrapper_container}/div{id=container}/div{class=width_common}/div[3]{class=width_common line_col}/div{id=col_1}/div{class=block_mid_new}{fromHead=0}{fromTail=1}', 2, NULL, 1, 3)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (8, N'/html/body/div[2]{id=page}/div[3]{id=wrapper_container}/div{id=container}/div{class=width_common}/div[3]{class=width_common line_col}/div{id=col_1}/div{class=block_mid_new}/div{class=bottom_pagination width_common}{fromHead=1}{fromTail=1}', 2, NULL, 2, 3)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (9, N'/html/body/form{method=post}{action=}{id=form1}/div[3]{id=pagewrap}/div[2]{id=content}/div{class=fl w660}/div[3]{class=shadow2 ml2}/div[2]{class=box2 ul-list}{fromHead=1}{fromTail=1}', 1, NULL, 1, 4)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (10, N'/html/body/form{method=post}{action=}{id=form1}/div[3]{id=pagewrap}/div[2]{id=content}/div{class=fl w660}/div[3]{class=shadow2 ml2}/div[2]{class=box2 ul-list}/div[11]{class=paging}{fromHead=10}{fromTail=2}', 1, NULL, 2, 4)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (26, N'/html/body/div{id=MainWraper}/div[4]{id=BodyWraper}/div{class=clearfix}/div{class=BodyLayout690 left m-r-10}/div[4]{class=clearfix}/div{class=BodyLayout460 left m-r-10}/div[2]{class=ArticleCateList}{fromHead=1}{fromTail=3}', 1, NULL, 1, 7)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (27, N'/html/body/div{id=MainWraper}/div[4]{id=BodyWraper}/div{class=clearfix}/div{class=BodyLayout690 left m-r-10}/div[2]{class=topCategoryNewsLastest}{fromHead=1}{fromTail=3}', 1, NULL, 1, 7)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (28, N'/html/body/div{id=MainWraper}/div[4]{id=BodyWraper}/div{class=clearfix}/div{class=BodyLayout690 left m-r-10}/div[4]{class=clearfix}/div{class=BodyLayout460 left m-r-10}/div[2]{class=ArticleCateList}/div[16]{class=ArticleCateListBottom}/div{class=clearfix}/div{class=left}{fromHead=0}{fromTail=2}', 1, NULL, 2, 7)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (29, N'/html/body/div{id=MainWraper}/div[4]{id=BodyWraper}/div{class=clearfix}/div{class=BodyLayout690 left m-r-10}{fromHead=0}{fromTail=2}', 2, NULL, 1, 7)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (30, N'', 2, NULL, 2, 7)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (31, N'/html/body/form{name=aspnetForm}{method=post}{action=/thoi-su.htm}{id=aspnetForm}/div[3]{class=wp980}/div[3]{class=content}/div{class=contentleft}{fromHead=0}{fromTail=3}', 1, NULL, 1, 8)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (32, N'/html/body/form{name=aspnetForm}{method=post}{action=/thoi-su.htm}{id=aspnetForm}/div[3]{class=wp980}/div[3]{class=content}/div{class=contentleft}/div{class=headerleftcm}/div[5]{class=pageindex}{fromHead=4}{fromTail=1}', 1, NULL, 2, 8)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (35, N'/html/body/div{class=wrapper category}/section{class=main gettop}/section{class=content }/div{class=left-side}{fromHead=0}{fromTail=2}', 1, NULL, 1, 6)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (36, N'', 1, NULL, 2, 6)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (37, N'/html/body/form{name=aspnetForm}{method=post}{action=/kinh-doanh/}{id=aspnetForm}/div[2]{id=container}{class=clearfix wrap-site}/div[2]{class=content}/section[2]{class=cat-browsing-news clearfix}{fromHead=3}{fromTail=9}', 1, NULL, 1, 9)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (38, N'/html/body/form{name=aspnetForm}{method=post}{action=/kinh-doanh/}{id=aspnetForm}/div[2]{id=container}{class=clearfix wrap-site}/div[2]{class=content}/section{class=cat-hl clearfix}/div[2]{class=pull-right}{fromHead=2}{fromTail=2}', 1, NULL, 1, 9)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (39, N'/html/body/form{name=aspnetForm}{method=post}{action=/kinh-doanh/}{id=aspnetForm}/div[2]{id=container}{class=clearfix wrap-site}/div[2]{class=content}/section[2]{class=cat-browsing-news clearfix}/div[2]{class=ld-pagination clearfix}{fromHead=18}{fromTail=1}', 1, NULL, 2, 9)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (40, N'/html/body/div{class=wrap}/div[2]{class=nld-body}/div{class=nld-container}/div[2]{class=nld-content}/div{class=page page-news}/div[2]{class=page-body}/div{class=list-news}{fromHead=0}{fromTail=1}', 1, NULL, 1, 10)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (41, N'/html/body/div{class=wrap}/div[2]{class=nld-body}/div{class=nld-container}/div[2]{class=nld-content}/div{class=page page-news}/div[2]{class=page-body}/div{class=list-news}/div[11]{class=pagination pagination-right}/ul{fromHead=1}{fromTail=1}', 1, NULL, 2, 10)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (45, N'/html/body/form{name=aspnetForm}{method=post}{action=/viec-lam.htm}{id=aspnetForm}/div[5]{class=wrapper}/div{class=container}/div{class=clearfix}/div{class=fl wid470}{fromHead=0}{fromTail=4}', 2, NULL, 1, 2)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (46, N'/html/body/form{name=aspnetForm}{method=post}{action=/viec-lam.htm}{id=aspnetForm}/div[5]{class=wrapper}/div{class=container}/div{class=clearfix}/div{class=fl wid470}/div[27]{class=fr mt1}/div[2]{class=fl}{fromHead=1}{fromTail=1}', 2, NULL, 2, 2)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (47, N'/html/body/div{class=wapper}/div{class=padding10}/div{class=left w506 marginright10}{fromHead=0}{fromTail=5}', 2, NULL, 1, 1)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (48, N'/html/body/div{class=wapper}/div{class=padding10}/div{class=left w506 marginright10}/div[16]{class=boxpage}/div[2]{class=right}{id=page}{fromHead=3}{fromTail=1}', 2, NULL, 2, 1)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (49, N'/html/body/div[2]{id=admWrapsite}/form{method=post}{action=}{id=form1}/div[3]{id=pagewrap}/div[2]{id=content}/div{class=fl w660}/div[3]{class=shadow2 ml2}/div[2]{class=box2 ul-list}{fromHead=1}{fromTail=1}', 2, NULL, 1, 4)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (50, N'/html/body/div[2]{id=admWrapsite}/form{method=post}{action=}{id=form1}/div[3]{id=pagewrap}/div[2]{id=content}/div{class=fl w660}/div[3]{class=shadow2 ml2}/div[2]{class=box2 ul-list}/div[11]{class=paging}{fromHead=10}{fromTail=2}', 2, NULL, 2, 4)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (53, N'/html/body/div[6]{class=wrapper}/div{class=container}/div[3]/div{class=row}/div{class=col-sm-12 col-md-8 col-home-left}{style=border-right: 1px solid #eeeeee}/div[3]{class=row}/div{class=col-md-8}/div{class=box-info list-news}{style=margin-top: 0}/div{class=box-body}{style=padding-top: 0}{fromHead=0}{fromTail=1}', 1, NULL, 1, 11)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (54, N'/html/body/div[6]{class=wrapper}/div{class=container}/div[3]/div{class=row}/div{class=col-sm-12 col-md-8 col-home-left}{style=border-right: 1px solid #eeeeee}/div{class=row r7}{fromHead=0}{fromTail=5}', 1, NULL, 1, 11)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (55, N'/html/body/div[6]{class=wrapper}/div{class=container}/div[3]/div{class=row}/div{class=col-sm-12 col-md-8 col-home-left}{style=border-right: 1px solid #eeeeee}/div[3]{class=row}/div{class=col-md-8}/div[2]{class=paging}/div{class=row}/div{class=col-md-12}/div{class=pagination pull-left}{fromHead=0}{fromTail=1}', 1, NULL, 2, 11)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (58, N'/html/body/form{name=aspnetForm}{method=post}{action=chinh-tri-xa-hoi.aspx}{onsubmit=javascript:return WebForm_OnSubmit();}{id=aspnetForm}/div[7]{id=s4-workspace02}{class=WC}/div{id=s4-bodyContainer}/div[2]{id=s4-mainarea}{class=s4-pr s4-widecontentarea}/div{id=MSO_ContentTable}/div{class=s4-ba}/div{class=ms-bodyareacell}/div{id=ctl00_MSO_ContentDiv}{class=Main-TNO}/div{class=divOut}{style=width:1003px}/div{class=divLayout}/div[4]{class=wcCont}/table{class=tt-mainlayout}{style=width: 100%; background:#fff}{cellspacing=0}{cellpadding=0}/tbody/tr/td{valign=top}{align=left}/div/div[2]{class=tt-Layout tc-detail}{fromHead=1}{fromTail=1}', 1, NULL, 1, 5)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (59, N'', 1, NULL, 2, 5)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (60, N'/html/body/div{id=body}/div[9]/div[2]{class=phantrai}/div{class=box2}{style=padding-bottom: 10px;}/div[2]{id=ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_pnlTopNews}{fromHead=1}{fromTail=4}', 1, NULL, 1, 12)
INSERT [dbo].[FieldWebElement] ([Id], [Address], [Group], [Priority], [WebElementTypeId], [NewspaperId]) VALUES (61, N'/html/body/div{id=body}/div[9]/div[2]{class=phantrai}/div{class=box2}{style=padding-bottom: 10px;}/div[4]{id=ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_pnlOtherNews}/div[3]{class=bottom_control}{style=padding-top: 5px; padding-bottom: 5px; width: 480px;}/div[2]{style=float: right; padding-right: 5px;}{fromHead=1}{fromTail=1}', 1, NULL, 2, 12)
SET IDENTITY_INSERT [dbo].[FieldWebElement] OFF
SET IDENTITY_INSERT [dbo].[GField] ON 

INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (1, N'Chính trị', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (2, N'Kinh tế', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (3, N'Quốc tế', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (4, N'Xã hội', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (5, N'Pháp luật', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (6, N'Thể thao', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (7, N'Phân tích', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (8, N'Giáo dục', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (9, N'Kinh doanh', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (10, N'Nhân ái', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (11, N'Văn hóa', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (12, N'Lao động việc làm', NULL)
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (13, N'Đầu tư công - Đầu tư nước ngoài', NULL)
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (14, N'Tỉnh Bình Dương', NULL)
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (15, N'Thời sự', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (16, N'Giải trí', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (17, N'Công đoàn', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (18, N'Chính trị - Xã hội', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (19, N'Quốc phòng', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (20, N'Công nghệ thông tin', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (21, N'Đời sống', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (22, N'Tài chính', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (23, N'Chứng khoán', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (24, N'Doanh nhân', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (25, N'Địa ốc', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (26, N'Thị trường', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (27, N'Gia đình', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (28, N'Sứ khỏe', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
INSERT [dbo].[GField] ([Id], [Name], [Description]) VALUES (29, N'Bạn đọc', N'Lĩnh vực phân loại này được sinh ra tự động khi bạn định nghĩa trang lĩnh vực. Bạn có thể thay đổi lĩnh vực phân loại này')
SET IDENTITY_INSERT [dbo].[GField] OFF
SET IDENTITY_INSERT [dbo].[GGRelation] ON 

INSERT [dbo].[GGRelation] ([Id], [Notation], [Name], [Description], [MetaData]) VALUES (1, N'PARENT', N'Cha-Con', N'Quan hệ cha con giữa hai lĩnh vực', N'0001')
INSERT [dbo].[GGRelation] ([Id], [Notation], [Name], [Description], [MetaData]) VALUES (2, N'SYNONYM', N'Đồng nghĩa', N'Quan hệ đồng nghĩa giữa hai lĩnh vực', N'1011')
INSERT [dbo].[GGRelation] ([Id], [Notation], [Name], [Description], [MetaData]) VALUES (3, N'RELATED', N'Có liên quan', N'Quan hệ có liên quan giữa hai đối tượng', N'1011')
SET IDENTITY_INSERT [dbo].[GGRelation] OFF
INSERT [dbo].[GGRelationInstance] ([GFieldId1], [GFieldId2], [GGRelationId]) VALUES (1, 5, 1)
INSERT [dbo].[GGRelationInstance] ([GFieldId1], [GFieldId2], [GGRelationId]) VALUES (1, 17, 1)
INSERT [dbo].[GGRelationInstance] ([GFieldId1], [GFieldId2], [GGRelationId]) VALUES (1, 19, 1)
INSERT [dbo].[GGRelationInstance] ([GFieldId1], [GFieldId2], [GGRelationId]) VALUES (2, 9, 1)
INSERT [dbo].[GGRelationInstance] ([GFieldId1], [GFieldId2], [GGRelationId]) VALUES (2, 12, 1)
INSERT [dbo].[GGRelationInstance] ([GFieldId1], [GFieldId2], [GGRelationId]) VALUES (2, 13, 1)
INSERT [dbo].[GGRelationInstance] ([GFieldId1], [GFieldId2], [GGRelationId]) VALUES (2, 22, 1)
INSERT [dbo].[GGRelationInstance] ([GFieldId1], [GFieldId2], [GGRelationId]) VALUES (2, 23, 1)
INSERT [dbo].[GGRelationInstance] ([GFieldId1], [GFieldId2], [GGRelationId]) VALUES (2, 24, 1)
INSERT [dbo].[GGRelationInstance] ([GFieldId1], [GFieldId2], [GGRelationId]) VALUES (2, 25, 1)
INSERT [dbo].[GGRelationInstance] ([GFieldId1], [GFieldId2], [GGRelationId]) VALUES (2, 26, 1)
INSERT [dbo].[GGRelationInstance] ([GFieldId1], [GFieldId2], [GGRelationId]) VALUES (4, 10, 1)
INSERT [dbo].[GGRelationInstance] ([GFieldId1], [GFieldId2], [GGRelationId]) VALUES (11, 16, 1)
INSERT [dbo].[GGRelationInstance] ([GFieldId1], [GFieldId2], [GGRelationId]) VALUES (12, 2, 3)
INSERT [dbo].[GGRelationInstance] ([GFieldId1], [GFieldId2], [GGRelationId]) VALUES (12, 9, 3)
INSERT [dbo].[GGRelationInstance] ([GFieldId1], [GFieldId2], [GGRelationId]) VALUES (13, 2, 3)
INSERT [dbo].[GGRelationInstance] ([GFieldId1], [GFieldId2], [GGRelationId]) VALUES (13, 9, 3)
INSERT [dbo].[GGRelationInstance] ([GFieldId1], [GFieldId2], [GGRelationId]) VALUES (18, 1, 1)
INSERT [dbo].[GGRelationInstance] ([GFieldId1], [GFieldId2], [GGRelationId]) VALUES (18, 4, 1)
INSERT [dbo].[GGRelationInstance] ([GFieldId1], [GFieldId2], [GGRelationId]) VALUES (18, 7, 1)
INSERT [dbo].[GGRelationInstance] ([GFieldId1], [GFieldId2], [GGRelationId]) VALUES (18, 15, 1)
SET IDENTITY_INSERT [dbo].[Newspaper] ON 

INSERT [dbo].[Newspaper] ([Id], [Name], [Url], [IsLocal], [Description], [DefinedTime], [IsActivated]) VALUES (1, N'Bình Dương Online', N'http://baobinhduong.vn/default.aspx', 1, N'Cơ quan của Đảng bộ Đảng cộng sản Việt Nam tỉnh Bình Dương', CAST(0xA3980293 AS SmallDateTime), 1)
INSERT [dbo].[Newspaper] ([Id], [Name], [Url], [IsLocal], [Description], [DefinedTime], [IsActivated]) VALUES (2, N'Dân trí', N'http://dantri.com.vn/', 0, N'Cơ quan của TW Hội Khuyến học Việt Nam', CAST(0xA398029A AS SmallDateTime), 1)
INSERT [dbo].[Newspaper] ([Id], [Name], [Url], [IsLocal], [Description], [DefinedTime], [IsActivated]) VALUES (3, N'VNEXPRESS', N'http://vnexpress.net/', 0, N'Cơ quan chủ quản: Bộ Khoa học Công nghệ', CAST(0xA39802A1 AS SmallDateTime), 1)
INSERT [dbo].[Newspaper] ([Id], [Name], [Url], [IsLocal], [Description], [DefinedTime], [IsActivated]) VALUES (4, N'Người Lao Động', N'http://nld.com.vn/', 0, N'Báo Người Lao Động Điện tử – Tiếng nói của Liên đoàn Lao động TPHCM', CAST(0xA39802B1 AS SmallDateTime), 1)
INSERT [dbo].[Newspaper] ([Id], [Name], [Url], [IsLocal], [Description], [DefinedTime], [IsActivated]) VALUES (5, N'Thanh niên', N'http://www.thanhnien.com.vn/pages/default.aspx', 0, N'Diễn đàn của Hội liên hệp Thanh niên Việt Nam', CAST(0xA39802D0 AS SmallDateTime), 1)
INSERT [dbo].[Newspaper] ([Id], [Name], [Url], [IsLocal], [Description], [DefinedTime], [IsActivated]) VALUES (6, N'Tuổi trẻ Online', N'http://tuoitre.vn/', 0, N'Cơ quan của Đoàn Thanh niên Cộng sản Hồ Chí Minh', CAST(0xA39802D7 AS SmallDateTime), 1)
INSERT [dbo].[Newspaper] ([Id], [Name], [Url], [IsLocal], [Description], [DefinedTime], [IsActivated]) VALUES (7, N'VietNamNet', N'http://vietnamnet.vn/', 0, N'Cơ quan chủ quản: Bộ Thông tin và Truyền Thông', CAST(0xA39802DE AS SmallDateTime), 1)
INSERT [dbo].[Newspaper] ([Id], [Name], [Url], [IsLocal], [Description], [DefinedTime], [IsActivated]) VALUES (8, N'VnEconomy', N'http://vneconomy.vn/', 0, N' Bản quyền thuộc về VnEconomy, báo điện tử thuộc nhóm Thời báo Kinh tế Việt Nam', CAST(0xA39802E8 AS SmallDateTime), 1)
INSERT [dbo].[Newspaper] ([Id], [Name], [Url], [IsLocal], [Description], [DefinedTime], [IsActivated]) VALUES (9, N'Lao động', N'http://laodong.com.vn/', 0, N'Cơ quan của Tổng Liên đoàn Lao động Việt Nam', CAST(0xA3BC048B AS SmallDateTime), 1)
INSERT [dbo].[Newspaper] ([Id], [Name], [Url], [IsLocal], [Description], [DefinedTime], [IsActivated]) VALUES (10, N'Thông tin việc làm - Người Lao động', N'http://vieclam.nld.com.vn/cam-nang/viec-lam.html', 0, NULL, CAST(0xA3BF0257 AS SmallDateTime), 1)
INSERT [dbo].[Newspaper] ([Id], [Name], [Url], [IsLocal], [Description], [DefinedTime], [IsActivated]) VALUES (11, N'Báo lao động thủ đô', N'http://baolaodongthudo.com.vn/', 0, N'GIẤY PHÉP BÁO ĐIỆN TỬ SỐ: 247/GP-BTTTT CẤP NGÀY 09/02/2012', CAST(0xA3C00219 AS SmallDateTime), 0)
INSERT [dbo].[Newspaper] ([Id], [Name], [Url], [IsLocal], [Description], [DefinedTime], [IsActivated]) VALUES (12, N'VTC News', N'http://vtc.vn/', 0, N'Trang báo điện tử của VTC Online', CAST(0xA3FB0218 AS SmallDateTime), 1)
SET IDENTITY_INSERT [dbo].[Newspaper] OFF
INSERT [dbo].[NNRelationInstance] ([NewspaperId1], [NewspaperId2], [NRelationId]) VALUES (4, 10, 1)
SET IDENTITY_INSERT [dbo].[NRelation] ON 

INSERT [dbo].[NRelation] ([Id], [Notation], [Name], [Description]) VALUES (1, N'PARENT', N'Cha-Con', N'Quan hệ cha con giữa hai đối tượng')
INSERT [dbo].[NRelation] ([Id], [Notation], [Name], [Description]) VALUES (2, N'SYNONYM', N'Đồng nghĩa', N'Quan hệ đồng nghĩa giữa 2 đối tượng')
INSERT [dbo].[NRelation] ([Id], [Notation], [Name], [Description]) VALUES (3, N'RELATED', N'Có liên quan', N'Quan hệ có liên quan giữa hai đối tượng')
SET IDENTITY_INSERT [dbo].[NRelation] OFF
SET IDENTITY_INSERT [dbo].[Role] ON 

INSERT [dbo].[Role] ([Id], [Name], [Description]) VALUES (1, N'Administrator', N'Admin role')
INSERT [dbo].[Role] ([Id], [Name], [Description]) VALUES (2, N'NormalUser', N'Normal user role')
SET IDENTITY_INSERT [dbo].[Role] OFF
SET IDENTITY_INSERT [dbo].[Topic] ON 

INSERT [dbo].[Topic] ([Id], [Name], [Description], [Tags], [Keyphrases], [KeyphraseGraphs]) VALUES (1, N'Giải quyết việc làm', NULL, N'giải quyết việc làm;tư vấn việc làm;tạo việc làm;giới thiệu việc làm', N'{"LDVL":["giải quyết việc làm","tư vấn việc làm","tạo việc làm","giới thiệu việc làm"],"DT":[]}', NULL)
INSERT [dbo].[Topic] ([Id], [Name], [Description], [Tags], [Keyphrases], [KeyphraseGraphs]) VALUES (2, N'Thất nghiệp', NULL, N'thất nghiệp;thôi việc;không tìm được việc', N'{"LDVL":["thất nghiệp","người thất nghiệp","lao động thất nghiệp","giảm thất nghiệp","bảo hiểm thất nghiệp","trợ cấp thất nghiệp"],"DT":[]}', NULL)
INSERT [dbo].[Topic] ([Id], [Name], [Description], [Tags], [Keyphrases], [KeyphraseGraphs]) VALUES (3, N'Trợ cấp', NULL, N'trợ cấp;tiền trợ cấp;trợ cấp thôi việc;trợ cấp mất việc;trợ cấp thất nghiệp;giảm trợ cấp thất nghiệp;thất nghiệp;mất việc', N'{"LDVL":["trợ cấp","tiền trợ cấp","trợ cấp thôi việc","trợ cấp mất việc","trợ cấp thất nghiệp","giảm trợ cấp thất nghiệp"],"DT":[]}', NULL)
INSERT [dbo].[Topic] ([Id], [Name], [Description], [Tags], [Keyphrases], [KeyphraseGraphs]) VALUES (4, N'Tiền lương', NULL, N'lương;tiền lương;mức lương;trả lương;thang lương;bậc lương;bản lương;tạm ứng tiền lương;lương hưu;chính sách tiền lương', N'{"LDVL":["lương","tiền lương","mức lương","trả lương","thang lương","bậc lương","bảng lương","tạm ứng tiền lương","lương hưu","chính sách tiền lương"],"DT":[]}', NULL)
INSERT [dbo].[Topic] ([Id], [Name], [Description], [Tags], [Keyphrases], [KeyphraseGraphs]) VALUES (5, N'Hợp đồng lao động', NULL, N'hợp đồng;lao động;hợp đồng lao động;hợp đồng thử việc;hợp đồng vô thời hạn;chấm dứt hợp đồng lao động', N'{"LDVL":["hợp đồng lao động","chấm dứt hợp đồng lao động","hợp đồng thử việc","hợp đồng cho thuê lại lao động","hợp đồng đào tạo nghề"],"DT":[]}', NULL)
INSERT [dbo].[Topic] ([Id], [Name], [Description], [Tags], [Keyphrases], [KeyphraseGraphs]) VALUES (6, N'Đình công', NULL, N'đình công;biểu tình;ngừng việc tập thể', N'{"LDVL":["đình công","đóng cửa tạm thời","người lãnh đạo đình công","hoãn đình công","ngừng đình công","tính hợp pháp của đình công"],"DT":[]}', NULL)
INSERT [dbo].[Topic] ([Id], [Name], [Description], [Tags], [Keyphrases], [KeyphraseGraphs]) VALUES (7, N'Bảo hộ lao động', NULL, N'bảo hộ lao động;an toàn lao động;tai nạn lao động;bệnh nghề nghiệp;vệ sinh lao động;bảo hiểm lao động;chế độ bảo hộ lao động', N'{"LDVL":["bảo hộ lao động","an toàn lao động","tai nạn lao động","bệnh nghề nghiệp","vệ sinh lao động","bảo hiểm lao động","chế độ bảo hộ lao động"],"DT":[]}', NULL)
INSERT [dbo].[Topic] ([Id], [Name], [Description], [Tags], [Keyphrases], [KeyphraseGraphs]) VALUES (8, N'Tư vấn tuyển dụng', NULL, N'tư vấn tuyển dụng;tư vấn việc làm;tư vấn nghề nghiệp;tư vấn cho người lao động;tư vấn nghề', N'{"LDVL":["tư vấn việc làm","tư vấn nghề nghiệp","tư vấn cho người lao động","tư vấn nghề"],"DT":[]}', NULL)
INSERT [dbo].[Topic] ([Id], [Name], [Description], [Tags], [Keyphrases], [KeyphraseGraphs]) VALUES (9, N'Tranh chấp lao động', NULL, N'tranh chấp lao động;trọng tài viên lao động;hội đồng trọng tài lao động', N'{"LDVL":["tranh chấp lao động","tranh chấp lao động tập thể","tranh chấp lao động cá nhân","hòa giải viên lao động","trọng tài viên lao động","hội đồng trọng tài lao động","đình công"],"DT":[]}', NULL)
INSERT [dbo].[Topic] ([Id], [Name], [Description], [Tags], [Keyphrases], [KeyphraseGraphs]) VALUES (10, N'Bảo hiểm người lao động', NULL, N'bảo hiểm thất nghiệp;bảo hiểm xã hội;bảo hiểm y tế;BHXH;BHYT;bảo hiểm sức khỏe;BHNT;bảo hiểm nhân thọ', N'{"LDVL":["bảo hiểm thất nghiệp","bảo hiểm xã hội","bảo hiểm y tế","đóng bảo hiểm xã hội","bảo hiểm nhân thọ","bảo hiểm sức khỏe"],"DT":[]}', NULL)
INSERT [dbo].[Topic] ([Id], [Name], [Description], [Tags], [Keyphrases], [KeyphraseGraphs]) VALUES (11, N'Đầu tư trực tiếp nước ngoài', NULL, N'đầu tư;đầu tư nước ngoài;FDI;công ty xuyên quốc gia;vốn nước ngoài;khu công nghệ cao', N'{"LDVL":[],"DT":["FDI","vốn đầu tư FDI","công ty xuyên quốc gia","100% vốn nước ngoài","khu chế xuất","khu công nghiệp","khu công nghệ cao","khu thương mại tự do","đặc khu kinh tế"]}', NULL)
INSERT [dbo].[Topic] ([Id], [Name], [Description], [Tags], [Keyphrases], [KeyphraseGraphs]) VALUES (12, N'Đầu tư ODA', NULL, N'ODA;hỗ trợ phát triển chính thức', N'{"LDVL":[],"DT":["ODA","nguồn vốn ODA","vốn ODA","hỗ trợ phát triển chính thức"]}', NULL)
INSERT [dbo].[Topic] ([Id], [Name], [Description], [Tags], [Keyphrases], [KeyphraseGraphs]) VALUES (13, N'định chế tài chính quốc tế', NULL, N'quỹ tiền tệ quốc tế;ngân hàng thế giới;công ty tài chính quốc tế;ngân hàng phát triển Châu Á;câu lạc bộ xử lí nợ', N'{"LDVL":[],"DT":["quỹ tiền tệ quốc tế","Ngân hàng thế giới","công ty tài chính quốc tế","cơ quan bảo lãnh đầu tư đa phương","trung tâm giải quyết tranh chấp về đầu tư quốc tế","Ngân hàng phát triển Châu Á","câu lạc bộ xử lý nợ"]}', NULL)
INSERT [dbo].[Topic] ([Id], [Name], [Description], [Tags], [Keyphrases], [KeyphraseGraphs]) VALUES (14, N'Đấu thầu', NULL, N'đấu thầu;nhà thầu;bên mời thầu;mời thầu;phương thức đấu thầu', N'{"LDVL":[],"DT":["hoạt động đấu thầu","bên mời thầu","bên nhà thầu","hình thức chọn nhà thầu","phương thức đấu thầu","điều kiện thực hiện đấu thầu","hợp đồng trong đấu thầu","điều kiện đấu thầu quốc tế","hình thức đấu thầu"]}', NULL)
INSERT [dbo].[Topic] ([Id], [Name], [Description], [Tags], [Keyphrases], [KeyphraseGraphs]) VALUES (15, N'Thẩm định dự án đầu tư', NULL, N'thẩm định dựa án;hiệu quả đầu tư;dự án đầu tư;hiệu quả kinh tế;hiệu quả tài chính;thẩm định;dự án;đầu tư', N'{"LDVL":[],"DT":["phân tích rủi ro","chỉ tiêu đánh giá hiệu quả dự án","thẩm định hiệu quả kinh tế","thẩm định hiệu quả tài chính"]}', NULL)
INSERT [dbo].[Topic] ([Id], [Name], [Description], [Tags], [Keyphrases], [KeyphraseGraphs]) VALUES (16, N'Lập dự án đầu tư', NULL, N'dự án đầu tư;lập dự án;nghiên cứu thị trường;phân tích tài chính;phân tích kinh tế;hình thức đầu tư;địa điểm đầu tư', N'{"LDVL":[],"DT":["nghiên cứu tiền khả thi","nghiên cứu thị trường","nghiên cứu kỹ thuật","phân tích tài chính","phân tích kinh tế","phân tích ngân lưu dự án","chỉ tiêu đánh giá hiệu quả dự án","hình thức đầu tư","địa điểm đầu tư"]}', NULL)
INSERT [dbo].[Topic] ([Id], [Name], [Description], [Tags], [Keyphrases], [KeyphraseGraphs]) VALUES (17, N'Quan hệ kinh tế quốc tế', NULL, N'hội nhập;WTO;liên kết khu vực;hiệp định thương mại;hợp tác kinh tế', N'{"LDVL":[],"DT":["Hiệp định thương mại Việt - Mỹ","WTO","liên kết kinh tế khu vực","hội nhập ASEAN","hội nhập kinh tế quốc tế"]}', NULL)
INSERT [dbo].[Topic] ([Id], [Name], [Description], [Tags], [Keyphrases], [KeyphraseGraphs]) VALUES (18, N'Tài chính quốc tế', NULL, N'tài chính;quốc tế;tỷ giá hối đoái;ngoại tệ;ngoại hối;di chuyển nguồn vốn;cán cân thanh toán', N'{"LDVL":[],"DT":["tỷ giá hối đoái","cán cân thanh toán","hệ thống tiền tệ quốc tế","sự di chuyển nguồn vốn","quản lý nợ nước ngoài","định chế tài chính quốc tế","ngoại tệ","ngoại hối","thanh toán quốc tế","tín dụng quốc tế"]}', NULL)
INSERT [dbo].[Topic] ([Id], [Name], [Description], [Tags], [Keyphrases], [KeyphraseGraphs]) VALUES (19, N'Quản trị kinh doanh quốc tế', NULL, N'kinh doanh quốc tế;chiến lược toàn cầu;quản trị;tài chính quốc tế', N'{"LDVL":[],"DT":["hoạch định chiến lược toàn cầu","quản trị tài chính quốc tế"]}', NULL)
INSERT [dbo].[Topic] ([Id], [Name], [Description], [Tags], [Keyphrases], [KeyphraseGraphs]) VALUES (20, N'Thủ tục đầu tư', NULL, N'giấy chứng nhận đầu tư;đầu tư;thủ tục đầu tư;dự án đầu tư', N'{"LDVL":[],"DT":["giấy chứng nhận đầu tư","đăng ký dự án đầu tư","hồ sơ dự án đầu tư","thẩm tra dự án đầu tư"]}', NULL)
INSERT [dbo].[Topic] ([Id], [Name], [Description], [Tags], [Keyphrases], [KeyphraseGraphs]) VALUES (21, N'Lĩnh vực đầu tư', NULL, N'giao thông;thủy lợi;năng lượng;dầu khí;khoáng sản;casino;thuốc lá;luyện kim;công nghệ cao;cơ khí', N'{"LDVL":[],"DT":["giao thông","thủy lợi","năng lượng điện","dầu khí","khoáng sản","casino","thuốc lá","luyện kim"]}', NULL)
SET IDENTITY_INSERT [dbo].[Topic] OFF
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([Id], [Name], [Password], [RoleId]) VALUES (1, N'Admin', N'admin123', 1)
SET IDENTITY_INSERT [dbo].[User] OFF
SET IDENTITY_INSERT [dbo].[WebElementType] ON 

INSERT [dbo].[WebElementType] ([Id], [WENotation], [Name], [Description]) VALUES (1, N'LIST', N'Danh sách', N'Element chứa danh sách tin bài')
INSERT [dbo].[WebElementType] ([Id], [WENotation], [Name], [Description]) VALUES (2, N'PAGINATION', N'Phân trang', N'Element chứa phân trang')
INSERT [dbo].[WebElementType] ([Id], [WENotation], [Name], [Description]) VALUES (3, N'TITLE', N'Tiêu đề', N'Element chứa tiêu đề tin bài')
INSERT [dbo].[WebElementType] ([Id], [WENotation], [Name], [Description]) VALUES (4, N'ABSTRACT', N'Tóm tắt', N'Element chứa nội dung tóm tắt tin bài')
INSERT [dbo].[WebElementType] ([Id], [WENotation], [Name], [Description]) VALUES (5, N'CONTENT', N'Nội dung', N'Element chứa nội dung tin bài')
INSERT [dbo].[WebElementType] ([Id], [WENotation], [Name], [Description]) VALUES (6, N'DATETIME', N'Thời gian', N'Element chứa thời gian')
INSERT [dbo].[WebElementType] ([Id], [WENotation], [Name], [Description]) VALUES (7, N'AUTHOR', N'Danh sách', N'Element chứa tên tác giả của tin bài')
INSERT [dbo].[WebElementType] ([Id], [WENotation], [Name], [Description]) VALUES (8, N'TAGS', N'Danh sách', N'Element chứa các từ khóa, tags liên quan tới tin bài')
INSERT [dbo].[WebElementType] ([Id], [WENotation], [Name], [Description]) VALUES (9, N'RELATION', N'Danh sách', N'Element chứa danh sách các tin bài liên quan')
SET IDENTITY_INSERT [dbo].[WebElementType] OFF
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__tmp_ms_x__2CB664DCA2F18AD1]    Script Date: 12/17/2014 11:11:58 AM ******/
ALTER TABLE [dbo].[Article] ADD UNIQUE NONCLUSTERED 
(
	[Title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__tmp_ms_x__C5B21431FCA018BC]    Script Date: 12/17/2014 11:11:58 AM ******/
ALTER TABLE [dbo].[Article] ADD UNIQUE NONCLUSTERED 
(
	[Url] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__VisitedL__C5B10009E1B0E482]    Script Date: 12/17/2014 11:11:58 AM ******/
ALTER TABLE [dbo].[VisitedLink] ADD UNIQUE NONCLUSTERED 
(
	[URL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Article] ADD  DEFAULT ('') FOR [Title]
GO
ALTER TABLE [dbo].[Article] ADD  DEFAULT ('') FOR [Url]
GO
ALTER TABLE [dbo].[Article] ADD  DEFAULT ('') FOR [Abstract]
GO
ALTER TABLE [dbo].[Article] ADD  DEFAULT ('') FOR [Author]
GO
ALTER TABLE [dbo].[Article] ADD  DEFAULT ('') FOR [Tags]
GO
ALTER TABLE [dbo].[Article] ADD  DEFAULT ('') FOR [Content]
GO
ALTER TABLE [dbo].[Article] ADD  DEFAULT ('False') FOR [IsIndexed]
GO
ALTER TABLE [dbo].[Article] ADD  DEFAULT ('False') FOR [IsMark]
GO
ALTER TABLE [dbo].[Field] ADD  DEFAULT ('FALSE') FOR [IsActivated]
GO
ALTER TABLE [dbo].[Newspaper] ADD  DEFAULT ('False') FOR [IsLocal]
GO
ALTER TABLE [dbo].[Newspaper] ADD  DEFAULT ('False') FOR [IsActivated]
GO
ALTER TABLE [dbo].[SystemMessage] ADD  DEFAULT ('MESSAGE') FOR [Type]
GO
ALTER TABLE [dbo].[SystemMessage] ADD  DEFAULT ('False') FOR [IsRead]
GO
ALTER TABLE [dbo].[UserQuery] ADD  DEFAULT ('0') FOR [IsSaved]
GO
ALTER TABLE [dbo].[VisitedLink] ADD  DEFAULT ((1)) FOR [VisitCount]
GO
ALTER TABLE [dbo].[AARelationInstance]  WITH CHECK ADD  CONSTRAINT [FK_AARelationInstance_Article_1] FOREIGN KEY([ArticleId1])
REFERENCES [dbo].[Article] ([Id])
GO
ALTER TABLE [dbo].[AARelationInstance] CHECK CONSTRAINT [FK_AARelationInstance_Article_1]
GO
ALTER TABLE [dbo].[AARelationInstance]  WITH CHECK ADD  CONSTRAINT [FK_AARelationInstance_Article_2] FOREIGN KEY([ArticleId2])
REFERENCES [dbo].[Article] ([Id])
GO
ALTER TABLE [dbo].[AARelationInstance] CHECK CONSTRAINT [FK_AARelationInstance_Article_2]
GO
ALTER TABLE [dbo].[AARelationInstance]  WITH CHECK ADD  CONSTRAINT [FK_AARelationInstance_NRelation] FOREIGN KEY([NRelationId])
REFERENCES [dbo].[NRelation] ([Id])
GO
ALTER TABLE [dbo].[AARelationInstance] CHECK CONSTRAINT [FK_AARelationInstance_NRelation]
GO
ALTER TABLE [dbo].[Article]  WITH CHECK ADD  CONSTRAINT [FK_Article_Field] FOREIGN KEY([FieldId])
REFERENCES [dbo].[Field] ([Id])
GO
ALTER TABLE [dbo].[Article] CHECK CONSTRAINT [FK_Article_Field]
GO
ALTER TABLE [dbo].[Article]  WITH CHECK ADD  CONSTRAINT [FK_Article_GField] FOREIGN KEY([GFieldId])
REFERENCES [dbo].[GField] ([Id])
GO
ALTER TABLE [dbo].[Article] CHECK CONSTRAINT [FK_Article_GField]
GO
ALTER TABLE [dbo].[Article]  WITH CHECK ADD  CONSTRAINT [FK_Article_Newspaper] FOREIGN KEY([NewspaperId])
REFERENCES [dbo].[Newspaper] ([Id])
GO
ALTER TABLE [dbo].[Article] CHECK CONSTRAINT [FK_Article_Newspaper]
GO
ALTER TABLE [dbo].[ArticleKG]  WITH CHECK ADD  CONSTRAINT [FK_ArticleKG_Article] FOREIGN KEY([Id])
REFERENCES [dbo].[Article] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ArticleKG] CHECK CONSTRAINT [FK_ArticleKG_Article]
GO
ALTER TABLE [dbo].[ArticleWebElement]  WITH CHECK ADD  CONSTRAINT [FK_ArticleWebElement_Newspaper] FOREIGN KEY([NewspaperId])
REFERENCES [dbo].[Newspaper] ([Id])
GO
ALTER TABLE [dbo].[ArticleWebElement] CHECK CONSTRAINT [FK_ArticleWebElement_Newspaper]
GO
ALTER TABLE [dbo].[ArticleWebElement]  WITH CHECK ADD  CONSTRAINT [FK_ArticleWebElement_WebElementType] FOREIGN KEY([WebElementTypeId])
REFERENCES [dbo].[WebElementType] ([Id])
GO
ALTER TABLE [dbo].[ArticleWebElement] CHECK CONSTRAINT [FK_ArticleWebElement_WebElementType]
GO
ALTER TABLE [dbo].[FFRelationInstance]  WITH CHECK ADD  CONSTRAINT [FK_FFRelationInstance_Field_1] FOREIGN KEY([FieldId1])
REFERENCES [dbo].[Field] ([Id])
GO
ALTER TABLE [dbo].[FFRelationInstance] CHECK CONSTRAINT [FK_FFRelationInstance_Field_1]
GO
ALTER TABLE [dbo].[FFRelationInstance]  WITH CHECK ADD  CONSTRAINT [FK_FFRelationInstance_Field_2] FOREIGN KEY([FieldId2])
REFERENCES [dbo].[Field] ([Id])
GO
ALTER TABLE [dbo].[FFRelationInstance] CHECK CONSTRAINT [FK_FFRelationInstance_Field_2]
GO
ALTER TABLE [dbo].[FFRelationInstance]  WITH CHECK ADD  CONSTRAINT [FK_FFRelationInstance_NRelation] FOREIGN KEY([NRelationId])
REFERENCES [dbo].[NRelation] ([Id])
GO
ALTER TABLE [dbo].[FFRelationInstance] CHECK CONSTRAINT [FK_FFRelationInstance_NRelation]
GO
ALTER TABLE [dbo].[Field]  WITH CHECK ADD  CONSTRAINT [FK_Field_GField] FOREIGN KEY([GFieldId])
REFERENCES [dbo].[GField] ([Id])
GO
ALTER TABLE [dbo].[Field] CHECK CONSTRAINT [FK_Field_GField]
GO
ALTER TABLE [dbo].[Field]  WITH CHECK ADD  CONSTRAINT [FK_Field_Newspaper] FOREIGN KEY([NewspaperId])
REFERENCES [dbo].[Newspaper] ([Id])
GO
ALTER TABLE [dbo].[Field] CHECK CONSTRAINT [FK_Field_Newspaper]
GO
ALTER TABLE [dbo].[FieldWebElement]  WITH CHECK ADD  CONSTRAINT [FK_FieldWebElement_Newspaper] FOREIGN KEY([NewspaperId])
REFERENCES [dbo].[Newspaper] ([Id])
GO
ALTER TABLE [dbo].[FieldWebElement] CHECK CONSTRAINT [FK_FieldWebElement_Newspaper]
GO
ALTER TABLE [dbo].[FieldWebElement]  WITH CHECK ADD  CONSTRAINT [FK_FieldWebElement_WebElementType] FOREIGN KEY([WebElementTypeId])
REFERENCES [dbo].[WebElementType] ([Id])
GO
ALTER TABLE [dbo].[FieldWebElement] CHECK CONSTRAINT [FK_FieldWebElement_WebElementType]
GO
ALTER TABLE [dbo].[GGRelationInstance]  WITH CHECK ADD  CONSTRAINT [FK_GGRelationInstance_GField_1] FOREIGN KEY([GFieldId1])
REFERENCES [dbo].[GField] ([Id])
GO
ALTER TABLE [dbo].[GGRelationInstance] CHECK CONSTRAINT [FK_GGRelationInstance_GField_1]
GO
ALTER TABLE [dbo].[GGRelationInstance]  WITH CHECK ADD  CONSTRAINT [FK_GGRelationInstance_GField_2] FOREIGN KEY([GFieldId2])
REFERENCES [dbo].[GField] ([Id])
GO
ALTER TABLE [dbo].[GGRelationInstance] CHECK CONSTRAINT [FK_GGRelationInstance_GField_2]
GO
ALTER TABLE [dbo].[GGRelationInstance]  WITH CHECK ADD  CONSTRAINT [FK_GGRelationInstance_GGRelation] FOREIGN KEY([GGRelationId])
REFERENCES [dbo].[GGRelation] ([Id])
GO
ALTER TABLE [dbo].[GGRelationInstance] CHECK CONSTRAINT [FK_GGRelationInstance_GGRelation]
GO
ALTER TABLE [dbo].[NNRelationInstance]  WITH CHECK ADD  CONSTRAINT [FK_NNRelationInstance_Newspaper_1] FOREIGN KEY([NewspaperId1])
REFERENCES [dbo].[Newspaper] ([Id])
GO
ALTER TABLE [dbo].[NNRelationInstance] CHECK CONSTRAINT [FK_NNRelationInstance_Newspaper_1]
GO
ALTER TABLE [dbo].[NNRelationInstance]  WITH CHECK ADD  CONSTRAINT [FK_NNRelationInstance_Newspaper_2] FOREIGN KEY([NewspaperId2])
REFERENCES [dbo].[Newspaper] ([Id])
GO
ALTER TABLE [dbo].[NNRelationInstance] CHECK CONSTRAINT [FK_NNRelationInstance_Newspaper_2]
GO
ALTER TABLE [dbo].[NNRelationInstance]  WITH CHECK ADD  CONSTRAINT [FK_NNRelationInstance_NRelation] FOREIGN KEY([NRelationId])
REFERENCES [dbo].[NRelation] ([Id])
GO
ALTER TABLE [dbo].[NNRelationInstance] CHECK CONSTRAINT [FK_NNRelationInstance_NRelation]
GO
ALTER TABLE [dbo].[SavedArticle]  WITH CHECK ADD  CONSTRAINT [FK_SavedArticle_Article] FOREIGN KEY([ArticleId])
REFERENCES [dbo].[Article] ([Id])
GO
ALTER TABLE [dbo].[SavedArticle] CHECK CONSTRAINT [FK_SavedArticle_Article]
GO
ALTER TABLE [dbo].[SavedArticle]  WITH CHECK ADD  CONSTRAINT [FK_SavedArticle_Field] FOREIGN KEY([FieldId])
REFERENCES [dbo].[Field] ([Id])
GO
ALTER TABLE [dbo].[SavedArticle] CHECK CONSTRAINT [FK_SavedArticle_Field]
GO
ALTER TABLE [dbo].[SavedArticle]  WITH CHECK ADD  CONSTRAINT [FK_SavedArticle_GField] FOREIGN KEY([GFieldId])
REFERENCES [dbo].[GField] ([Id])
GO
ALTER TABLE [dbo].[SavedArticle] CHECK CONSTRAINT [FK_SavedArticle_GField]
GO
ALTER TABLE [dbo].[SavedArticle]  WITH CHECK ADD  CONSTRAINT [FK_SavedArticle_Newspaper] FOREIGN KEY([NewspaperId])
REFERENCES [dbo].[Newspaper] ([Id])
GO
ALTER TABLE [dbo].[SavedArticle] CHECK CONSTRAINT [FK_SavedArticle_Newspaper]
GO
ALTER TABLE [dbo].[SavedArticle]  WITH CHECK ADD  CONSTRAINT [FK_SavedArticle_UserQuery] FOREIGN KEY([UserQueryId])
REFERENCES [dbo].[UserQuery] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SavedArticle] CHECK CONSTRAINT [FK_SavedArticle_UserQuery]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
ALTER TABLE [dbo].[UserProfile]  WITH CHECK ADD  CONSTRAINT [FK_UserProfile_User] FOREIGN KEY([Id])
REFERENCES [dbo].[User] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserProfile] CHECK CONSTRAINT [FK_UserProfile_User]
GO
ALTER TABLE [dbo].[UserQuery]  WITH CHECK ADD  CONSTRAINT [FK_UserQuery_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserQuery] CHECK CONSTRAINT [FK_UserQuery_User]
GO
