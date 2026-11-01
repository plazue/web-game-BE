CREATE DATABASE PhongCachXanhDB;
GO

USE PhongCachXanhDB;
GO

-- 1. Categories Table (For managing categories in the store)
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL,
    Slug NVARCHAR(100) NOT NULL
);

-- 2. Users Table (Supports Customer and Admin roles for login/registration)
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) DEFAULT 'Customer', -- Values: 'Customer' or 'Admin'
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- 3. Products Table (Stores items displayed in categories and detail pages like SP1-SP4)
CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryID INT NULL,
    Brand NVARCHAR(100) NOT NULL,
    Title NVARCHAR(255) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    StockQuantity INT NULL,
    ProductImage NVARCHAR(MAX) NULL,
    ProductImages1 NVARCHAR(500) NULL,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

-- 4. Product Images Table (Handles multiple gallery thumbnail images per product)
CREATE TABLE ProductImages (
    ImageID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT NOT NULL,
    ImageURL NVARCHAR(MAX) NOT NULL,
    IsThumbnail BIT DEFAULT 0, -- 1 for main showcase image, 0 for extra thumbnails
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE
);

-- 5. Product Specifications Table (For technical specs rows on product detail pages)
CREATE TABLE ProductSpecs (
    SpecKeyID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT NOT NULL,
    SpecLabel NVARCHAR(100) NOT NULL,
    SpecValue NVARCHAR(MAX) NOT NULL,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE
);

-- 6. Orders Table (Checkout and order history tracking)
CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NULL,
    Subtotal DECIMAL(18,2) NOT NULL,
    OrderNote NVARCHAR(MAX),
    OrderStatus NVARCHAR(50) DEFAULT N'Đã xác nhận',
    CreatedAt DATETIME DEFAULT GETDATE(),
FOREIGN KEY (UserID) REFERENCES Users(UserID)
);
    CREATE TABLE OrderDetails (
    OrderDetailID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);