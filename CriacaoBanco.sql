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


IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'Usuario')
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
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'TipoSistema')
BEGIN
	CREATE TABLE TipoSistema(
		IdTipoSistema INT NOT NULL IDENTITY(1,1),
		Descricao VARCHAR(255),
		PRIMARY KEY(IdTipoSistema)
	)
END
GO

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
GO
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
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'PeriodoCobranca')
BEGIN
	CREATE TABLE PeriodoCobranca(
		IdPeriodoCobranca  INT NOT NULL,
		Descricao VARCHAR(255),
		PRIMARY KEY(IdPeriodoCobranca)
	);
END
GO



--SERVICO FINANCEIRO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'ServicoFinanceiro')
BEGIN
	CREATE TABLE ServicoFinanceiro(
		IdServicoFinanceiro INT NOT NULL IDENTITY(1,1),
		IdContaBancaria INT NOT NULL,
		IdPeriodoCobranca INT NOT NULL,
		DescricaoServico VARCHAR(255) NOT NULL,
		DiaVencimento INT NOT NULL,
		ValorCobranca DECIMAL(10,2) NOT NULL,
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
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'StatusParcela')
BEGIN
	CREATE TABLE StatusParcela(
		IdStatusParcela INT NOT NULL,
		Descricao VARCHAR(255),
		PRIMARY KEY(IdStatusParcela)
	);
END
GO


IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'Parcela')
BEGIN
	CREATE TABLE Parcela(
		IdParcela INT NOT NULL IDENTITY(1,1),
		IdSistema INT NOT NULL,
		IdServicoFinanceiro INT NOT NULL,
		IdStatusParcela INT NOT NULL,
		Numero INT NOT NULL,
		DataGeracao DATETIME NOT NULL,
		DataVencimento DATETIME NOT NULL,
		DataCancelamento DATETIME NULL,
		Valor DECIMAL(10,2) NOT NULL,
		Desconto DECIMAL(10,2) NULL,
		Acrescimo DECIMAL(10,2) NULL,
		Observacao VARCHAR(MAX),
		PRIMARY KEY (IdParcela),
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
GO


CREATE OR ALTER VIEW VwServicoFinanceiro AS
	SELECT [IdServicoFinanceiro]
		  ,[PeriodoCobrancaDescricao] = PC.Descricao
		  ,[ContaBancariaDescricao] = CONCAT('Conta:', CON.NumeroConta,' Ag:',CON.NumeroAgencia)
		  ,[DescricaoServico]
		  ,[DiaVencimento]
		  ,[ValorCobranca]
		  ,[QuantidadeParcelas]
	  FROM [dbo].[ServicoFinanceiro] SF
	  INNER JOIN ContaBancaria CON ON SF.IdContaBancaria = CON.IdContaBancaria
	  INNER JOIN PeriodoCobranca PC ON PC.IdPeriodoCobranca =  SF.IdPeriodoCobranca
GO

CREATE OR ALTER VIEW VwSistema
AS
SELECT 
		S.*,
		[ClienteNomeEmpresa] = C.NomeEmpresa,
		[DescricaoTipoSistema] = TS.Descricao
FROM Sistema  S
INNER JOIN Cliente C ON C.IdCliente =  S.IdCliente
INNER JOIN TipoSistema TS ON TS.IdTipoSistema = S.IdTipoSistema
GO





IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'PagamentoParcela')
BEGIN
	CREATE TABLE PagamentoParcela(
		IdPagamentoParcela INT NOT NULL IDENTITY(1,1),
		IdParcela INT NOT NULL,
		DataPagamento DATETIME NOT NULL,
		ValorDepositoBancario DECIMAL(10,2) NOT NULL DEFAULT 0,
		ValorCartaoCredito DECIMAL (10,2) NOT NULL DEFAULT 0,
		ValorCartaoDebito DECIMAL (10,2) NOT NULL DEFAULT 0
	);
	ALTER TABLE PagamentoParcela
	WITH CHECK ADD CONSTRAINT FK_PagamentoParcela_Parcela
	FOREIGN KEY([IdParcela])
	REFERENCES [dbo].[Parcela](IdParcela)
END
GO



CREATE OR ALTER    VIEW [dbo].[VwParcela]  
AS  
SELECT P.[IdParcela]  
      ,P.[IdSistema]  
      ,P.[IdServicoFinanceiro]  
	  ,SF.[DescricaoServico]  
      ,P.[IdStatusParcela]  
	  ,[StatusParcelaDescricao] = SP.Descricao  
      ,P.[Numero]  
      ,[DataGeracao] = FORMAT([DataGeracao],'dd/MM/yyyy')  
      ,[DataVencimento] = FORMAT([DataVencimento],'dd/MM/yyyy')  
      ,[DataCancelamento] = FORMAT([DataCancelamento],'dd/MM/yyyy')  
      ,[ValorPagar] = (([Valor] + [Acrescimo]) - [Desconto])  
      ,P.[Desconto]  
      ,P.[Acrescimo]  
      ,P.[Observacao]
	  ,[ValorPago] = (ISNULL(PP.ValorDepositoBancario, 0) + ISNULL(PP.ValorCartaoCredito, 0) + ISNULL(PP.ValorCartaoDebito, 0))
  FROM [dbo].[Parcela] P  
  INNER JOIN ServicoFinanceiro SF ON SF.IdServicoFinanceiro = P.IdServicoFinanceiro  
  INNER JOIN StatusParcela SP ON SP.IdStatusParcela = P.IdStatusParcela  
  LEFT JOIN PagamentoParcela PP ON PP.IdParcela = P.IdParcela
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'ServicoFinanceiro' AND COLUMN_NAME LIKE 'StripePriceId')
BEGIN
	ALTER TABLE ServicoFinanceiro
	ADD StripePriceId VARCHAR(255) NULL
END
GO


IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'ServicoFinanceiro' AND COLUMN_NAME LIKE 'StripeOrdem')
BEGIN
	ALTER TABLE ServicoFinanceiro
	ADD StripeOrdem INT NULL
END
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'ServicoFinanceiro' AND COLUMN_NAME LIKE 'IdTipoSistema')
BEGIN
	ALTER TABLE ServicoFinanceiro
	ADD IdTipoSistema INT NULL;

	ALTER TABLE ServicoFinanceiro 
	WITH CHECK ADD CONSTRAINT ServicoFinanceiro_TipoSistema
	FOREIGN KEY([IdTipoSistema])
	REFERENCES[dbo].TipoSistema(IdTipoSistema)

	ALTER TABLE ServicoFinanceiro
	CHECK CONSTRAINT ServicoFinanceiro_TipoSistema
END
GO


IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'Pais' AND COLUMN_NAME LIKE 'Padrao')
BEGIN
	ALTER TABLE Pais
	ADD Padrao BIT NOT NULL DEFAULT 0
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'Estado' AND COLUMN_NAME LIKE 'Padrao')
BEGIN
	ALTER TABLE Estado
	ADD Padrao BIT NOT NULL DEFAULT 0
END
GO


IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME LIKE 'PagamentoParcela' AND COLUMN_NAME LIKE 'StripePaymentIntentId')
BEGIN
	ALTER TABLE PagamentoParcela
	ADD StripePaymentIntentId VARCHAR(100)
END
GO