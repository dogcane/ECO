--DB CREATION
CREATE DATABASE "ECOSampleApp_MT";

--USERS AND PERMISSIONS
CREATE USER ecosample WITH PASSWORD 'ecosample';

-- Grant all privileges on the database to ecosample user
GRANT ALL PRIVILEGES ON DATABASE "ECOSampleApp_MT" TO ecosample;

-- Grant all privileges on the public schema
GRANT ALL ON SCHEMA public TO ecosample;

-- Grant all privileges on all existing tables in public schema
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO ecosample;

-- Grant all privileges on all existing sequences in public schema
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO ecosample;

-- Grant all privileges on all existing functions in public schema
GRANT ALL PRIVILEGES ON ALL FUNCTIONS IN SCHEMA public TO ecosample;

-- Grant default privileges for future objects created in public schema
ALTER DEFAULT PRIVILEGES IN SCHEMA public 
    GRANT ALL PRIVILEGES ON TABLES TO ecosample;

ALTER DEFAULT PRIVILEGES IN SCHEMA public 
    GRANT ALL PRIVILEGES ON SEQUENCES TO ecosample;

ALTER DEFAULT PRIVILEGES IN SCHEMA public 
    GRANT ALL PRIVILEGES ON FUNCTIONS TO ecosample;

-- Grant usage and create privileges on public schema (allows creating new objects)
GRANT USAGE, CREATE ON SCHEMA public TO ecosample;
