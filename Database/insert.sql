-- Seed data for the Airport Flight Management System
-- Safe to re-run: each INSERT is guarded by NOT EXISTS on a natural key.

-- ============================================================
-- Planes
-- ============================================================
INSERT INTO planes (registration_number, model, at_airport)
SELECT v.registration_number, v.model, v.at_airport
FROM (VALUES
    ('AIR-010', 'Airbus A320',          true),
    ('AIR-011', 'Embraer 190',          true),
    ('AIR-012', 'Boeing 800',           false),
    ('AIR-013', 'Boeing 737-800',       true),
    ('AIR-014', 'Boeing 787-9 Dreamliner', true),
    ('AIR-015', 'Airbus A321neo',       true),
    ('AIR-016', 'Airbus A350-900',      false),
    ('AIR-017', 'ATR 72-600',           true),
    ('AIR-018', 'Bombardier CRJ-900',   true),
    ('AIR-019', 'Embraer E195-E2',      true),
    ('AIR-020', 'Boeing 747-8',         false),
    ('AIR-021', 'Airbus A220-300',      true),
    ('AIR-022', 'Boeing 757-200',       true),
    ('AIR-023', 'Airbus A330-300',      false),
    ('AIR-024', 'McDonnell Douglas MD-90', true)
) AS v(registration_number, model, at_airport)
WHERE NOT EXISTS (
    SELECT 1 FROM planes p WHERE p.registration_number = v.registration_number
);

-- ============================================================
-- Flights
--   plane_id  : looked up by registration_number
--   airport_id: looked up by code
-- ============================================================
INSERT INTO flights (
    flight_code, destination, departure_time, arrival_time,
    plane_id, total_seats, available_seats, ticket_price,
    status, departure_date, departure_airport_id
)
SELECT
    v.flight_code, v.destination, v.departure_time, v.arrival_time,
    (SELECT id FROM planes   WHERE registration_number = v.plane_reg LIMIT 1),
    v.total_seats, v.available_seats, v.ticket_price,
    v.status, v.departure_date,
    (SELECT id FROM airports WHERE code = v.airport_code LIMIT 1)
FROM (VALUES
    -- (flight_code, destination,    dep_time,        arr_time,        plane_reg, total, avail, price,   status,        dep_date,      airport)
    ('FR-106', 'London',             TIME '06:30',   TIME '08:55',   'AIR-013', 189, 142, 189.50, 'On Time',      DATE '2026-05-08', 'OTP'),
    ('FR-107', 'Amsterdam',          TIME '07:45',   TIME '10:10',   'AIR-015', 220, 198, 215.00, 'On Time',      DATE '2026-05-08', 'OTP'),
    ('FR-108', 'Frankfurt',          TIME '09:00',   TIME '11:05',   'AIR-014', 296, 250, 240.75, 'On Time',     DATE '2026-05-08', 'CLJ'),
    ('FR-109', 'Istanbul',           TIME '10:15',   TIME '12:00',   'AIR-018',  90,  76, 175.25, 'On Time',     DATE '2026-05-08', 'TSR'),
    ('FR-110', 'Dubai',              TIME '23:30',   TIME '06:45',   'AIR-016', 314, 280, 690.00, 'On Time',     DATE '2026-05-09', 'OTP'),
    ('FR-111', 'Vienna',             TIME '12:00',   TIME '13:25',   'AIR-017',  72,  60, 125.00, 'On Time',     DATE '2026-05-09', 'BBU'),
    ('FR-112', 'Munich',             TIME '14:20',   TIME '16:00',   'AIR-019', 132, 121, 198.40, 'Delayed',     DATE '2026-05-09', 'IAS'),
    ('FR-113', 'Berlin',             TIME '16:45',   TIME '18:50',   'AIR-013', 189, 165, 210.00, 'On Time',     DATE '2026-05-09', 'OTP'),
    ('FR-114', 'Prague',             TIME '08:10',   TIME '09:35',   'AIR-021', 149, 138, 165.50, 'On Time',     DATE '2026-05-10', 'CLJ'),
    ('FR-115', 'Budapest',           TIME '11:30',   TIME '12:30',   'AIR-018',  90,  82, 110.00, 'On Time',     DATE '2026-05-10', 'OTP'),
    ('FR-116', 'New York JFK',       TIME '15:00',   TIME '20:30',   'AIR-014', 296, 240, 875.00, 'On Time',     DATE '2026-05-10', 'OTP'),
    ('FR-117', 'Barcelona',          TIME '13:15',   TIME '16:10',   'AIR-022', 200, 187, 230.00, 'On Time',     DATE '2026-05-10', 'OTP'),
    ('FR-118', 'Madrid',             TIME '17:50',   TIME '21:05',   'AIR-015', 220, 205, 245.75, 'Delayed',     DATE '2026-05-10', 'CLJ'),
    ('FR-119', 'Rome Fiumicino',     TIME '09:25',   TIME '11:00',   'AIR-019', 132, 130, 199.99, 'On Time',     DATE '2026-05-11', 'TSR'),
    ('FR-120', 'Athens',             TIME '06:00',   TIME '08:20',   'AIR-013', 189, 170, 185.00, 'On Time',     DATE '2026-05-11', 'OTP'),
    ('FR-121', 'Zurich',             TIME '14:40',   TIME '17:00',   'AIR-021', 149, 142, 275.00, 'Canceled',    DATE '2026-05-11', 'OTP'),
    ('FR-122', 'Lisbon',             TIME '10:30',   TIME '14:15',   'AIR-022', 200, 188, 290.50, 'On Time',     DATE '2026-05-12', 'OTP'),
    ('FR-123', 'Copenhagen',         TIME '07:00',   TIME '09:25',   'AIR-018',  90,  88, 220.00, 'On Time',     DATE '2026-05-12', 'CND'),
    ('FR-124', 'Stockholm Arlanda',  TIME '12:45',   TIME '15:30',   'AIR-015', 220, 210, 255.00, 'Delayed',     DATE '2026-05-12', 'OTP'),
    ('FR-125', 'Tel Aviv',           TIME '22:10',   TIME '02:30',   'AIR-014', 296, 270, 420.00, 'On Time',     DATE '2026-05-12', 'OTP')
) AS v(flight_code, destination, departure_time, arrival_time,
       plane_reg, total_seats, available_seats, ticket_price,
       status, departure_date, airport_code)
WHERE NOT EXISTS (
    SELECT 1 FROM flights f WHERE f.flight_code = v.flight_code
);

-- ============================================================
-- Verify
-- ============================================================
SELECT 'planes'   AS table_name, COUNT(*) FROM planes
UNION ALL
SELECT 'flights',                COUNT(*) FROM flights
UNION ALL
SELECT 'airports',               COUNT(*) FROM airports;
