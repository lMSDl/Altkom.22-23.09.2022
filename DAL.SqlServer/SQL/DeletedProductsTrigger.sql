﻿CREATE OR ALTER TRIGGER PRODUCT_Delete On Product
    AFTER DELETE
    AS
    BEGIN
        SET NOCOUNT ON;

        UPDATE dbo.[Order] SET DeletedProducts += 1
        WHERE Id IN (SELECT OrdersId FROM dbo.OrderProduct WHERE ProductsId IN (SELECT DELETED.Id FROM DELETED))

    END