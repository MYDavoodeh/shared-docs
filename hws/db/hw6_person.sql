DROP DATABASE IF EXISTS hw6_person;
CREATE DATABASE hw6_person;
USE hw6_person;

DROP TABLE IF EXISTS persons;
CREATE TABLE persons
(
    id SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    national_id CHAR(10) UNIQUE,
    fname VARCHAR(20),
    lname VARCHAR(20),
    eye_color CHAR(2),
    bd DATE,
    postal_code CHAR(10) -- hazfe address con az postal_code qaabele xaandan ast
);

TRUNCATE TABLE persons;
INSERT INTO persons (national_id, fname, lname, eye_color, bd, postal_code)
VALUES ("1924567890", "Ali", "Alipour", "BR", "2000-12-31", "1234567890"),
       ("2924567890", "Faride", "Elaahipour", "BL", "1981-12-31", "1234567890");

SELECT *
  FROM persons;
