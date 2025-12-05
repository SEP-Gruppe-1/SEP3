-- Opret schema og sæt search path
CREATE SCHEMA IF NOT EXISTS cinema;
SET search_path TO cinema;

-- Kunde-tabellen
CREATE TABLE IF NOT EXISTS Customer (
                                        phone varchar(15) PRIMARY KEY,
                                        name varchar(100),
                                        password varchar(50) NOT NULL CHECK (
                                            password ~ '[A-Z]' AND
                                            password ~ '[a-z]' AND
                                            password ~ '[0-9]' AND
                                            password ~ '[^a-zA-Z0-9]' AND
                                            LENGTH(password) >= 8
                                            ),
                                        email varchar(100) UNIQUE NOT NULL CHECK (email LIKE '%@%.%')
);

-- Layout til sæder
CREATE TABLE IF NOT EXISTS seat_layout (
                                           layout_id serial PRIMARY KEY,
                                           name varchar(50) NOT NULL
);

-- Biografsal (hall)
CREATE TABLE IF NOT EXISTS hall (
                                    hall_id serial PRIMARY KEY,
                                    hall_number smallint UNIQUE NOT NULL,
                                    capacity smallint NOT NULL CHECK (capacity > 0),
                                    layout_id integer REFERENCES seat_layout(layout_id)
);

-- Film-tabellen
CREATE TABLE IF NOT EXISTS movie (
                                     movie_id serial PRIMARY KEY,
                                     title varchar(100) NOT NULL,
                                     duration_minutes smallint CHECK (duration_minutes > 0),
                                     genre varchar(50),
                                     release_date date
);

-- Forestillinger
CREATE TABLE IF NOT EXISTS Screening (
                                         screening_id serial PRIMARY KEY,
                                         movie_id integer NOT NULL REFERENCES movie(movie_id),
                                         hall_id integer NOT NULL REFERENCES hall(hall_id),
                                         screening_time timestamp NOT NULL,
                                         available_seats smallint CHECK (available_seats >= 0)
);



CREATE TABLE IF NOT EXISTS layout_seat (
                                           layout_seat_id serial PRIMARY KEY,
                                           layout_id integer NOT NULL REFERENCES seat_layout(layout_id) ON DELETE CASCADE,
                                           row_letter char(1) NOT NULL CHECK (row_letter ~ '^[A-Z]$'),
                                           seat_number smallint NOT NULL CHECK (seat_number > 0),
                                           UNIQUE (layout_id, row_letter, seat_number)
);

-- Fysiske sæder i hall
CREATE TABLE IF NOT EXISTS seat (
                                    seat_id serial PRIMARY KEY,
                                    hall_id integer NOT NULL REFERENCES hall(hall_id),
                                    row_letter char(1) NOT NULL CHECK (row_letter ~ '^[A-Z]$'),
                                    seat_number smallint NOT NULL CHECK (seat_number > 0),
                                    UNIQUE (hall_id, row_letter, seat_number)
);

-- Booking-tabeller
CREATE TABLE IF NOT EXISTS Booking (
                                       booking_id serial PRIMARY KEY,
                                       customer_phone varchar(15) NOT NULL REFERENCES Customer(phone),
                                       screening_id integer NOT NULL REFERENCES Screening(screening_id),
                                       seats_booked smallint CHECK (seats_booked > 0)
);

CREATE TABLE IF NOT EXISTS BookingSeat (
                                           booking_id integer NOT NULL REFERENCES Booking(booking_id) ON DELETE CASCADE,
                                           seat_id integer NOT NULL REFERENCES seat(seat_id),
                                           PRIMARY KEY (booking_id, seat_id)
);

-- Drop gammel constraint hvis nødvendigt
ALTER TABLE seat DROP CONSTRAINT IF EXISTS uq_seat_screen_row_num;
DROP INDEX IF EXISTS uq_seat_screen_row_num;

-- Tilføj ny constraint
ALTER TABLE seat ADD CONSTRAINT uq_seat_hall_row_num UNIQUE (hall_id, row_letter, seat_number);

-- Indsæt layout og sæder
BEGIN;

WITH small AS (
    INSERT INTO seat_layout(name) VALUES ('SmallHall') RETURNING layout_id
)
INSERT INTO layout_seat(layout_id, row_letter, seat_number)
SELECT s.layout_id, chr(ascii('A') + r)::char(1), n
FROM small s, generate_series(0,3) AS r, generate_series(1,10) AS n;

WITH big AS (
    INSERT INTO seat_layout(name) VALUES ('BigHall') RETURNING layout_id
)
INSERT INTO layout_seat(layout_id, row_letter, seat_number)
SELECT b.layout_id, chr(ascii('A') + r)::char(1), n
FROM big b, generate_series(0,7) AS r, generate_series(1,12) AS n;

-- Opret halls og sæder
WITH h1 AS (
    INSERT INTO hall(hall_number, capacity, layout_id)
        VALUES (1, 40, (SELECT layout_id FROM seat_layout WHERE name = 'SmallHall'))
        RETURNING hall_id, layout_id
),
     h2 AS (
         INSERT INTO hall(hall_number, capacity, layout_id)
             VALUES (2, 96, (SELECT layout_id FROM seat_layout WHERE name = 'BigHall'))
             RETURNING hall_id, layout_id
     )
INSERT INTO seat(hall_id, row_letter, seat_number)
SELECT h1.hall_id, ls.row_letter, ls.seat_number
FROM layout_seat ls CROSS JOIN h1
WHERE ls.layout_id = h1.layout_id;

WITH h2ref AS (
    SELECT hall_id, layout_id FROM hall WHERE hall_number = 2
)
INSERT INTO seat(hall_id, row_letter, seat_number)
SELECT h2ref.hall_id, ls.row_letter, ls.seat_number
FROM layout_seat ls CROSS JOIN h2ref
WHERE ls.layout_id = h2ref.layout_id;

-- Indsæt film
INSERT INTO movie(title, duration_minutes, genre, release_date)
VALUES
    ('The Matrix', 136, 'Sci-Fi', '1999-03-31'),
    ('Spirited Away', 125, 'Animation', '2001-07-20');

-- Opret forestillinger
INSERT INTO Screening(movie_id, hall_id, screening_time, available_seats)
VALUES
    (
        (SELECT movie_id FROM movie WHERE title = 'The Matrix'),
        (SELECT hall_id FROM hall WHERE hall_number = 1),
        TIMESTAMP '2025-10-25 19:30:00',
        (SELECT COUNT(*) FROM seat WHERE hall_id = (SELECT hall_id FROM hall WHERE hall_number = 1))
    ),
    (
        (SELECT movie_id FROM movie WHERE title = 'Spirited Away'),
        (SELECT hall_id FROM hall WHERE hall_number = 2),
        TIMESTAMP '2025-10-25 17:00:00',
        (SELECT COUNT(*) FROM seat WHERE hall_id = (SELECT hall_id FROM hall WHERE hall_number = 2))
    );

-- Opret kunder
INSERT INTO Customer(name, password, email, phone)
VALUES
    ('Alice Example', 'Str0ng!Pass', 'alice@example.com', '5550001'),
    ('Bob Example',   'An0ther$1',   'bob@example.com',   '5550002');

-- Alice booker A1 og A2 i hall 1
WITH chosen_screening AS (
    SELECT screening_id FROM Screening
    WHERE movie_id = (SELECT movie_id FROM movie WHERE title = 'The Matrix')
      AND hall_id = (SELECT hall_id FROM hall WHERE hall_number = 1)
),
     b AS (
         INSERT INTO Booking(customer_phone, screening_id, seats_booked)
             VALUES ('5550001', (SELECT screening_id FROM chosen_screening), 2)
             RETURNING booking_id
     )
INSERT INTO BookingSeat(booking_id, seat_id)
SELECT b.booking_id, s.seat_id
FROM b
         JOIN seat s ON s.hall_id = (SELECT hall_id FROM hall WHERE hall_number = 1)
WHERE (s.row_letter, s.seat_number) IN ( ('A',1), ('A',2) );

ALTER TABLE Customer
    ADD COLUMN role VARCHAR(50) NOT NULL DEFAULT 'customer'
        CHECK (role IN ('customer', 'employee', 'admin'));

COMMIT;
