# ITExpert

Тестовое задание IT-Expert

# Структура проекта

Проект написан на .net 6.0 с использованием пакета Serilog для логгирования и EntityFrameworkCore для создания баз и таблиц. База создана на MsSql.

Решение состоит из 3 проектов:
- *Api* - запускаемый проект, содержащий основные настройки для запуска, файл конфигурации, обработчик middlware и контроллеры
- *Data* - бибилиотека классов, в которой хранятся модели сущностей, запросов и ответов, контекст базы данных и миграции
- *Services* - бибилиотека классов, в которой хранятся интерфейсы и их реализация с основной логикой

# Тестовые задания

## Тестовое задание 1

За сохранение данных в базу отвечает класс [LoggingMiddleware](https://github.com/insaniaOfficialis/ITExpert/blob/main/Api/Middleware/LoggingMiddleware.cs).

### Часть 1

За логику данной задачи отвечает [CodeController](https://github.com/insaniaOfficialis/ITExpert/blob/main/Api/Controllers/CodeController.cs) как принимающий и интерфейс с реализацией [AddCode](https://github.com/insaniaOfficialis/ITExpert/tree/main/Services/Codes/AddCode) как основная логика.


Структура таблицы представлена на скриншоте:
![image](https://github.com/insaniaOfficialis/ITExpert/assets/94796519/8d8153c3-bf3c-4b0d-b742-ddcaf2242461)

Также с ней можно ознакомиться по сущности [Codes](https://github.com/insaniaOfficialis/ITExpert/blob/main/Data/Entites/Codes.cs).

#### Результаты выполнения:

- Postman 1:

![image](https://github.com/insaniaOfficialis/ITExpert/assets/94796519/25e838cd-fecf-4e47-a542-628c2090e45e)
- БД 1:

![image](https://github.com/insaniaOfficialis/ITExpert/assets/94796519/a7092276-351c-4587-8676-1078a5b1460d)
- Postman 2:

![image](https://github.com/insaniaOfficialis/ITExpert/assets/94796519/5b19b28a-baa6-4418-b505-65fa9c5241b8)
- БД 2:

![image](https://github.com/insaniaOfficialis/ITExpert/assets/94796519/71e0acf1-c00c-4bce-96b1-6320fcb536bd)

### Часть 2

За логику данной задачи отвечает [CodeController](https://github.com/insaniaOfficialis/ITExpert/blob/main/Api/Controllers/CodeController.cs) как принимающий и интерфейс с реализацией [GetCode](https://github.com/insaniaOfficialis/ITExpert/tree/main/Services/Codes/GetCode) как логика c использованием linq или [GetCodeBD](https://github.com/insaniaOfficialis/ITExpert/tree/main/Services/Codes/GetCodeBD) как логика c использованием хранимых процедур.

Сама хранимая процедура:
``` sql
CREATE PROCEDURE dbo.get_codes (@json_in NVARCHAR(MAX))
AS
BEGIN

  BEGIN TRY
    DECLARE @sortKey NVARCHAR(MAX)
           ,@isAscending BIT
           ,@skip INT
           ,@take INT
           ,@orderNumber INT
           ,@code INT
           ,@value NVARCHAR(MAX)
           ;
    
    SELECT @sortKey = j.SortKey
          ,@isAscending = j.IsAscending
          ,@skip = j.[Skip]
          ,@take = j.[Take]
          ,@orderNumber = j.OrderNumber
          ,@code = j.Code
          ,@value = j.[Value]
    FROM OPENJSON(@json_in, '$')
    WITH (
            SortKey NVARCHAR(MAX) '$.Sort.SortKey'
           ,IsAscending BIT '$.Sort.IsAscending'
           ,[Skip] INT
           ,[Take] INT
           ,OrderNumber INT
           ,Code INT
           ,[Value] NVARCHAR(MAX)
          ) j;

    DECLARE @json_out NVARCHAR(MAX) = (
                                       SELECT c.order_number 'orderNumber'
                                             ,c.code
                                             ,c.[value]
                                       FROM Codes c
                                       WHERE (@orderNumber IS NULL OR c.order_number = @orderNumber)
                                       AND (@code IS NULL OR c.code = @code)
                                       AND (@value IS NULL OR c.[value] LIKE CONCAT('%', @value, '%'))
                                       ORDER BY CASE WHEN @sortKey = 'orderNumber' AND @isAscending = 1 THEN c.order_number END
                                               ,CASE WHEN @sortKey = 'code' AND @isAscending = 1 THEN code END
                                               ,CASE WHEN @sortKey = 'value' AND @isAscending = 1 THEN [value] END
                                               ,CASE WHEN @sortKey = 'orderNumber' AND @isAscending = 0 THEN order_number END DESC
                                               ,CASE WHEN @sortKey = 'code' AND @isAscending = 0 THEN code END DESC
                                               ,CASE WHEN @sortKey = 'value' AND @isAscending = 0 THEN [value] END DESC
                                       OFFSET ISNULL(@skip, 0) ROWS FETCH NEXT ISNULL(@take, 20) ROWS ONLY
                                       FOR JSON PATH
                                      );

    SELECT @json_out;
    
  END TRY
  BEGIN CATCH
    IF @@trancount > 0
		  ROLLBACK;
		THROW;
  END CATCH;

END;
GO
```


#### Результаты выполнения:

- Postman 1:

![image](https://github.com/insaniaOfficialis/ITExpert/assets/94796519/7e86d337-2f53-4453-bee0-5d8838b86e83)
- Postman 2:

![image](https://github.com/insaniaOfficialis/ITExpert/assets/94796519/5126b669-9d4e-4df4-bc77-fde045a902f3)

## Тестовое задание 2

За логику данной задачи отвечает [ClientController](https://github.com/insaniaOfficialis/ITExpert/blob/main/Api/Controllers/ClientController.cs) как принимающий и интерфейс с реализацией [GetClient](https://github.com/insaniaOfficialis/ITExpert/tree/main/Services/Clients/GetClient) как логика c использованием linq или [GetClientsBD](https://github.com/insaniaOfficialis/ITExpert/blob/main/Services/Clients/GetClientBD/GetClientsBD.cs) как логика c использованием хранимых процедур.

Сама хранимая процедура:

``` sql
CREATE PROCEDURE dbo.get_clients (@json_in NVARCHAR(MAX))
AS
BEGIN

  BEGIN TRY
    DECLARE @count INT
           ;
    
    SELECT @count = j.[Count]
    FROM OPENJSON(@json_in, '$')
    WITH (
            [Count] INT
          ) j;

    DECLARE @json_out NVARCHAR(MAX) = (
                                       SELECT c.ClientName
                                             ,COUNT(cc.Id) 'CountContacts'
                                       FROM Clients c
                                       LEFT JOIN ClientContacts cc ON c.Id = cc.ClientId
                                       GROUP BY c.ClientName
                                       HAVING COUNT(cc.Id) > ISNULL(@count, 0)
                                       FOR JSON PATH
                                      );

    SELECT @json_out;
    
  END TRY
  BEGIN CATCH
    IF @@trancount > 0
		  ROLLBACK;
		THROW;
  END CATCH;

END;
GO
```

#### Результаты выполнения:

- Postman 1:

![image](https://github.com/insaniaOfficialis/ITExpert/assets/94796519/029ea6e4-6ae6-4154-882d-217850e0884a)
- Postman 2:

![image](https://github.com/insaniaOfficialis/ITExpert/assets/94796519/eb241e2c-4291-4cca-bc62-0eb121e7099e)
- Postman 3:

![image](https://github.com/insaniaOfficialis/ITExpert/assets/94796519/b768b200-e790-41f3-8d41-02eb31ced1fa)
- Postman 4:

![image](https://github.com/insaniaOfficialis/ITExpert/assets/94796519/e68ae31c-461e-4975-8506-479d13179cb2)

## Тестовое задание 3

За логику данной задачи отвечает табличная функция, представленная ниже:

``` sql
CREATE FUNCTION dbo.get_dates ()
RETURNS @table TABLE (Id BIGINT, Sd DATE, Ed DATE)
AS
BEGIN

  INSERT @table
  SELECT dt.Id
        ,dt.Dt 'Sd'
        ,di.Dt 'Ed'
  FROM (
          SELECT dt.Id, dt.Dt, ROW_NUMBER() OVER(PARTITION BY dt.Id ORDER BY dt.Dt) 'Number' 
          FROM Dates dt
       ) dt
  JOIN (
          SELECT dt.Id, dt.Dt, ROW_NUMBER() OVER(PARTITION BY dt.Id ORDER BY dt.Dt) 'Number' 
          FROM Dates dt
       ) di ON di.Id = dt.Id
  WHERE di.Number = dt.Number + 1
  ORDER BY dt.Id, dt.Dt, di.Dt

  RETURN;

END;
GO
```

#### Результаты выполнения:

- BD 1:

![image](https://github.com/insaniaOfficialis/ITExpert/assets/94796519/36171d13-1c9d-46c1-b419-16ceb859cab3)
- BD 2:

![image](https://github.com/insaniaOfficialis/ITExpert/assets/94796519/86885501-42f7-40f2-90c6-380cfcce35d6)

# Дополнения:

- За вызов хранимых процедур отвечает интерфейс с реализацией [BaseExecute](https://github.com/insaniaOfficialis/ITExpert/tree/main/Services/BD/Procedures/BaseExecute)
- За накатывание миграций, внесение первоначальных данных и создание процедур с функциями отвечает интерфейс с реализацией [Initialization](https://github.com/insaniaOfficialis/ITExpert/tree/main/Services/Initialization)
- Прикладываю также выгруженную [postman коллекцию](https://github.com/insaniaOfficialis/ITExpert/tree/main/Usefull)
