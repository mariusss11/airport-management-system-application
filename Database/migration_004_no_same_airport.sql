-- Migration: Prevent a flight from having the same departure and destination airport

ALTER TABLE flights
ADD CONSTRAINT chk_flights_different_airports
    CHECK (departure_airport_id IS NULL
        OR destination_airport_id IS NULL
        OR departure_airport_id <> destination_airport_id);
