﻿--DB CREATION
CREATE DATABASE "ECOSampleApp";

--TABLE CREATION
CREATE TABLE "Speakers" (
	"Id" UUID CONSTRAINT Primary_Key_Speakers PRIMARY KEY,
	"Name" VARCHAR(50) NOT NULL,
	"Surname" VARCHAR(50) NOT NULL,
	"Description" VARCHAR(1000) NOT NULL,
	"Age" INT NOT NULL
);

CREATE TABLE "Events" (
	"Id" UUID CONSTRAINT Primary_Key_Events PRIMARY KEY,
	"Name" VARCHAR(50) NOT NULL,
	"Description" VARCHAR(1000) NOT NULL,
	"StartDate" TIMESTAMP(3) NOT NULL,
	"EndDate" TIMESTAMP(3) NOT NULL
);

CREATE TABLE "Sessions" (
	"Id" UUID CONSTRAINT Primary_Key_Sessions PRIMARY KEY,
	"Title" VARCHAR(50) NOT NULL,
	"Description" VARCHAR(1000) NOT NULL,
	"Level" INT NOT NULL,
	"FK_Event" UUID,
	"FK_Speaker" UUID,
	CONSTRAINT "Foreign_Key_Event" FOREIGN KEY("FK_Event") REFERENCES "Events"("Id"),
	CONSTRAINT "Foreign_Key_Speaker" FOREIGN KEY("FK_Speaker") REFERENCES "Speakers"("Id")
);

--CLEAN DATA
TRUNCATE TABLE "Sessions" CASCADE;
TRUNCATE TABLE "Events" CASCADE;
TRUNCATE TABLE "Speakers" CASCADE;

-- Create the user with password
CREATE USER ecosample WITH PASSWORD 'ecosample';

-- Grant database-level privileges
GRANT CONNECT ON DATABASE "ECOSampleApp" TO ecosample;
GRANT USAGE, CREATE ON SCHEMA public TO ecosample;

-- Grant all privileges on existing objects
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO ecosample;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO ecosample;
GRANT ALL PRIVILEGES ON ALL FUNCTIONS IN SCHEMA public TO ecosample;

-- Grant privileges on future objects (important for MartenDB)
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON TABLES TO ecosample;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON SEQUENCES TO ecosample;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON FUNCTIONS TO ecosample;

-- Grant necessary schema privileges for MartenDB operations
GRANT USAGE ON SCHEMA information_schema TO ecosample;
GRANT USAGE ON SCHEMA pg_catalog TO ecosample;
GRANT SELECT ON ALL TABLES IN SCHEMA information_schema TO ecosample;

-- Pre-create common extensions that MartenDB needs (as superuser)
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE EXTENSION IF NOT EXISTS "pgcrypto";
CREATE EXTENSION IF NOT EXISTS "ltree";
CREATE EXTENSION IF NOT EXISTS "hstore";

-- Grant usage on the extensions
GRANT USAGE ON SCHEMA public TO ecosample;

-- Grant specific privileges needed for MartenDB schema management
-- Allow creating and dropping objects in the database
GRANT CREATE ON DATABASE "ECOSampleApp" TO ecosample;

-- If MartenDB needs to create temporary tables during operations
GRANT TEMPORARY ON DATABASE "ECOSampleApp" TO ecosample;