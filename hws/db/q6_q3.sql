-- MohammadYaasin Daavoodeh
-- Q3
DROP DATABASE davoodeh;
CREATE DATABASE davoodeh;
USE davoodeh;

CREATE TABLE students
(
    id INT, -- be tore zemni id shaamele terme vorudi va Code-e reshte hast
    first_name VARCHAR(30) NOT NULL,
    last_name VARCHAR(30) NOT NULL,
    password VARCHAR(64), -- ba3dan tavaste narmafzaar va xode shaxs por mishavad
    addr VARCHAR(255) NOT NULL,
    phone VARCHAR(9) UNIQUE NOT NULL, -- chon 0 va 9 moshtarake
    phone_secondary VARCHAR(9),

    CONSTRAINT pk_id PRIMARY KEY(id)
);

CREATE TABLE classes
(
    id INT AUTO_INCREMENT, -- be tore zemni id shaamele terme vorudi va Code-e reshte hast
    student_id INT,
    term CHAR(3),
    tutor_id INT, -- FK jadvali farzi
    classroom VARCHAR(255), -- 255 ke agar Link majaazi bud ja shavad. agar Code-e Class Physic-i baashad 10 ham kaafi ast.
    added_at TIMESTAMP NOT NULL,
    updated_at TIMESTAMP,
    hazf_at TIMESTAMP, -- agar meqdaar dasht hazf karde

    CONSTRAINT pk_id PRIMARY KEY(id),
    FOREIGN KEY (student_id) REFERENCES students(id)
);


-- Population
INSERT INTO students (id, first_name, last_name, addr, phone) VALUES
    (981001, 'Ali', 'Alipur', 'Tehran, A. Alichap', '161234567'),
    (981002, 'Baqer', 'Baqeri', 'Tehran, A. Baqeri', '261234567')
;

INSERT INTO classes (student_id, term, tutor_id, classroom, added_at) VALUES
    (981001, '981', 1, 'Edaari 123', '2020-01-01 12:12:12'),
    (981002, '982', 1, 'https://conflink.ext/982-1', '2020-01-01 12:15:12')
;

-- Queries
SELECT first_name, last_name FROM students;

SELECT first_name, last_name, term, tutor_id, classes.classroom FROM students
INNER JOIN classes ON students.id = classes.student_id;
