CREATE TABLE [dbo].[FFRelationInstance]
(
	[FieldId1] INT NOT NULL,
	[FieldId2] INT NOT NULL,
	[NRelationId] INT NOT NULL,

	CONSTRAINT [PK_FFRelation] PRIMARY KEY ([FieldId1], [FieldId2]), 
    CONSTRAINT [FK_FFRelationInstance_Field_1] FOREIGN KEY ([FieldId1]) REFERENCES [Field]([Id]),
	CONSTRAINT [FK_FFRelationInstance_Field_2] FOREIGN KEY ([FieldId2]) REFERENCES [Field]([Id]),
	CONSTRAINT [FK_FFRelationInstance_NRelation] FOREIGN KEY ([NRelationId]) REFERENCES [NRelation]([Id]),
)
