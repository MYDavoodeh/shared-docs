DROP DATABASE IF EXISTS hw10_store;
CREATE DATABASE hw10_store;
USE hw10_store;

CREATE TABLE users (
    id SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    email VARCHAR(100) UNIQUE NOT NULL,
    password VARCHAR(64), -- for hashing algorithms
    address VARCHAR(100),
    phone VARCHAR(9)
);
CREATE TABLE categories (
    id SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(20),
    parent SMALLINT UNSIGNED,

    FOREIGN KEY (parent) REFERENCES categories(id)
);
CREATE TABLE products (
    id SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(20),
    category_id SMALLINT UNSIGNED,
    stock SMALLINT UNSIGNED,
    price INT,
    description VARCHAR(100),
    updated_at TIMESTAMP NULL,

    FOREIGN KEY (category_id) REFERENCES categories(id)
);
CREATE TABLE orders (
    id SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    user_id SMALLINT UNSIGNED,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NULL,
    approved_at TIMESTAMP NULL,

    FOREIGN KEY (user_id) REFERENCES users(id)
);
CREATE TABLE order_products (
    id SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    product_id SMALLINT UNSIGNED NOT NULL,
    order_id SMALLINT UNSIGNED NOT NULL,
    qty SMALLINT UNSIGNED DEFAULT 1,

    FOREIGN KEY (product_id) REFERENCES products(id),
    FOREIGN KEY (order_id) REFERENCES orders(id)
);

SHOW TABLES;

INSERT INTO users (email, password, address, phone) VALUES
(
    'mamad@site.ext',
    '$2y$12$LcIiLu8e2vr97RAp.LUOLuOKz9D5yA5PmT5r4ste0X6OKwNz.6Mfe',
    'Iran, Tehran',
    '303456789'
),
(
    'ali@site.ext',
    '$2y$12$k4P4K07CgEQBV0i2LO3KGuFrDGx9MVLvbE8F9XYBNF6q/jvdvOepe',
    'Iran, Sistan',
    '303456789'
);
INSERT INTO categories (name, parent) VALUES
('Self-care', NULL),
('Shampoo', 1),
('Electronics', NULL),
('Keyboard', 3);
INSERT INTO orders (user_id) VALUES (1), (1), (1), (2), (2);
INSERT INTO products (name, category_id, stock, price, description) VALUES
('Xcare', 2, 3, 15000, 'For men'),
('Kplus Keyboard', 4, 8, 200000, 'For pros');
INSERT INTO order_products (product_id, order_id, qty) VALUES
(1, 1, 1),
(1, 2, 2),
(1, 3, 1),
(2, 3, 1),
(1, 4, 1),
(2, 5, 1);

SELECT id,
       email,
       address,
       phone
  FROM users;

SELECT *
  FROM categories;

SELECT *
  FROM orders;

SELECT *
  FROM products;

SELECT *
  FROM order_products;

SELECT products.id,
       products.name
  FROM products
  JOIN order_products
    ON products.id=order_products.product_id
  JOIN orders
    ON orders.id=order_products.order_id
 WHERE user_id = (
        SELECT id
          FROM users
         WHERE email='ali@site.ext'
       );

SELECT order_id,
       products.id,
       products.name,
       order_products.qty
  FROM products
  JOIN categories
    ON categories.id=products.category_id
  JOIN order_products
    ON order_products.product_id=products.id
  JOIN orders
    ON order_products.order_id=orders.id
 WHERE user_id=1
   AND orders.id IN (
        SELECT order_id -- only take order_id
          FROM (
                SELECT count(*) AS product_count,
                       order_id
                  FROM order_products
                 GROUP BY order_id
                HAVING product_count>=2
               ) AS counter
       );
