IF DB_ID('LeveTaskSystemDb') IS NULL
BEGIN
    CREATE DATABASE LeveTaskSystemDb;
END
GO

USE LeveTaskSystemDb;
GO

IF OBJECT_ID('dbo.Tasks', 'U') IS NOT NULL DROP TABLE dbo.Tasks;
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL DROP TABLE dbo.Users;
GO

CREATE TABLE dbo.Users
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    FullName NVARCHAR(150) NOT NULL,
    BirthDate DATE NOT NULL,
    LandlinePhone NVARCHAR(20) NOT NULL,
    MobilePhone NVARCHAR(20) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    Address NVARCHAR(200) NOT NULL,
    PhotoPath NVARCHAR(300) NOT NULL,
    PasswordHash NVARCHAR(300) NOT NULL,
    Role INT NOT NULL,
    ManagerId UNIQUEIDENTIFIER NULL,
    CONSTRAINT FK_Users_Manager FOREIGN KEY (ManagerId) REFERENCES dbo.Users(Id)
);
GO

CREATE TABLE dbo.Tasks
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Message NVARCHAR(500) NOT NULL,
    DueDate DATE NOT NULL,
    Status INT NOT NULL,
    AssignedToUserId UNIQUEIDENTIFIER NOT NULL,
    CreatedByManagerId UNIQUEIDENTIFIER NOT NULL,
    CreatedAt DATETIME2 NOT NULL,
    CompletedAt DATETIME2 NULL,
    CONSTRAINT FK_Tasks_AssignedTo FOREIGN KEY (AssignedToUserId) REFERENCES dbo.Users(Id),
    CONSTRAINT FK_Tasks_Manager FOREIGN KEY (CreatedByManagerId) REFERENCES dbo.Users(Id)
);
GO

INSERT INTO dbo.Users
(
    Id, FullName, BirthDate, LandlinePhone, MobilePhone, Email, Address, PhotoPath, PasswordHash, Role, ManagerId
)
VALUES
(
    '8AD687A0-C30E-4198-BFDB-4F23F57BE320',
    'Usuario Gestor Inicial',
    '1990-01-01',
    '0000000000',
    '00000000000',
    'ti@leveinvestimentos.com.br',
    'Endereco padrao',
    '',
    '100000.zjL3MhmhY2X8FcCYjNse5Q==.9R/7dgljXliM6h7D4QJI43nQ4Gx55SyJ8ygffM0j9Q0=',
    1,
    NULL
);
GO
