-- User
create table [User]
(
    [IDUser]		int				identity,		constraint [User_PK] primary key ([IDUser]),
	[Name]			nvarchar(100)	not null,
	[Middle_Names]	nvarchar(max),
	[Surname]		nvarchar(100)	not null,
);
go

-- N-to-1
CREATE TABLE [Login]
(
	[IDLogin]		int				identity,		constraint [Login_PK] primary key ([IDLogin]),
	[Email]			nvarchar(100)	not null,
    [PasswordHash]	nvarchar(256)	not null,
    [PasswordSalt]	nvarchar(256)	not null,
    [UserID]		int				not null,		constraint [Login_User_FK] foreign key ([UserID]) references [User]([IDUser])
);
go

-- 1-to-N
create table [Administrator]
(
    [UserID]		int				not null,		constraint [Administrator_PK] primary key ([UserID]),
													constraint [Administrator_User_FK] foreign key ([UserID]) references [User]([IDUser]),
);
go

-- 1-to-N
create table [Genre]
(
    [IDGenre]		int				identity,		constraint [Genre_PK] primary key ([IDGenre]),
	[Name]			nvarchar(100)	not null,
	[Description]	nvarchar(max),
);
go

-- M-to-N
create table [Location]
(
    [IDLocation]	int				identity,		constraint [Location_PK] primary key ([IDLocation]),
	[Name]			nvarchar(100)	not null,
	[Description]	nvarchar(max),
);
go

-- Primary
create table [Book]
(
    [IDBook]		int				identity,		constraint [Book_PK] primary key ([IDBook]),
	[Name]			nvarchar(100)	not null,
	[Description]	nvarchar(max),
	[GenreID]		int,							constraint [Book_Genre_FK] foreign key ([GenreID]) references [Genre]([IDGenre]),
);
go

-- M-to-N-bridge
create table [BookLocationLink]
(
	[IDBLLink]		int				identity,		constraint [BookLocationLink_PK] primary key ([IDBLLink]),
    [LocationID]	int				not null,		constraint [BookLocationLink_Location_FK] foreign key ([LocationID]) references [Location]([IDLocation]),
	[BookID]		int				not null,		constraint [BookLocationLink_Book_FK] foreign key ([BookID]) references [Book]([IDBook]),
	[Total]			int				not null,		constraint [BookLocationLink_Total_Check_Above_Zero] check ([Total] >= 0),
);
go

-- User-M-to-N-bridge
create table [UserBorrowingReservation]
(
	[IDReservation]		int					identity,		constraint [UserBorrowingReservation_PK] primary key ([IDReservation]),
    [BLLinkID]			int					not null,		constraint [UserBorrowingReservation_BookLocationLink_FK] foreign key ([BLLinkID]) references [BookLocationLink]([IDBLLink]),
    [UserID]			int					not null,		constraint [UserBorrowingReservation_User_FK] foreign key ([UserID]) references [User]([IDUser]),
	[DateReserved]		datetimeoffset		not null,
	[DateExpiration]	datetimeoffset		not null,
	[DateBorrowed]		datetimeoffset,
	[DateReturned]		datetimeoffset,
);

GO

-- Data insertion
-- Admins:

declare @LastUserScopeIdent int
insert into [User]([Name], [Surname])
values
('Walter', 'White')

set @LastUserScopeIdent = SCOPE_IDENTITY()

Insert into [Login]([Email], [PasswordHash], [PasswordSalt], [UserID])
values
(
	'walter.white1@mail.com',
	'UpEaTlAoTDVDS4l3/CWXMOl6rI67CLyoqJBgz4J4ltI=',
	'+C5TLnxyy6C82SJlC6PIyw==',
	@LastUserScopeIdent
)

insert into [Administrator](UserID)
values
(@LastUserScopeIdent)


insert into [User]([Name], [Surname])
values
('Saul', 'Goodman')

set @LastUserScopeIdent = SCOPE_IDENTITY()

Insert into [Login]([Email], [PasswordHash], [PasswordSalt], [UserID])
values
(
	'saul.thegoodman@mail.com',
	'eyy2XIzR0q+QXYt4iiYD0jL7l2qRp4LPu0Z8UHhbMZ0=',
	'on6DmlO0Yz/tQmWRxdIlpA==',
	@LastUserScopeIdent
)

insert into [Administrator](UserID)
values
(@LastUserScopeIdent)


--Users:
insert into [User]([Name], [Surname])
values
('Jesse', 'Pinkman')

set @LastUserScopeIdent = SCOPE_IDENTITY()

Insert into [Login]([Email], [PasswordHash], [PasswordSalt], [UserID])
values
(
	'jpinkman@mail.com', 
	'LLLcJSxCPBjPI3smBVoTr4JPDU0naSI2Qbgdfiqaefw=',
	'uL2x7MvQvc35F0FYz67n/Q==',
	@LastUserScopeIdent
)


insert into [User]([Name], [Surname])
values
('Hank', 'Schrader')

set @LastUserScopeIdent = SCOPE_IDENTITY()

Insert into [Login]([Email], [PasswordHash], [PasswordSalt], [UserID])
values
(
	'schrader.ha@mail.com',
	'YMUbgsMf0QoxU8XaWreA8ApqDxjBNQmXWfdbFj2pOOQ=',
	'2jROlTdNlAEd0IHZXsrhvw==',
	@LastUserScopeIdent
)


insert into [User]([Name], [Surname])
values
('Skyler', 'White')

set @LastUserScopeIdent = SCOPE_IDENTITY()

Insert into [Login]([Email], [PasswordHash], [PasswordSalt], [UserID])
values
(
	'white.ff@mail.com', 
	'LJ/Oz3WdTB48FVXripZdAKSsS0t1rJJy4zBRN7g5KaQ=',
	'+IgfRhbouffh69Sv+T/Tjw==',
	@LastUserScopeIdent
)

-- Genres:
insert into [Genre]([Name], [Description])
values
('N/A', 'Non-Applicable')

insert into [Genre]([Name], [Description])
values
('Romance', 'Rated top 1 in bonafidebookworm.com''s most popular book genres')

insert into [Genre]([Name], [Description])
values
('Fantasy', 'Gaze upon another world and despair, for you will never have that infinite gas stove')

insert into [Genre]([Name], [Description])
values
('True Crime', 'YEEEEEEEEEEEEEEAAAAAAAAAAAAAAAAAAHHHHHHHHHHHHHHHHHHHHHHHHHH')

insert into [Genre]([Name], [Description])
values
('Mistery', 'Why Do They Call It Oven When You Of In The Cold Food Of Out Hot Eat The Food')

insert into [Genre]([Name], [Description])
values
('Dystopian', 'Orwell that ends well')

insert into [Genre]([Name], [Description])
values
('Science Fiction', 'Gaze upon another world and despair, for you will never have that infinite gas stove')

insert into [Genre]([Name], [Description])
values
('Adventure', 'Swiper no swiping! Swiper no swiping! Swiper no swiping!')

-- Books:
insert into [Book]([Name],[GenreID],[Description])
values
('Lucky Break', (select top 1 g.IDGenre from [Genre] as g where g.Name = 'Adventure'), '"The seedy underbelly of the Library, unmasked."')

insert into [Book]([Name],[GenreID],[Description])
values
('81st Turn, Seventh Year, Eighteenth Cycle, Tresday', (select top 1 g.IDGenre from [Genre] as g where g.Name = 'N/A'), 'An excerpt from the Journal of Aframos Longjourney, Pilgrim With notes by Avos Torr, Scholar of Rheve Library Tresday, Eighteenth Cycle, Seventh Year, 81st Turn Forty-First Day in the Trees')

insert into [Book]([Name],[GenreID],[Description])
values
('Anagnorisis', (select top 1 g.IDGenre from [Genre] as g where g.Name = 'Mistery'), 'A man is rescued after 17 years of being a POW and faces impossible consequences')

insert into [Book]([Name],[GenreID],[Description])
values
('The Contrary Traveler''s Story', (select top 1 g.IDGenre from [Genre] as g where g.Name = 'Adventure'), 'An unsettling story on the roots of a traveller')

-- Locations:
insert into [Location]([Name], [Description])
values
('Old Town', 'Find the oldest brick and whisper the name of that which we hate. A fine entry point, but you might look a bit weird to onlookers.')

insert into [Location]([Name], [Description])
values
('New Zagreb', 'Go to the modern ampitheater at midnight and spread fresh blood from a featherless biped of any type. Worry not, for you will be retroactively removed from all memory upon completion.')

insert into [Location]([Name], [Description])
values
('Zagreb East', 'Seek and you shall find. DO NOT accept any cookies.')

insert into [Location]([Name], [Description])
values
('Zagreb West', 'Abandoned library, basement.')

-- BookLocationLinks:
--		Old Town:
insert into [BookLocationLink]([BookID], [LocationID], [Total])
values
((select top 1 b.IDBook from [Book] as b where b.Name = 'Lucky Break'), (select top 1 l.IDLocation from [Location] as l where l.Name = 'Old Town'), 3)

insert into [BookLocationLink]([BookID], [LocationID], [Total])
values
((select top 1 b.IDBook from [Book] as b where b.Name = '81st Turn, Seventh Year, Eighteenth Cycle, Tresday'), (select top 1 l.IDLocation from [Location] as l where l.Name = 'Old Town'), 1)

insert into [BookLocationLink]([BookID], [LocationID], [Total])
values
((select top 1 b.IDBook from [Book] as b where b.Name = 'The Contrary Traveler''s Story'), (select top 1 l.IDLocation from [Location] as l where l.Name = 'Old Town'), 4)

--		New Zagreb:
insert into [BookLocationLink]([BookID], [LocationID], [Total])
values
((select top 1 b.IDBook from [Book] as b where b.Name = 'Anagnorisis'), (select top 1 l.IDLocation from [Location] as l where l.Name = 'New Zagreb'), 2)

insert into [BookLocationLink]([BookID], [LocationID], [Total])
values
((select top 1 b.IDBook from [Book] as b where b.Name = 'Lucky Break'), (select top 1 l.IDLocation from [Location] as l where l.Name = 'New Zagreb'), 1)

--		Zagreb East:
insert into [BookLocationLink]([BookID], [LocationID], [Total])
values
((select top 1 b.IDBook from [Book] as b where b.Name = 'Lucky Break'), (select top 1 l.IDLocation from [Location] as l where l.Name = 'Zagreb East'), 5)

insert into [BookLocationLink]([BookID], [LocationID], [Total])
values
((select top 1 b.IDBook from [Book] as b where b.Name = '81st Turn, Seventh Year, Eighteenth Cycle, Tresday'), (select top 1 l.IDLocation from [Location] as l where l.Name = 'Zagreb East'), 6)

insert into [BookLocationLink]([BookID], [LocationID], [Total])
values
((select top 1 b.IDBook from [Book] as b where b.Name = 'Anagnorisis'), (select top 1 l.IDLocation from [Location] as l where l.Name = 'Zagreb East'), 1)

insert into [BookLocationLink]([BookID], [LocationID], [Total])
values
((select top 1 b.IDBook from [Book] as b where b.Name = 'The Contrary Traveler''s Story'), (select top 1 l.IDLocation from [Location] as l where l.Name = 'Zagreb East'), 1)

--		Zagreb West:
insert into [BookLocationLink]([BookID], [LocationID], [Total])
values
((select top 1 b.IDBook from [Book] as b where b.Name = '81st Turn, Seventh Year, Eighteenth Cycle, Tresday'), (select top 1 l.IDLocation from [Location] as l where l.Name = 'Zagreb West'), 2)

insert into [BookLocationLink]([BookID], [LocationID], [Total])
values
((select top 1 b.IDBook from [Book] as b where b.Name = 'Anagnorisis'), (select top 1 l.IDLocation from [Location] as l where l.Name = 'Zagreb West'), 3)

insert into [BookLocationLink]([BookID], [LocationID], [Total])
values
((select top 1 b.IDBook from [Book] as b where b.Name = 'The Contrary Traveler''s Story'), (select top 1 l.IDLocation from [Location] as l where l.Name = 'Zagreb West'), 3)

GO

-- Drop Tables (generated, use removed)

ALTER TABLE [dbo].[UserBorrowingReservation] DROP CONSTRAINT [UserBorrowingReservation_User_FK]
GO
ALTER TABLE [dbo].[UserBorrowingReservation] DROP CONSTRAINT [UserBorrowingReservation_BookLocationLink_FK]
GO
ALTER TABLE [dbo].[Login] DROP CONSTRAINT [Login_User_FK]
GO
ALTER TABLE [dbo].[BookLocationLink] DROP CONSTRAINT [BookLocationLink_Location_FK]
GO
ALTER TABLE [dbo].[BookLocationLink] DROP CONSTRAINT [BookLocationLink_Book_FK]
GO
ALTER TABLE [dbo].[Book] DROP CONSTRAINT [Book_Genre_FK]
GO
ALTER TABLE [dbo].[Administrator] DROP CONSTRAINT [Administrator_User_FK]
GO
/****** Object:  Table [dbo].[UserBorrowingReservation]    Script Date: 23/11/2024 22:07:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserBorrowingReservation]') AND type in (N'U'))
DROP TABLE [dbo].[UserBorrowingReservation]
GO
/****** Object:  Table [dbo].[User]    Script Date: 23/11/2024 22:07:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
DROP TABLE [dbo].[User]
GO
/****** Object:  Table [dbo].[Login]    Script Date: 23/11/2024 22:07:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Login]') AND type in (N'U'))
DROP TABLE [dbo].[Login]
GO
/****** Object:  Table [dbo].[Location]    Script Date: 23/11/2024 22:07:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Location]') AND type in (N'U'))
DROP TABLE [dbo].[Location]
GO
/****** Object:  Table [dbo].[Genre]    Script Date: 23/11/2024 22:07:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Genre]') AND type in (N'U'))
DROP TABLE [dbo].[Genre]
GO
/****** Object:  Table [dbo].[BookLocationLink]    Script Date: 23/11/2024 22:07:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BookLocationLink]') AND type in (N'U'))
DROP TABLE [dbo].[BookLocationLink]
GO
/****** Object:  Table [dbo].[Book]    Script Date: 23/11/2024 22:07:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Book]') AND type in (N'U'))
DROP TABLE [dbo].[Book]
GO
/****** Object:  Table [dbo].[Administrator]    Script Date: 23/11/2024 22:07:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Administrator]') AND type in (N'U'))
DROP TABLE [dbo].[Administrator]
GO
