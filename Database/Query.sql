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

-- M-to-N-bridge
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

GO

-- Drop Tables (generated, use removed)

GO
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
