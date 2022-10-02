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


IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'Cliente')
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
		Ativo BIT NOT NULL,
		PRIMARY KEY(IdCliente)
	);

	ALTER TABLE [Cliente]
	WITH CHECK ADD CONSTRAINT [FK_Cliente_Estado]
	FOREIGN KEY([IdEstado])
	REFERENCES [Estado](IdEstado);

	ALTER TABLE [Cliente] 
	CHECK CONSTRAINT [FK_Cliente_Estado]
	
END
GO


IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'Contato')
BEGIN

	CREATE TABLE Contato(
		IdContato INT NOT NULL IDENTITY(1,1),
		IdCliente INT NOT NULL,
		NomeCompleto VARCHAR(255),
		Email VARCHAR(255),
		Celular VARCHAR(50),
		Telefone VARCHAR(50),
		Senha VARCHAR(255)
		PRIMARY KEY(IdContato)
	);

	ALTER TABLE Contato 
	WITH CHECK ADD CONSTRAINT FK_Contato_Cliente
	FOREIGN KEY (IdCliente) 
	REFERENCES [Cliente](IdCliente);

	ALTER TABLE Contato
	CHECK CONSTRAINT FK_Contato_Cliente;
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


IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME LIKE 'Usuario')
BEGIN
	CREATE TABLE Usuario(
		IdUsuario INT NOT NULL IDENTITY(1,1),
		NomeCompleto VARCHAR(255),
		Email VARCHAR(255) NOT NULL,
		Senha VARCHAR(255),
		Desabilitado BIT NOT NULL DEFAULT 0,
		PRIMARY KEY(IdUsuario)
	)
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'TipoSistema')
BEGIN
	CREATE TABLE TipoSistema(
		IdTipoSistema INT NOT NULL IDENTITY(1,1),
		Descricao VARCHAR(255),
		PRIMARY KEY(IdTipoSistema)
	)
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'Sistema')
BEGIN
	
		CREATE TABLE Sistema(
		IdSistema INT NOT NULL IDENTITY(1,1),
		IdCliente INT NOT NULL,
		IdTipoSistema INT NOT NULL,
		DominioProvisorio VARCHAR(255),
		Dominio VARCHAR(255),
		Pasta VARCHAR(255),
		BancoDeDados VARCHAR(255),
		Ativo BIT NOT NULL,
		DataInicio DATETIME NOT NULL,
		DataCancelamento DATETIME,
		PRIMARY KEY(IdSistema)
		);

	ALTER TABLE Sistema
	WITH CHECK ADD CONSTRAINT FK_Sistema_Cliente
	FOREIGN KEY(IdCliente) 
	REFERENCES [dbo].Cliente([IdCliente])

	ALTER TABLE Sistema
	CHECK CONSTRAINT FK_Sistema_Cliente

	ALTER TABLE Sistema
	WITH CHECK ADD CONSTRAINT FK_Sistema_TipoSistema
	FOREIGN KEY (IdTipoSistema)
	REFERENCES [dbo].TipoSistema([IdTipoSistema])

	ALTER TABLE Sistema
	CHECK CONSTRAINT FK_Sistema_TipoSistema
END

--financeiro

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'ContaBancaria')
BEGIN
	CREATE TABLE ContaBancaria(
		IdContaBancaria INT NOT NULL IDENTITY(1,1),
	    NumeroAgencia VARCHAR(50) NOT NULL,
		NumeroConta VARCHAR(50) NOT NULL,
		NumeroBanco INT NOT NULL,
		NomeBanco VARCHAR(100) NOT NULL,
		Cnpj VARCHAR(50),
		PRIMARY KEY(IdContaBancaria)
	);
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'PeriodoCobranca')
BEGIN
	CREATE TABLE PeriodoCobranca(
		IdPeriodoCobranca  INT NOT NULL,
		Descricao VARCHAR(255),
		PRIMARY KEY(IdPeriodoCobranca)
	);
END

IF NOT EXISTS(SELECT * FROM [PeriodoCobranca])
BEGIN
	INSERT INTO [PeriodoCobranca](IdPeriodoCobranca, Descricao)
	VALUES 
		(1,'MENSAL'),
		(2,'SEMESTRAL'),
		(3,'ANUAL');
END


--SERVICO FINANCEIRO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'ServicoFinanceiro')
BEGIN
	CREATE TABLE ServicoFinanceiro(
		IdServicoFinanceiro INT NOT NULL IDENTITY(1,1),
		IdContaBancaria INT NOT NULL,
		IdPeriodoCobranca INT NOT NULL,
		DescricaoServico VARCHAR(255) NOT NULL,
		DiaVencimento INT NOT NULL,
		ValorCobranca DECIMAL NOT NULL,
		QuantidadeParcelas INT NOT NULL
		PRIMARY KEY(IdServicoFinanceiro)
	);

	ALTER TABLE ServicoFinanceiro
	WITH CHECK ADD CONSTRAINT FK_ServicoFinanceiro_ContaBancaria
	FOREIGN KEY(IdContaBancaria)
	REFERENCES [dbo].ContaBancaria(IdContaBancaria)

	ALTER TABLE ServicoFinanceiro
	CHECK CONSTRAINT FK_ServicoFinanceiro_ContaBancaria

	ALTER TABLE ServicoFinanceiro
	WITH CHECK ADD CONSTRAINT FK_ServicoFinanceiro_PeriodoCobranca
	FOREIGN KEY (IdPeriodoCobranca)
	REFERENCES [dbo].[PeriodoCobranca](IdPeriodoCobranca)

	ALTER TABLE ServicoFinanceiro
	CHECK CONSTRAINT FK_ServicoFinanceiro_PeriodoCobranca

END


IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'StatusParcela')
BEGIN
	CREATE TABLE StatusParcela(
		IdStatusParcela INT NOT NULL,
		Descricao VARCHAR(255),
		PRIMARY KEY(IdStatusParcela)
	);
END


IF NOT EXISTS (SELECT * FROM [StatusParcela])
BEGIN
	INSERT INTO StatusParcela
	VALUES 
		(1,'ABERTO'),
		(2,'PAGO'),
		(3,'GRATUITO'),
		(4,'CANCELADO');
END


IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'Parcela')
BEGIN
	CREATE TABLE Parcela(
		IdParcela INT NOT NULL IDENTITY(1,1),
		IdSistema INT NOT NULL,
		IdServicoFinanceiro INT NOT NULL,
		IdStatusParcela INT NOT NULL,
		DataGeracao DATETIME NOT NULL,
		DataVencimento DATETIME NOT NULL,
		DataCancelamento DATETIME NULL,
		Valor DECIMAL NOT NULL,
		Desconto DECIMAL NULL,
		Acrescimo DECIMAL NULL,
		Observacao VARCHAR(MAX)
	);

	ALTER TABLE Parcela
	WITH CHECK ADD CONSTRAINT FK_Parcela_Sistema
	FOREIGN KEY(IdSistema)
	REFERENCES [dbo].[Sistema](IdSistema);
	
	ALTER TABLE Parcela
	CHECK CONSTRAINT FK_Parcela_Sistema;

	ALTER TABLE Parcela
	WITH CHECK ADD CONSTRAINT FK_Parcela_ServicoFinanceiro
	FOREIGN KEY (IdServicoFinanceiro)
	REFERENCES [dbo].[ServicoFinanceiro](IdServicoFinanceiro)

	ALTER TABLE Parcela
	CHECK CONSTRAINT FK_Parcela_ServicoFinanceiro;

	ALTER TABLE Parcela
    WITH CHECK ADD CONSTRAINT FK_Parcela_StatusParcela
	FOREIGN KEY (IdStatusParcela)
	REFERENCES [dbo].[StatusParcela](IdStatusParcela);

	ALTER TABLE Parcela
	CHECK CONSTRAINT FK_Parcela_StatusParcela;

END



