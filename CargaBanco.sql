IF NOT EXISTS(SELECT * FROM Pais)
BEGIN
	SET IDENTITY_INSERT Pais ON;

	INSERT INTO Pais (IdPais, NomePais)
	VALUES (1, 'Brasil');

	SET IDENTITY_INSERT Pais OFF;
END
GO

IF NOT EXISTS(SELECT * FROM Estado)
BEGIN
	SET IDENTITY_INSERT Estado ON;

	INSERT INTO Estado(IdEstado, IdPais, NomeEstado)
	VALUES (1,1,'ESPIRITO SANTO');

	INSERT INTO Estado(IdEstado, IdPais, NomeEstado)
	VALUES (2,1,'SAO PAULO');

	INSERT INTO Estado(IdEstado, IdPais, NomeEstado)
	VALUES (3,1,'MINAS GERAIS');

	INSERT INTO Estado(IdEstado, IdPais, NomeEstado)
	VALUES (4,1,'RIO DE JANEIRO');

	INSERT INTO Estado(IdEstado, IdPais, NomeEstado)
	VALUES (5,1,'RIO GRANDE DO SUL');

	INSERT INTO Estado(IdEstado, IdPais, NomeEstado)
	VALUES (6,1,'CURITIBA');

	SET IDENTITY_INSERT Estado ON;
END
GO

IF NOT EXISTS(SELECT * FROM [Usuario])
BEGIN
	INSERT INTO [dbo].[Usuario]
           ([NomeCompleto]
           ,[Email]
           ,[Senha]
           ,[Desabilitado])
     VALUES
           ('system'
           ,'system'
           ,'admin'
           ,0)
END
GO
