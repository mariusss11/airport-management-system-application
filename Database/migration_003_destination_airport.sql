-- Migration: Replace destination text column with destination_airport_id FK

-- Add destination_airport_id FK to flights
ALTER TABLE flights
ADD COLUMN IF NOT EXISTS destination_airport_id INTEGER REFERENCES airports(id);

-- Migrate existing text destinations to FK where airport name or code matches
UPDATE flights f
SET destination_airport_id = a.id
FROM airports a
WHERE LOWER(f.destination) = LOWER(a.name)
   OR LOWER(f.destination) = LOWER(a.code);

-- Drop the old text column
ALTER TABLE flights DROP COLUMN IF EXISTS destination;

-- Add destination_airport_id FK to archived_flights as well
ALTER TABLE archived_flights
ADD COLUMN IF NOT EXISTS destination_airport_id INTEGER REFERENCES airports(id);
