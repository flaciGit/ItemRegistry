# ItemRegistry
This is a tiny item register example, result of many followed tutorials and tinkering with stuffs as i am getting familiar with ASP.NET


Table of content

ASP.NET Core, Entity Framework, Razor Pages, MS SQL

User registration, login and cookies.

Create, list and edit items, with name and value attributes if the user logged in is an admin.

List items into multiple pages with sorting and search feature.

Only list items if the user is not admin or logged out.


Database:
```
CREATE TABLE Items
(
	Id INT NOT NULL IDENTITY(1,1),
	Name VARCHAR(90),
	Value INT NOT NULL DEFAULT 0,

	PRIMARY KEY (Id)
)

CREATE TABLE Users
(
	Id INT NOT NULL IDENTITY(1,1),
	Name VARCHAR(90) NOT NULL,
	Password VARCHAR(90) NOT NULL,
	IsAdmin BIT NOT NULL DEFAULT 0,

	PRIMARY KEY(Id)
)
```
