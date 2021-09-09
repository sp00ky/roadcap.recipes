CREATE TABLE [dbo].[ingredient]
(
	[ingredient_id] INT NOT NULL IDENTITY PRIMARY KEY,
	[recipe_id] INT NOT NULL,
	[ingredient_name] NVARCHAR(75) NOT NULL,
	[amount] DECIMAL(6, 2) NOT NULL DEFAULT(1),
	[units] NVARCHAR(15) NULL,
	[special_instructions] NVARCHAR(255) NULL, 
    CONSTRAINT [FK_ingredient__recipe] FOREIGN KEY ([recipe_id]) REFERENCES [recipe]([recipe_id]) ON DELETE CASCADE
)
