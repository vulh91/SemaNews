CREATE TABLE [dbo].[GGRelationInstance]
(
	[GFieldId1] INT NOT NULL,
	[GFieldId2] INT NOT NULL,
	[GGRelationId] INT NOT NULL,
	CONSTRAINT [PK_GGRelation] PRIMARY KEY ([GFieldId1],[GFieldId2],[GGRelationId]), 
    CONSTRAINT [FK_GGRelationInstance_GField_1] FOREIGN KEY ([GFieldId1]) REFERENCES [GField]([Id]),
	CONSTRAINT [FK_GGRelationInstance_GField_2] FOREIGN KEY ([GFieldId2]) REFERENCES [GField]([Id]), 
    CONSTRAINT [FK_GGRelationInstance_GGRelation] FOREIGN KEY ([GGRelationId]) REFERENCES [GGRelation]([Id]),

)
