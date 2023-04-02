DROP DATABASE IF EXISTS rlask_db;
CREATE DATABASE rlask_db;
USE rlask_db;


CREATE TABLE contractors
(
  contractor_id INT NOT NULL AUTO_INCREMENT,
  contractor_name VARCHAR(255) NOT NULL,
  contractor_address VARCHAR(255) NOT NULL,
  is_deleted BOOLEAN DEFAULT 0,
  PRIMARY KEY (contractor_id)
);


CREATE TABLE customers
(
  customer_id INT NOT NULL AUTO_INCREMENT,
  customer_name VARCHAR(255) NOT NULL,
  customer_address VARCHAR(255) NOT NULL,
  is_deleted BOOLEAN DEFAULT 0,
  PRIMARY KEY (customer_id)
);

CREATE TABLE invoices
(
  invoice_id UUID NOT NULL,
  invoice_date DATETIME NOT NULL,
  days_to_pay INT NOT NULL,
  contractor_id INT NOT NULL,
  customer_id INT NOT NULL,
  extra_details VARCHAR(15382),
  PRIMARY KEY (invoice_id),
  CONSTRAINT fk_invoices_contractors_contractor_id FOREIGN KEY (contractor_id) REFERENCES contractors(contractor_id),
  CONSTRAINT fk_invoices_customers_customer_id FOREIGN KEY (customer_id) REFERENCES customers(customer_id)
);


CREATE TABLE products
(
  product_id INT NOT NULL AUTO_INCREMENT,
  product_name VARCHAR(255) NOT NULL,
  unit VARCHAR(255) NOT NULL,
  unit_price DECIMAL(15,2) NOT NULL,
  is_material BOOLEAN NOT NULL,
  is_deleted BOOLEAN DEFAULT 0,
  picture BLOB,
  PRIMARY KEY (product_id)
);

CREATE TABLE product_details
(
  product_detail_id INT NOT NULL AUTO_INCREMENT,
  property_name VARCHAR(255) NOT NULL,
  property_value VARCHAR(15382),
  product_id INT NOT NULL,
  CONSTRAINT fk_product_details_products_product_id FOREIGN KEY (product_id) REFERENCES products(product_id),
  PRIMARY KEY (product_detail_id)
);


CREATE TABLE invoice_rows
(
  invoice_row_id INT NOT NULL AUTO_INCREMENT,
  product_id INT NOT NULL,
  unit_price DECIMAL(15,2) NOT NULL,
  amount DECIMAL(15,2) NOT NULL,
  invoice_id UUID NOT NULL,
  PRIMARY KEY (invoice_row_id),
  CONSTRAINT fk_invoice_rows_invoices_invoice_id FOREIGN KEY (invoice_id) REFERENCES invoices(invoice_id),
  CONSTRAINT fk_invoice_rows_products_product_id FOREIGN KEY (product_id) REFERENCES products(product_id)
);



CREATE VIEW invoices_total_sum_view AS 
SELECT invoice_id, SUM(unit_price * amount) AS total_sum FROM invoice_rows GROUP BY invoice_id;

CREATE VIEW invoices_text_view AS 
SELECT i.invoice_id, i.invoice_date, ADDDATE(i.invoice_date,i.days_to_pay) AS due_date, t.total_sum, cu.customer_name, cu.customer_address, i.extra_details
FROM invoices i JOIN customers cu ON i.customer_id = cu.customer_id JOIN invoices_total_sum_view t ON i.invoice_id = t.invoice_id;





DELIMITER &&  
CREATE PROCEDURE insert_row_freeze_price (IN invoiceid UUID, productid INT, amount decimal)
BEGIN  
INSERT INTO invoice_rows (product_id, unit_price, amount, invoice_id) 
VALUES (productid, (SELECT unit_price FROM products WHERE products.product_id=productid), amount, invoiceid);
END &&  
DELIMITER;


DELIMITER &&  
CREATE PROCEDURE get_invoice_by_invoice_id (IN invoiceid UUID)
BEGIN  
SELECT i.invoice_date, i.days_to_pay, co.contractor_name, co.contractor_address, cu.customer_name, cu.customer_address, i.extra_details
FROM invoices i JOIN contractors co ON i.contractor_id = co.contractor_id JOIN customers cu ON i.customer_id = cu.customer_id
WHERE i.invoice_id = invoiceid;
END &&  
DELIMITER ;

DELIMITER &&  
CREATE PROCEDURE get_invoices_text_view_by_customer_id (IN customerid INT)
BEGIN  
SELECT i.invoice_id, i.invoice_date, ADDDATE(i.invoice_date,i.days_to_pay) AS due_date, t.total_sum, cu.customer_name, cu.customer_address, i.extra_details
FROM invoices i JOIN customers cu ON i.customer_id = cu.customer_id JOIN invoices_total_sum_view t ON i.invoice_id = t.invoice_id
WHERE i.customer_id = customerid;
END &&  
DELIMITER ;


DELIMITER &&  
CREATE PROCEDURE get_rows_by_invoice_id (IN invoiceid UUID)  
BEGIN  
SELECT p.product_name, r.amount, p.unit, r.unit_price, r.amount * r.unit_price AS subtotal, p.is_material 
FROM products p JOIN invoice_rows r ON p.product_id = r.product_id
WHERE r.invoice_id = invoiceid
ORDER BY is_material DESC;
END &&  
DELIMITER ; 


