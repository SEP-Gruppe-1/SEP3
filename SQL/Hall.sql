-- Opret schema og sæt search path
CREATE SCHEMA IF NOT EXISTS cinema;
SET search_path TO cinema;


DROP TABLE IF EXISTS bookingseat CASCADE;
DROP TABLE IF EXISTS booking CASCADE;
DROP TABLE IF EXISTS seat CASCADE;
DROP TABLE IF EXISTS layout_seat CASCADE;
DROP TABLE IF EXISTS screening CASCADE;
DROP TABLE IF EXISTS hall CASCADE;
DROP TABLE IF EXISTS movie CASCADE;
DROP TABLE IF EXISTS seat_layout CASCADE;
DROP TABLE IF EXISTS customer CASCADE;

-- Kunde-tabellen

CREATE TABLE IF NOT EXISTS Customer (
                                        phone varchar(15) PRIMARY KEY,
                                        name varchar(100),
                                        password varchar(100) NOT NULL,
                                        email varchar(100) UNIQUE NOT NULL CHECK (email LIKE '%@%.%'),
                                        role VARCHAR(50) NOT NULL DEFAULT 'Customer'
                                            CHECK (role IN ('Customer', 'Employee', 'Admin'))
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
                                     release_date date,
                                     description text,
                                     poster_url varchar,
                                     banner_url varchar
);

-- Forestillinger
CREATE TABLE IF NOT EXISTS Screening (
                                         screening_id serial PRIMARY KEY,
                                         movie_id integer NOT NULL REFERENCES movie(movie_id),
                                         hall_id integer NOT NULL REFERENCES hall(hall_id),
                                         screening_date DATE NOT NULL,
                                         start_time TIME NOT NULL,
                                         available_seats smallint CHECK (available_seats >= 0)
);

ALTER TABLE Screening
    ADD CONSTRAINT uq_screening_hall_time
        UNIQUE (hall_id, screening_date, start_time);

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
ALTER TABLE seat DROP CONSTRAINT IF EXISTS uq_seat_hall_row_num;
DROP INDEX IF EXISTS uq_seat_hall_row_num;
-- Tilføj ny constraint
ALTER TABLE seat ADD CONSTRAINT uq_seat_hall_row_num UNIQUE (hall_id, row_letter, seat_number);

-- Seat_Layout opdateret tabel
ALTER TABLE seat_layout
    ADD COLUMN max_row_letter char(1),
    ADD COLUMN max_seat_number smallint;

-- opdatering indsat
UPDATE seat_layout sl
SET
    max_row_letter = sub.max_row_letter,
    max_seat_number = sub.max_seat_number
FROM (
         SELECT
             layout_id,
             MAX(row_letter) AS max_row_letter,
             MAX(seat_number) AS max_seat_number
         FROM layout_seat
         GROUP BY layout_id
     ) AS sub
WHERE sl.layout_id = sub.layout_id;



-- automatisk opdaterering af Seat_layout
CREATE OR REPLACE FUNCTION update_seat_layout_stats()
    RETURNS trigger AS $$
BEGIN
    UPDATE seat_layout
    SET
        max_row_letter = (SELECT MAX(row_letter) FROM layout_seat WHERE layout_id = NEW.layout_id),
        max_seat_number = (SELECT MAX(seat_number) FROM layout_seat WHERE layout_id = NEW.layout_id)
    WHERE layout_id = NEW.layout_id;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;
-- trigger til opdatering
CREATE TRIGGER trg_update_seat_layout
    AFTER INSERT OR UPDATE OR DELETE ON layout_seat
    FOR EACH ROW
EXECUTE FUNCTION update_seat_layout_stats();


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
INSERT INTO movie(title, duration_minutes, genre, release_date, description, poster_url, banner_url)
VALUES
    ('The Matrix', 136, 'Sci-Fi', '1999-03-31', 'The Matrix is a simulated reality (a computer-generated dream world) created by sentient machines to pacify humanity, using human bodies as an energy source while their minds live out a false, 1999-era life, until rebels like Morpheus discover the truth and recruit programmer Neo to fight the machines and free mankind, offering choice between illusion and harsh reality', 'https://m.media-amazon.com/images/M/MV5BN2NmN2VhMTQtMDNiOS00NDlhLTliMjgtODE2ZTY0ODQyNDRhXkEyXkFqcGc@._V1_.jpg', 'https://static.posters.cz/image/hp/69061.jpg'),
    ('Spirited Away', 125, 'Animation', '2001-07-20', 'Spirited Away is a Hayao Miyazaki masterpiece about 10-year-old Chihiro, who gets trapped in a magical world of gods and spirits after her parents are turned into pigs; she must work at a bathhouse run by the witch Yubaba to survive, find her courage, reclaim her name, and save her family, encountering strange creatures like No-Face and learning lessons about greed, identity, and compassion.', 'https://m.media-amazon.com/images/M/MV5BNTEyNmEwOWUtYzkyOC00ZTQ4LTllZmUtMjk0Y2YwOGUzYjRiXkEyXkFqcGc@._V1_.jpg', 'https://www.fathomentertainment.com/wp-content/uploads/GF23_FathomBanners_2023-02-22_SpiritedAway_1920x700.jpg.jpg');

-- Opret forestillinger
INSERT INTO Screening(movie_id, hall_id, screening_date, start_time, available_seats)
VALUES
    (
        (SELECT movie_id FROM movie WHERE title = 'The Matrix'),
        (SELECT hall_id FROM hall WHERE hall_number = 1),
        date '2025-10-25',
        time '19:30:00',
        (SELECT COUNT(*) FROM seat WHERE hall_id = (SELECT hall_id FROM hall WHERE hall_number = 1))
    ),
    (
        (SELECT movie_id FROM movie WHERE title = 'Spirited Away'),
        (SELECT hall_id FROM hall WHERE hall_number = 2),
        date '2025-10-25',
        time '17:00:00',
        (SELECT COUNT(*) FROM seat WHERE hall_id = (SELECT hall_id FROM hall WHERE hall_number = 2))
    );

COMMIT;
