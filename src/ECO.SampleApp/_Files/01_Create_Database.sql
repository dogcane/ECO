﻿CREATE TABLE [Speakers] (
	[Id] UniqueIdentifier PRIMARY KEY,
	[Name] NVarChar(50) NOT NULL,
	[Surname] NVarChar(50) NOT NULL,
	[Description] NVarChar(1000) NOT NULL,
	[Age] INT NOT NULL
)

CREATE TABLE [Events] (
	[Id] UniqueIdentifier CONSTRAINT Primary_Key_Events PRIMARY KEY NONCLUSTERED,
	[Name] NVarChar(50) NOT NULL,
	[Description] NVarChar(1000) NOT NULL,
	[StartDate] DateTime NOT NULL,
	[EndDate] DateTime NOT NULL
)

CREATE TABLE [Sessions] (
	[Id] UniqueIdentifier CONSTRAINT Primary_Key_Sessions PRIMARY KEY NONCLUSTERED,
	[Title] NVarChar(50) NOT NULL,
	[Description] NVarChar(1000) NOT NULL,
	[Level] INT NOT NULL,
	[FK_Event] UniqueIdentifier,
	[FK_Speaker] UniqueIdentifier,
	CONSTRAINT Foreign_Key_Event FOREIGN KEY(FK_Event) REFERENCES [Events](Id),
	CONSTRAINT Foreign_Key_Speaker FOREIGN KEY(FK_Speaker) REFERENCES [Speakers](Id)
)