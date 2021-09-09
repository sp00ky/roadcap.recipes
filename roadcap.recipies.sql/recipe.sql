CREATE TABLE [dbo].[recipe]
(
	[recipe_id] INT NOT NULL IDENTITY PRIMARY KEY,
	[title] NVARCHAR(50) NOT NULL,
	[description] NVARCHAR(1000) NULL,
	[image] VARBINARY(MAX) NULL,
	[instructions] NVARCHAR(MAX) NOT NULL
)

GO
