IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Countries] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Countries] PRIMARY KEY ([Id])
);

CREATE TABLE [Students] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Gender] nvarchar(max) NOT NULL,
    [CountryId] int NOT NULL,
    [StateId] int NOT NULL,
    [CityId] int NOT NULL,
    CONSTRAINT [PK_Students] PRIMARY KEY ([Id])
);

CREATE TABLE [States] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [CountryId] int NOT NULL,
    CONSTRAINT [PK_States] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_States_Countries_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Countries] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Cities] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [StateId] int NOT NULL,
    CONSTRAINT [PK_Cities] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Cities_States_StateId] FOREIGN KEY ([StateId]) REFERENCES [States] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Cities_StateId] ON [Cities] ([StateId]);

CREATE INDEX [IX_States_CountryId] ON [States] ([CountryId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260825111817_InitialCreate', N'10.0.11');

COMMIT;
GO

