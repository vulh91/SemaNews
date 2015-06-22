CREATE TABLE [dbo].[AARelationInstance]
(
	[ArticleId1] INT NOT NULL,
	[ArticleId2] INT NOT NULL,
	[NRelationId] INT NOT NULL,
	[RelationWeight] FLOAT NULL,
	CONSTRAINT [PK_AARelationInstance] PRIMARY KEY ([ArticleId1],[ArticleId2]), 
    CONSTRAINT [FK_AARelationInstance_NRelation] FOREIGN KEY ([NRelationId]) REFERENCES [NRelation]([Id]), 
    CONSTRAINT [FK_AARelationInstance_Article_1] FOREIGN KEY ([ArticleId1]) REFERENCES [Article]([Id]),
	CONSTRAINT [FK_AARelationInstance_Article_2] FOREIGN KEY ([ArticleId2]) REFERENCES [Article]([Id]),

)
