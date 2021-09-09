# roadcap.recipes

## Projects
### roadcap.recipes
MVC project containing the UI
### roadcap.recipes.api
Web API containing CRUD methods for Recipes and Ingredients.  At present, only deleting Recipies and Ingredients will invoke an API.
### roadcap.recipes.business
Business rules.  Only a class containing a single empty rule exists.
### roadcap.recipes.entities
EF Core entities and dbContext
### roadcap.recipes.sql
SQL Server project with implementation of Recipe and Ingredient tables.  Typically I don't use EF migrations in production.  In order to create the database, publish this project.
\
\
\
Note - Currently the image handling will only work with jpegs.  
