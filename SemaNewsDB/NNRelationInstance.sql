CREATE TABLE [dbo].[NNRelationInstance]
(
	[NewspaperId1] INT,						--> Newspaper1 là chuyên trang của 1 Newspaper2 nào
	[NewspaperId2] INT,						--> Newspaper2
	[NRelationId] INT NOT NULL,
	CONSTRAINT [PK_NNRelationInstance] PRIMARY KEY ([NewspaperId1], [NewspaperId2]), 
    CONSTRAINT [FK_NNRelationInstance_Newspaper_1] FOREIGN KEY ([NewspaperId1]) REFERENCES [Newspaper]([Id]),
	CONSTRAINT [FK_NNRelationInstance_Newspaper_2] FOREIGN KEY ([NewspaperId2]) REFERENCES [Newspaper]([Id]), 
    CONSTRAINT [FK_NNRelationInstance_NRelation] FOREIGN KEY ([NRelationId]) REFERENCES [NRelation]([Id]),


)
