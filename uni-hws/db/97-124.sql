-- [[file:97-124.org::*جزئیات و توضیحات][جزئیات و توضیحات:1]]
USE sakila;
-- جزئیات و توضیحات:1 ends here

-- [[file:97-124.org::*تمرین‌ها][تمرین‌ها:1]]
SELECT actor_id,
       first_name,
       last_name
  FROM actor
 ORDER BY last_name,
          first_name
 LIMIT 10;
-- تمرین‌ها:1 ends here

-- [[file:97-124.org::*تمرین‌ها][تمرین‌ها:2]]
SELECT actor_id,
       first_name,
       last_name
  FROM actor
 WHERE last_name IN ('WILLIAMS', 'DAVIS');
-- تمرین‌ها:2 ends here

-- [[file:97-124.org::*تمرین‌ها][تمرین‌ها:3]]
SELECT DISTINCT customer_id
  FROM rental
 WHERE date(rental_date) = '2005-07-05';
-- تمرین‌ها:3 ends here

-- [[file:97-124.org::*تمرین‌ها][تمرین‌ها:4]]
SELECT c.email,
       r.return_date
  FROM customer c
 INNER JOIN rental r
    ON c.customer_id = r.customer_id
 WHERE date(r.rental_date) = '2005-06-14'
 ORDER BY return_date DESC;
-- تمرین‌ها:4 ends here

-- [[file:97-124.org::*عبارت ~SELECT~][عبارت ~SELECT~:1]]
SELECT first_name,
       last_name
  FROM customer
 WHERE last_name = 'ZIEGLER';
-- عبارت ~SELECT~:1 ends here

-- [[file:97-124.org::*عبارت ~SELECT~][عبارت ~SELECT~:2]]
SELECT *
  FROM category;
-- عبارت ~SELECT~:2 ends here

-- [[file:97-124.org::*عبارت ~SELECT~][عبارت ~SELECT~:3]]
SELECT *
  FROM category;
-- عبارت ~SELECT~:3 ends here

-- [[file:97-124.org::*عبارت ~SELECT~][عبارت ~SELECT~:4]]
SELECT *
  FROM language;
-- عبارت ~SELECT~:4 ends here

-- [[file:97-124.org::*عبارت ~SELECT~][عبارت ~SELECT~:5]]
SELECT language_id,
       name,
       last_update
  FROM language;
-- عبارت ~SELECT~:5 ends here

-- [[file:97-124.org::*عبارت ~SELECT~][عبارت ~SELECT~:6]]
SELECT name
  FROM language;
-- عبارت ~SELECT~:6 ends here

-- [[file:97-124.org::*عبارت ~SELECT~][عبارت ~SELECT~:7]]
SELECT language_id,
       'COMMON' language_usage,
       language_id * 3.1415927 lang_pi_value,
       upper(name) language_name
  FROM language;
-- عبارت ~SELECT~:7 ends here

-- [[file:97-124.org::*عبارت ~SELECT~][عبارت ~SELECT~:8]]
SELECT version(),
       user(),
       database();
-- عبارت ~SELECT~:8 ends here

-- [[file:97-124.org::*نام مستعار][نام مستعار:1]]
SELECT language_id,
       'COMMON' AS language_usage,
       language_id * 3.1415927 AS lang_pi_value,
       upper(name) AS language_name
  FROM language;
-- نام مستعار:1 ends here

-- [[file:97-124.org::*نام مستعار][نام مستعار:2]]
SELECT actor_id
  FROM film_actor
 ORDER BY actor_id;
-- نام مستعار:2 ends here

-- [[file:97-124.org::*نام مستعار][نام مستعار:3]]
SELECT COUNT(actor_id)
  FROM film_actor
 ORDER BY actor_id;
-- نام مستعار:3 ends here

-- [[file:97-124.org::*نام مستعار][نام مستعار:4]]
SELECT DISTINCT actor_id
  FROM film_actor
 ORDER BY actor_id;
-- نام مستعار:4 ends here

-- [[file:97-124.org::*عبارت ~FROM~][عبارت ~FROM~:1]]
SELECT concat(cust.last_name, ', ', cust.first_name) full_name
  FROM (
        SELECT first_name,
               last_name,
               email
          FROM customer
         WHERE first_name = 'JESSIE'
       ) cust;
-- عبارت ~FROM~:1 ends here

-- [[file:97-124.org::*جداول موقت][جداول موقت:1]]
CREATE TEMPORARY TABLE actors_j (
    actor_id SMALLINT(5),
    first_name VARCHAR(45),
    last_name VARCHAR(45)
);
INSERT INTO actors_j SELECT actor_id,
       first_name,
       last_name
  FROM actor
 WHERE last_name LIKE 'J%';
SELECT *
  FROM actors_j;
-- جداول موقت:1 ends here

-- [[file:97-124.org::*نما][نما:1]]
DROP VIEW cust_hw;
CREATE VIEW cust_hw AS SELECT customer_id,
       first_name,
       last_name,
       active
  FROM customer;
-- نما:1 ends here

-- [[file:97-124.org::*نما][نما:2]]
SELECT first_name,
       last_name
  FROM cust_hw
 WHERE active=0;
-- نما:2 ends here

-- [[file:97-124.org::*پیوند][پیوند:1]]
SELECT customer.first_name,
       customer.last_name,
       time(rental.rental_date) rental_time
  FROM customer
 INNER JOIN rental
    ON customer.customer_id = rental.customer_id
 LIMIT 5;
-- پیوند:1 ends here

-- [[file:97-124.org::*پیوند][پیوند:2]]
SELECT c.first_name,
       c.last_name,
       time(r.rental_date) rental_time
  FROM customer AS c
 INNER JOIN rental AS r
    ON c.customer_id = r.customer_id
 WHERE date(r.rental_date) = '2005-06-14';
-- پیوند:2 ends here

-- [[file:97-124.org::*عبارت ~WHERE~][عبارت ~WHERE~:1]]
SELECT title
  FROM film
 WHERE rating = 'G'
   AND rental_duration >= 7;
-- عبارت ~WHERE~:1 ends here

-- [[file:97-124.org::*عبارت ~WHERE~][عبارت ~WHERE~:2]]
 SELECT title
   FROM film
  WHERE rating = 'G'
     OR rental_duration >= 7
  LIMIT 5;
-- عبارت ~WHERE~:2 ends here

-- [[file:97-124.org::*عبارت ~WHERE~][عبارت ~WHERE~:3]]
SELECT title,
       rating,
       rental_duration
  FROM film
 WHERE (rating = 'G' AND rental_duration >= 7)
    OR (rating = 'PG-13' AND rental_duration < 4)
 LIMIT 5;
-- عبارت ~WHERE~:3 ends here

-- [[file:97-124.org::*گروه‌ها][گروه‌ها:1]]
SELECT c.first_name,
       c.last_name,
       count(*)
  FROM customer c
 INNER JOIN rental r
    ON c.customer_id = r.customer_id
 GROUP BY c.first_name,
          c.last_name
 LIMIT 10;
-- گروه‌ها:1 ends here

-- [[file:97-124.org::*مرتب‌سازی][مرتب‌سازی:1]]
SELECT c.first_name,
       c.last_name,
       time(r.rental_date) rental_time
  FROM customer c
 INNER JOIN rental r
    ON c.customer_id = r.customer_id
 WHERE date(r.rental_date) = '2005-06-14'
 ORDER BY c.last_name,
          c.first_name;
-- مرتب‌سازی:1 ends here

-- [[file:97-124.org::*مرتب‌سازی][مرتب‌سازی:2]]
SELECT c.first_name,
       c.last_name,
       time(r.rental_date) rental_time
  FROM customer c
 INNER JOIN rental r
    ON c.customer_id = r.customer_id
 WHERE date(r.rental_date) = '2005-06-14'
 ORDER BY time(r.rental_date) DESC;
 -- ORDER BY 3 DESC;
-- مرتب‌سازی:2 ends here
