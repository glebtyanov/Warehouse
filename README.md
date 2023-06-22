# Warehouse

Docker image of api:
https://hub.docker.com/repository/docker/rhapsodiczeus/warehouse/tags?page=1&ordering=last_updated
docker pull rhapsodiczeus/warehouse:1.0

Entities:
  1. Department - M:M with Worker through DepartmentWorkers entity
  2. Worker -  M:M with Department through DepartmentWorkers entity, 1:M with Order, Position
    additionaly serves for authentication.
  3. Position (of Worker) - 1:M with Worker
    serves as Role for authorization
  4. Order - 1:M with Status, Customer, Worker, Product and 1:1 with Transaction
  5. Status (of Order) - 1:M with Order
  6. Customer - 1:M with Order
  7. Product - 1:M with Order

Script to fill database:

-- Insert statuses
INSERT INTO Statuses (Name)
VALUES ('Done'), ('Waiting for payment'), ('Canceled'), ('OnTheWay'), ('Processed')

-- Insert positions
INSERT INTO Positions (Name)
VALUES ('CEO'), ('Manager'), ('Regular');

-- Insert customers
INSERT INTO Customers (Name, Address, ContactNumber, Email)
VALUES ('John Doe', '123 Main St', '555-1234', 'john@example.com'),
('Jane Smith', '456 Elm St', '555-5678', 'jane@example.com');

-- Insert departments
INSERT INTO Departments (Name, Location, Capacity)
VALUES ('Sales', 'Floor 1', 10),
('Marketing', 'Floor 2', 8);

-- Insert workers
INSERT INTO Workers (FirstName, LastName, Address, ContactNumber, Email, PositionId, PasswordHash, PasswordSalt, HireDate)
VALUES ('Jessy', 'Pinkman', 'Bryanskaya 12', '375445545544', 'jessy@example.com', 3, 0x, 0x, GETDATE()),
       ('Walter', 'White', 'Legendarnaya 8', '375293488775', 'walter@example.com', 2, 0x, 0x, GETDATE()),
       ('Gustavo', 'Fring', 'Svetskaya 8', '375293445845', 'gus@example.com', 1, 0x, 0x, GETDATE());

--Insert department workers
INSERT INTO DepartmentWorkers (DepartmentId, WorkerId)
VALUES (1, 1), (2, 2), (1,2), (2,3), (1,3)


 --Insert orders
INSERT INTO Orders (OrderDate, ProductAmount, StatusId, WorkerId, CustomerId)
VALUES (GETDATE(), 100.0, 1, 2, 1),
       (GETDATE(), 200.0, 1, 3, 2);

-- Insert products
INSERT INTO Products (Name, Description, Quantity, Price)
VALUES ('Product 1', 'Description 1', 10, 9.99),
       ('Product 2', 'Description 2', 20, 19.99);

-- Insert order products
INSERT INTO OrderProducts (OrderId, ProductId)
VALUES (1, 1),
       (2, 2);

-- Insert transactions
INSERT INTO Transactions (OrderId, TransactionDate, Amount, PaymentMethod)
VALUES (1, GETDATE(), 100.0, 'Credit Card'),
       (2, GETDATE(), 200.0, 'PayPal');
