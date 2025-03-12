INSERT INTO ImageTypes ( ImageTypeName, CreatedAt, UpdatedAt, Status)
values ('Image', GETDATE(),GETDATE(),1)

INSERT INTO SessionTypes (SessionTypeName , CreatedAt, UpdatedAt, Status)
values ('MovilApp',GETDATE(),GETDATE(),1)

INSERT INTO Roles (RoleName,CreatedAt,UpdatedAt,Status)
VALUES ('SuperAdmin',GETDATE(),GETDATE(),1),
		('Admin',GETDATE(),GETDATE(),1),
		('User',GETDATE(),GETDATE(),1)

 
INSERT INTO Priorities (Name,CreatedAt,UpdatedAt,Status)
VALUES ('ALTA',GETDATE(),GETDATE(),1),
('MEDIA',GETDATE(),GETDATE(),1),
('BAJA',GETDATE(),GETDATE(),1)


INSERT INTO StatusTasks (Name,CreatedAt,UpdatedAt,Status)
VALUES ('Pendiente',GETDATE(),GETDATE(),1),('En Progreso',GETDATE(),GETDATE(),1),('Completado',GETDATE(),GETDATE(),1)



INSERT INTO dbo.Companys (
    CompanyFiscalName, 
    CompanyComercialName, 
    Email, 
    CompanyAddress, 
    IdCart, 
    CompanyPhone, 
    CreatedAt, 
    UpdatedAt, 
    Status
) VALUES 
(
    'Tech Solutions S.A.', 
    'Tech Solutions', 
    'contacto@techsolutions.com', 
    'Av. Tecnológica #123, Ciudad', 
    '123456789012345', 
    '555-1234', 
    GETDATE(), 
    GETDATE(), 
    1
),
(
    'AutoFix S.A.', 
    'AutoFix', 
    'info@autofix.com', 
    'Calle Mecánica #456, Ciudad', 
    '987654321098765', 
    '555-5678', 
    GETDATE(), 
    GETDATE(), 
    1
),
(
    'Clean Car S.A.', 
    'Clean Car', 
    'soporte@cleancar.com', 
    'Boulevard Lavado #789, Ciudad', 
    '567890123456789', 
    '555-6789', 
    GETDATE(), 
    GETDATE(), 
    1
);


INSERT INTO dbo.CompanyEmployees (
    NameEmployee, 
    FirstSurname, 
    SecondSurname, 
    IdCompany, 
    CreatedAt, 
    UpdatedAt, 
    Status
) VALUES 
(
    'Carlos', 
    'Gómez', 
    'López', 
    1,  -- Tech Solutions S.A.
    GETDATE(), 
    GETDATE(), 
    1
),
(
    'María', 
    'Fernández', 
    'Ruiz', 
    1,  -- Tech Solutions S.A.
    GETDATE(), 
    GETDATE(), 
    1
),
(
    'José', 
    'Martínez', 
    'Pérez', 
    2,  -- AutoFix S.A.
    GETDATE(), 
    GETDATE(), 
    1
),
(
    'Ana', 
    'Rodríguez', 
    'García', 
    2,  -- AutoFix S.A.
    GETDATE(), 
    GETDATE(), 
    1
),
(
    'Luis', 
    'Hernández', 
    'Soto', 
    3,  -- Clean Car S.A.
    GETDATE(), 
    GETDATE(), 
    1
),
(
    'Elena', 
    'Mendoza', 
    'Jiménez', 
    3,  -- Clean Car S.A.
    GETDATE(), 
    GETDATE(), 
    1
);
