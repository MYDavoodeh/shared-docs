-- [[file:reservation.org::*شمایه][شمایه:1]]
DROP DATABASE hw_db_reservation;
-- شمایه:1 ends here

-- [[file:reservation.org::*شمایه][شمایه:2]]
CREATE DATABASE hw_db_reservation;
USE hw_db_reservation;
-- شمایه:2 ends here

-- [[file:reservation.org::*شمایه][شمایه:3]]
CREATE TABLE employees (
    id SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    fname VARCHAR(20) NOT NULL,
    lname VARCHAR(20) NOT NULL
);

CREATE TABLE foods (
    id SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(20) NOT NULL,
    ingredients VARCHAR(20),
    available SMALLINT,
    updated_at TIMESTAMP NULL
);

CREATE TABLE halls (
    id SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(20),
    capacity SMALLINT,
    updated_at TIMESTAMP NULL
);

CREATE TABLE reservations (
    id SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    employee_id SMALLINT UNSIGNED NOT NULL,
    voted_for SMALLINT UNSIGNED NOT NULL,
    food_id SMALLINT UNSIGNED,
    hall_id SMALLINT UNSIGNED,
    taken_at TIMESTAMP NULL,
    exited_at TIMESTAMP NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (employee_id) REFERENCES employees(id),
    FOREIGN KEY (hall_id) REFERENCES halls(id),
    FOREIGN KEY (voted_for) REFERENCES foods(id),
    FOREIGN KEY (food_id) REFERENCES foods(id)
);
-- شمایه:3 ends here

-- [[file:reservation.org::show_tables][show_tables]]
SHOW TABLES;
-- show_tables ends here

-- [[file:reservation.org::*مقداردهی][مقداردهی:1]]
INSERT INTO foods (name, ingredients)
VALUES ('Burger', 'meat,cheese,tomato'), ('Cheese sandwich', 'cheese,tomato');

INSERT INTO employees (fname, lname)
VALUES ('Ali', 'Alipour'), ('Mamad', 'Mamadi');

INSERT INTO halls (name, capacity)
VALUES ('Western', 30), ('Eastern', 40);
-- مقداردهی:1 ends here

-- [[file:reservation.org::*مقداردهی][مقداردهی:2]]
INSERT INTO reservations (employee_id, voted_for)
VALUES (1, 1), (2, 1);
-- مقداردهی:2 ends here

-- [[file:reservation.org::*کوئری][کوئری:1]]
UPDATE foods
   SET available=(
        SELECT COUNT(*)
          FROM reservations
         WHERE food_id IS NULL
           AND voted_for=1
           AND created_at BETWEEN now() - INTERVAL 1 WEEK AND now()
       )
 WHERE id=2;
-- کوئری:1 ends here

-- [[file:reservation.org::*کوئری][کوئری:2]]
SELECT *
  FROM foods;
-- کوئری:2 ends here

-- [[file:reservation.org::*کوئری][کوئری:3]]
UPDATE reservations
   SET food_id=2
 WHERE voted_for IN (1,2)
   AND food_id IS NULL
   AND created_at BETWEEN now() - INTERVAL 1 WEEK AND now();
-- کوئری:3 ends here

-- [[file:reservation.org::*کوئری][کوئری:4]]
SELECT *
  FROM reservations;
-- کوئری:4 ends here

-- [[file:reservation.org::*کوئری][کوئری:5]]
SELECT *
  FROM halls;
-- کوئری:5 ends here

-- [[file:reservation.org::*کوئری][کوئری:6]]
SELECT *
  FROM employees;
-- کوئری:6 ends here

-- [[file:reservation.org::*کوئری][کوئری:8]]
DESCRIBE employees;
DESCRIBE foods;
DESCRIBE halls;
DESCRIBE reservations;
-- کوئری:8 ends here
