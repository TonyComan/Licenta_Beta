

-- Tabelă pentru utilizatori (login)
CREATE TABLE utilizator (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    parola_hash VARCHAR(255) NOT NULL,
    rol VARCHAR(20) NOT NULL CHECK (rol IN ('admin', 'medic', 'receptie'))
);
INSERT INTO utilizator (username, parola_hash, rol) VALUES
('admin1', 'hash_admin', 'admin'),
('medic1', 'hash_medic', 'medic'),
('receptie1', 'hash_receptie', 'receptie');


-- Tabelă pacienți
CREATE TABLE pacient (
    id SERIAL PRIMARY KEY,
    nume VARCHAR(50) NOT NULL,
    prenume VARCHAR(50) NOT NULL,
    cnp VARCHAR(13) UNIQUE NOT NULL,
    data_nasterii DATE NOT NULL,
    telefon VARCHAR(10) NOT NULL,
    email VARCHAR(50) NOT NULL,
    adresa VARCHAR(100) NOT NULL
);
INSERT INTO pacient (nume, prenume, cnp, data_nasterii, telefon, email, adresa) VALUES
('Popescu', 'Andrei', '1970504123456', '1997-05-04', '0722333444', 'andrei.popescu@email.com', 'Str. Lalelelor nr. 10'),
('Ionescu', 'Maria', '2860101123456', '1986-01-01', '0744555666', 'maria.ionescu@email.com', 'Bd. Revolutiei nr. 3');

-- Tabelă medici
CREATE TABLE medic (
    id SERIAL PRIMARY KEY,
    nume VARCHAR(50) NOT NULL,
    prenume VARCHAR(50) NOT NULL,
    specializare VARCHAR(50) NOT NULL,
    telefon VARCHAR(10) NOT NULL,
    email VARCHAR(50) NOT NULL
);
INSERT INTO medic (nume, prenume, specializare, telefon, email) VALUES
('Dumitrescu', 'Raluca', 'Ortodontie', '0733111222', 'raluca.dumitrescu@dentalpro.ro'),
('Georgescu', 'Mihai', 'Implantologie', '0721999888', 'mihai.georgescu@dentalpro.ro');


-- Tabelă programări
CREATE TABLE programare (
    id SERIAL PRIMARY KEY,
    pacient_id INT REFERENCES pacient(id) ON DELETE CASCADE,
    medic_id INT REFERENCES medic(id) ON DELETE CASCADE,
    data_programare DATE NOT NULL,
    ora_programare TIME NOT NULL,
    status VARCHAR(20) NOT NULL DEFAULT 'activ'
);
INSERT INTO programare (pacient_id, medic_id, data_programare, ora_programare, status) VALUES
(1, 1, '2025-05-25', '10:00', 'activ'),
(2, 2, '2025-05-26', '14:30', 'activ');


-- Tabelă servicii medicale
CREATE TABLE serviciu_medical (
    id SERIAL PRIMARY KEY,
    denumire VARCHAR(50) NOT NULL,
    descriere TEXT NOT NULL,
    pret NUMERIC(10, 2) NOT NULL
);
INSERT INTO serviciu_medical (denumire, descriere, pret) VALUES
('Detartraj', 'Curățarea tartrului dentar cu ultrasunete', 150.00),
('Albire dentară', 'Tratament cosmetic pentru albirea dinților', 400.00),
('Implant dentar', 'Inserare implant dentar titan', 2500.00);


-- Tabelă tratamente
CREATE TABLE tratament (
    id SERIAL PRIMARY KEY,
    pacient_id INT REFERENCES pacient(id) ON DELETE CASCADE,
    medic_id INT REFERENCES medic(id) ON DELETE CASCADE,
    serviciu_id INT REFERENCES serviciu_medical(id),
    descriere TEXT NOT NULL,
    data_tratament DATE DEFAULT CURRENT_DATE,
    observatii TEXT
);
INSERT INTO tratament (pacient_id, medic_id, serviciu_id, descriere, observatii) VALUES
(1, 1, 1, 'Detartraj complet arcade superioare și inferioare', 'Recomandare control în 6 luni'),
(2, 2, 3, 'Implant molar stânga jos', 'Sângerare redusă, pacient cooperant');


-- Tabelă facturi
CREATE TABLE factura (
    id SERIAL PRIMARY KEY,
    tratament_id INT REFERENCES tratament(id) ON DELETE CASCADE,
    data_emitere DATE DEFAULT CURRENT_DATE,
    total NUMERIC(10, 2) NOT NULL,
    metoda_plata VARCHAR(20) NOT NULL
);
INSERT INTO factura (tratament_id, total, metoda_plata) VALUES
(1, 150.00, 'Numerar'),
(2, 2500.00, 'Card');


-- Tabelă rețete
CREATE TABLE reteta (
    id SERIAL PRIMARY KEY,
    tratament_id INT REFERENCES tratament(id) ON DELETE CASCADE,
    durata VARCHAR(50) NOT NULL,
    dozaj VARCHAR(30) NOT NULL,
    medicament VARCHAR(50) NOT NULL
);
INSERT INTO reteta (tratament_id, durata, dozaj, medicament) VALUES
(2, '7 zile', '3x/zi după mese', 'Augmentin 875mg');


-- Tabelă tehnicieni dentari
CREATE TABLE tehnician_dentar (
    id SERIAL PRIMARY KEY,
    nume VARCHAR(50) NOT NULL,
    prenume VARCHAR(50) NOT NULL,
    specializare VARCHAR(50) NOT NULL,
    telefon VARCHAR(10) NOT NULL,
    email VARCHAR(50) NOT NULL,
    medic_colaborant VARCHAR(100)
);
INSERT INTO tehnician_dentar (nume, prenume, specializare, telefon, email, medic_colaborant) VALUES
('Vasilescu', 'Ion', 'Protezare', '0741234567', 'ion.vasilescu@labdentar.ro', 'Georgescu Mihai');

