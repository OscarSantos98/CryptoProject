IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'CryptoDB')
  BEGIN
    CREATE DATABASE [CryptoDB]


    END
    GO
       USE [CryptoDB]
    GO

CREATE TABLE [user_actions] (
  [user_actions_id] smallint PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [user_actions] varchar(20),
  [role_id] smallint
)
GO

CREATE TABLE [themes] (
  [theme_id] smallint PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [theme_name] varchar(20) UNIQUE NOT NULL
)
GO

CREATE TABLE [role] (
  [role_id] smallint PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [role_desc] varchar(50) NOT NULL DEFAULT 'User'
)
GO

CREATE TABLE [user] (
  [user_id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [role_id] smallint DEFAULT (1),
  [name] varchar(20),
  [email] varchar(100) UNIQUE NOT NULL,
  [password] varchar(100) NOT NULL,
  [theme_id] smallint DEFAULT (1),
  [receiveNotifications] bit DEFAULT (0),
  [createdAt] datetime DEFAULT (getdate())
)
GO

CREATE TABLE [crypto] (
  [crypto_id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [user_id] int,
  [couple_name] varchar(100),
  [value] float,
  [last_update] datetime NOT NULL DEFAULT (getdate())
)
GO

CREATE TABLE [refreshToken] (
  [token_id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [user_id] int NOT NULL,
  [token] varchar(200) NOT NULL,
  [expiry_date] datetime NOT NULL
)
GO

EXEC sp_addextendedproperty
@name = N'Table_Description',
@value = 'Stores all actions that a particular role can perform',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'user_actions';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = '1. admin_page',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'user_actions',
@level2type = N'Column', @level2name = 'user_actions';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'dar',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'themes',
@level2type = N'Column', @level2name = 'theme_name';
GO

EXEC sp_addextendedproperty
@name = N'Table_Description',
@value = 'Stores all role titles',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'role';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Admin or User',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'role',
@level2type = N'Column', @level2name = 'role_desc';
GO

EXEC sp_addextendedproperty
@name = N'Table_Description',
@value = 'Stores user data',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'user';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = '1 - User (role with less privileges)',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'user',
@level2type = N'Column', @level2name = 'role_id';
GO

EXEC sp_addextendedproperty
@name = N'Table_Description',
@value = 'Stores crypto data from API',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'crypto';
GO

EXEC sp_addextendedproperty
@name = N'Table_Description',
@value = 'Stores refreshToken from user',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'refreshToken';
GO

ALTER TABLE [user_actions] ADD FOREIGN KEY ([role_id]) REFERENCES [role] ([role_id]) ON DELETE SET NULL
GO

ALTER TABLE [user] ADD FOREIGN KEY ([theme_id]) REFERENCES [themes] ([theme_id]) ON DELETE SET DEFAULT
GO

ALTER TABLE [user] ADD FOREIGN KEY ([role_id]) REFERENCES [role] ([role_id]) ON DELETE SET DEFAULT
GO

ALTER TABLE [crypto] ADD FOREIGN KEY ([user_id]) REFERENCES [user] ([user_id]) ON DELETE CASCADE
GO

ALTER TABLE [refreshToken] ADD FOREIGN KEY ([user_id]) REFERENCES [user] ([user_id]) ON DELETE CASCADE
GO
