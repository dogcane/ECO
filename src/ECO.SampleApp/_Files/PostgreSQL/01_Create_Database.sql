﻿CREATE TABLE Speakers (
	Id UUID CONSTRAINT Primary_Key_Speakers PRIMARY KEY,
	Name VARCHAR(50) NOT NULL,
	Surname VARCHAR(50) NOT NULL,
	Description VARCHAR(1000) NOT NULL,
	Age INT NOT NULL
);

CREATE TABLE Events (
	Id UUID CONSTRAINT Primary_Key_Events PRIMARY KEY,
	Name VARCHAR(50) NOT NULL,
	Description VARCHAR(1000) NOT NULL,
	StartDate TIMESTAMP(3) NOT NULL,
	EndDate TIMESTAMP(3) NOT NULL
);

CREATE TABLE Sessions (
	Id UUID CONSTRAINT Primary_Key_Sessions PRIMARY KEY,
	Title VARCHAR(50) NOT NULL,
	Description VARCHAR(1000) NOT NULL,
	Level INT NOT NULL,
	FK_Event UUID,
	FK_Speaker UUID,
	CONSTRAINT Foreign_Key_Event FOREIGN KEY(FK_Event) REFERENCES Events(Id),
	CONSTRAINT Foreign_Key_Speaker FOREIGN KEY(FK_Speaker) REFERENCES Speakers(Id)
);

--CLEAN DATA
DELETE FROM Sessions
DELETE FROM Events
DELETE FROM Speakers