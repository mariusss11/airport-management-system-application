-- Migration: Add airports table and departure airport support

-- Create airports table
CREATE TABLE IF NOT EXISTS airports (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL,
    code TEXT NOT NULL
);

-- Add departure_airport_id to flights table
ALTER TABLE flights
ADD COLUMN IF NOT EXISTS departure_airport_id INTEGER REFERENCES airports(id);

-- Add departure_airport_id to archived_flights table
ALTER TABLE archived_flights
ADD COLUMN IF NOT EXISTS departure_airport_id INTEGER REFERENCES airports(id);

-- Seed airports table with some Romanian and European airports
INSERT INTO airports (name, code) VALUES
('Henri Coandă International Airport', 'OTP'),
('Bucharest Baneasa Airport', 'BBU'),
('Cluj Airport', 'CLJ'),
('Constanța Airport', 'CND'),
('Timișoara Airport', 'TSR'),
('Iași Airport', 'IAS'),
('Berlin Brandenburg Airport', 'BER'),
('Munich Airport', 'MUC'),
('Vienna International Airport', 'VIE'),
('Budapest Ferenc Liszt Airport', 'BUD'),
('Prague Václav Havel Airport', 'PRG');
