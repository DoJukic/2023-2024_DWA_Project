-- 1-to-N
CREATE TABLE [Login]
(
	[IDLogin]		int				identity,		constraint [Login_PK] primary key ([IDLogin]),
	[Email]			nvarchar(100)	not null,
    [PasswordPlain]	nvarchar(256)	not null,
);
go

-- User
create table [User]
(
    [IDUser]		int				identity,		constraint [User_PK] primary key ([IDUser]),
	[Name]			nvarchar(100)	not null,
	[Middle_Names]	nvarchar(max),
	[Surname]		nvarchar(100)	not null,
    [LoginID]		int				not null,		constraint [User_Login_FK] foreign key ([LoginID]) references [Login]([IDLogin]),);
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

declare @LastScopeIdent int

Insert into [Login]([Email], [PasswordPlain])
values
('walter.white1@mail.com', 'WaltuhRulez654')

set @LastScopeIdent = SCOPE_IDENTITY()
insert into [User]([Name], [Surname], [LoginID])
values
('Walter', 'White', @LastScopeIdent)

set @LastScopeIdent = SCOPE_IDENTITY()
insert into [Administrator](UserID)
values
(@LastScopeIdent)


Insert into [Login]([Email], [PasswordPlain])
values
('saul.thegoodman@mail.com', 'WhiskeyEchoWhiskey')

set @LastScopeIdent = SCOPE_IDENTITY()
insert into [User]([Name], [Surname], [LoginID])
values
('Saul', 'Goodman', @LastScopeIdent)

set @LastScopeIdent = SCOPE_IDENTITY()
insert into [Administrator](UserID)
values
(@LastScopeIdent)


--Users:

Insert into [Login]([Email], [PasswordPlain])
values
('jpinkman@mail.com', 'LimaAlphaDelta')

set @LastScopeIdent = SCOPE_IDENTITY()
insert into [User]([Name], [Surname], [LoginID])
values
('Jesse', 'Pinkman', @LastScopeIdent)


Insert into [Login]([Email], [PasswordPlain])
values
('schrader.ha@mail.com', 'schadenfreude')

set @LastScopeIdent = SCOPE_IDENTITY()
insert into [User]([Name], [Surname], [LoginID])
values
('Hank', 'Schrader', @LastScopeIdent)


Insert into [Login]([Email], [PasswordPlain])
values
('white.ff@mail.com', 'The quick brown fox jumps over the lazy dog')

set @LastScopeIdent = SCOPE_IDENTITY()
insert into [User]([Name], [Surname], [LoginID])
values
('Skyler', 'White', @LastScopeIdent)

GO

-- Drop Tables (generated, use removed)

ALTER TABLE [dbo].[UserBorrowingReservation] DROP CONSTRAINT [UserBorrowingReservation_User_FK]
GO
ALTER TABLE [dbo].[UserBorrowingReservation] DROP CONSTRAINT [UserBorrowingReservation_BookLocationLink_FK]
GO
ALTER TABLE [dbo].[User] DROP CONSTRAINT [User_Login_FK]
GO
ALTER TABLE [dbo].[BookLocationLink] DROP CONSTRAINT [BookLocationLink_Location_FK]
GO
ALTER TABLE [dbo].[BookLocationLink] DROP CONSTRAINT [BookLocationLink_Book_FK]
GO
ALTER TABLE [dbo].[Book] DROP CONSTRAINT [Book_Genre_FK]
GO
ALTER TABLE [dbo].[Administrator] DROP CONSTRAINT [Administrator_User_FK]
GO
/****** Object:  Table [dbo].[UserBorrowingReservation]    Script Date: 15/11/2024 17:33:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserBorrowingReservation]') AND type in (N'U'))
DROP TABLE [dbo].[UserBorrowingReservation]
GO
/****** Object:  Table [dbo].[User]    Script Date: 15/11/2024 17:33:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
DROP TABLE [dbo].[User]
GO
/****** Object:  Table [dbo].[Login]    Script Date: 15/11/2024 17:33:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Login]') AND type in (N'U'))
DROP TABLE [dbo].[Login]
GO
/****** Object:  Table [dbo].[Location]    Script Date: 15/11/2024 17:33:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Location]') AND type in (N'U'))
DROP TABLE [dbo].[Location]
GO
/****** Object:  Table [dbo].[Genre]    Script Date: 15/11/2024 17:33:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Genre]') AND type in (N'U'))
DROP TABLE [dbo].[Genre]
GO
/****** Object:  Table [dbo].[BookLocationLink]    Script Date: 15/11/2024 17:33:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BookLocationLink]') AND type in (N'U'))
DROP TABLE [dbo].[BookLocationLink]
GO
/****** Object:  Table [dbo].[Book]    Script Date: 15/11/2024 17:33:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Book]') AND type in (N'U'))
DROP TABLE [dbo].[Book]
GO
/****** Object:  Table [dbo].[Administrator]    Script Date: 15/11/2024 17:33:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Administrator]') AND type in (N'U'))
DROP TABLE [dbo].[Administrator]
GO
