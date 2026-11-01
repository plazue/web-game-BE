ALTER TABLE Products ADD SalePrice DECIMAL(18, 2) NULL;
ALTER TABLE Products ADD Color NVARCHAR(50) NULL;
ALTER TABLE Products ADD ProductType NVARCHAR(100) NULL; 
ALTER TABLE Orders
ADD PaymentType NVARCHAR(50) DEFAULT ('CASH') NULL;
ALTER TABLE Orders
ADD 
    CreditCard      NVARCHAR(100) NULL,
    CardHolderName NVARCHAR(100) NULL,      -- Tên chủ thẻ
    CardNumberLast4 NVARCHAR(4) NULL,       -- 4 số cuối của thẻ (đảm bảo bảo mật)
    CardExpiryDate NVARCHAR(10) NULL,       -- Ngày hết hạn thẻ (MM/YY)
    CardBrand NVARCHAR(50) NULL;       
    ALTER TABLE Orders
ADD CreditCard NVARCHAR(max) NULL;-- Loại thẻ (Visa, MasterCard, JCB,...)
