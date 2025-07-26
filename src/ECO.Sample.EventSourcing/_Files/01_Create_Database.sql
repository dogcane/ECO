-- Create the database
CREATE DATABASE "ECOSampleES_MT";

-- Connect to the new database (you'll need to execute this manually or use \c in psql)
-- \c "ECOSampleES_MT"

-- Create the user with necessary privileges for MartenDB (without CREATEROLE)
CREATE USER ecosample WITH PASSWORD 'ecosample' CREATEDB;

-- Grant database-level privileges
GRANT CONNECT ON DATABASE "ECOSampleES_MT" TO ecosample;
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
GRANT CREATE ON DATABASE "ECOSampleES_MT" TO ecosample;

-- If MartenDB needs to create temporary tables during operations
GRANT TEMPORARY ON DATABASE "ECOSampleES_MT" TO ecosample;