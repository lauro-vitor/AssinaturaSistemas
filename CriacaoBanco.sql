USE master
GO

IF  DB_ID('AssinaturaSistema') IS NULL
BEGIN
	CREATE DATABASE AssinaturaSistema
END
GO

USE [AssinaturaSistema]
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'Pais')
BEGIN
	CREATE TABLE [Pais](
		IdPais INT NOT NULL IDENTITY(1,1),
		NomePais VARCHAR(255),
		PRIMARY KEY (IdPais)
	);
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'Estado')
BEGIN
	BEGIN
		CREATE TABLE [Estado](
		IdEstado INT NOT NULL IDENTITY(1,1),
		IdPais INT NOT NULL,
		NomeEstado VARCHAR(255)
		PRIMARY KEY (IdEstado)
	  );
	END

	BEGIN
		ALTER TABLE [Estado]
		WITH CHECK ADD CONSTRAINT  [FK_Estado_Pais]
		FOREIGN KEY([IdPais]) REFERENCES [Pais]([IdPais]);

		ALTER TABLE [Estado]
		CHECK CONSTRAINT [FK_Estado_Pais];
	END
	
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'Cliente')
BEGIN
	BEGIN
		CREATE TABLE [Cliente](
			IdCliente INT NOT NULL IDENTITY(1,1),
			IdEstado INT NOT NULL,
			NomeEmpresa VARCHAR(255) NOT NULL,
			Telefone VARCHAR(50),
			Observacao VARCHAR(MAX),
			Endereco VARCHAR(255),
			CodigoPostal VARCHAR(20),
			DataCadastro DATETIME NOT NULL,
			DataAtualizacao DATETIME,
			Ativo BIT NOT NULL,
			PRIMARY KEY(IdCliente)
		);
			
	END

	BEGIN
		ALTER TABLE [Cliente]
		WITH CHECK ADD CONSTRAINT [FK_Cliente_Estado]
		FOREIGN KEY([IdEstado])
		REFERENCES [Estado]([IdEstado]);

		ALTER TABLE [Cliente]
		CHECK CONSTRAINT [FK_Cliente_Estado];
	END
	
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE '%Cliente%')
BEGIN
	
	CREATE TABLE [Cliente](
		IdCliente INT NOT NULL IDENTITY(1,1),
		IdEstado INT NOT NULL,
		NomeEmpresa VARCHAR(255),
		Observacao VARCHAR(MAX),
		Endereco VARCHAR(255),
		CodigoPostal VARCHAR(20),
		DataCadastro DATETIME,
		UltimaAtualizacao DATETIME,
		Ativo BIT NOT NULL
	);

	ALTER TABLE [Cliente]
	WITH CHECK ADD CONSTRAINT [FK_Cliente_Estado]
	FOREIGN KEY([IdEstado])
	REFERENCES [Estado](IdEstado);

	ALTER TABLE [Cliente] 
	CHECK CONSTRAINT [FK_Cliente_Estado]
	
END
GO

CREATE OR ALTER  VIEW  VwListaClientes  
AS  SELECT   
 C.IdCliente,  
 C.NomeEmpresa,  
 P.IdPais,  
 P.NomePais,  
 E.IdEstado,  
 E.NomeEstado,  
 C.CodigoPostal,  
 C.Endereco,  
 C.DataCadastro,  
 C.UltimaAtualizacao,  
 C.Ativo,
 C.Observacao
FROM Cliente C  
INNER JOIN Estado E ON E.IdEstado = C.IdEstado  
INNER JOIN Pais P ON P.IdPais = E.IdPais  
GO