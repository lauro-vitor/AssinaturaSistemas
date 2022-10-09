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

IF NOT EXISTS(SELECT * FROM [TipoSistema])
BEGIN
	BEGIN TRAN
	SET IDENTITY_INSERT [TipoSistema] ON

		INSERT INTO [TipoSistema] (IdTipoSistema, Descricao)
		VALUES(1,'Fabricante'),
		(2,'Distribuidor'),
		(3,'Exportador')

	SET IDENTITY_INSERT [TipoSistema] OFF

	COMMIT TRAN
END
GO


IF NOT EXISTS (SELECT * FROM [StatusParcela])
BEGIN
	INSERT INTO StatusParcela
	VALUES 
		(1,'ABERTO'),
		(2,'PAGO'),
		(3,'GRATUITO'),
		(4,'CANCELADO');
END
GO

IF NOT EXISTS(SELECT * FROM [PeriodoCobranca])
BEGIN
	INSERT INTO [PeriodoCobranca](IdPeriodoCobranca, Descricao)
	VALUES 
		(1,'MENSAL'),
		(2,'SEMESTRAL'),
		(3,'ANUAL');
END
GO


BEGIN TRAN
IF NOT EXISTS(SELECT * FROM ServicoFinanceiro WHERE [StripePriceId] IS NOT NULL)
BEGIN
	
	DECLARE @mensal  INT  =1;
	DECLARE @semestral INT  = 2;
	DECLARE @anual  INT  =3;
	DECLARE @DiaVencimento INT = 15;
	DECLARE @DescricaoServico VARCHAR(255) = '';
	DECLARE @ValorCobranca DECIMAL(10,2);
	DECLARE @QuantidadeParcelas INT = 0;
	DECLARE @StripePriceId VARCHAR(255) = '';
	DECLARE @StripeOrdem INT = 0;
	DECLARE @TipoSistemaFabricante INT = 1;
	DECLARE @TipoSistemaDistribuidor INT  = 2;
	DECLARE @TipoSistemaExportador INT = 3;


	--FABRICANTE MENSAL
	SET @DescricaoServico = 'Fabricator Monthly';
	SET @ValorCobranca = 179.00;
	SET @QuantidadeParcelas = 12;
	SET @StripePriceId = 'price_1LqDDWLmrf2PWwIxm5Xvq5kD';
	SET @StripeOrdem = 1;

	INSERT INTO ServicoFinanceiro([IdContaBancaria],
		[IdPeriodoCobranca], 
		[DescricaoServico], 
		[DiaVencimento],
		[ValorCobranca], 
		[QuantidadeParcelas],
		[StripePriceId],
		[StripeOrdem],
		[IdTipoSistema])
	VALUES (1,@mensal,@DescricaoServico,@DiaVencimento,@ValorCobranca, @QuantidadeParcelas, @StripePriceId, @StripeOrdem, @TipoSistemaFabricante);


	--FABRICANTE ANUAL
	SET @DescricaoServico = 'Fabricator Semesterly';
	SET @ValorCobranca = 1020.00;
	SET @QuantidadeParcelas = 2;
	SET @StripePriceId = 'price_1LqDDWLmrf2PWwIx5yEqx09V';
	SET @StripeOrdem = 2;

   INSERT INTO ServicoFinanceiro([IdContaBancaria],
		[IdPeriodoCobranca], 
		[DescricaoServico], 
		[DiaVencimento],
		[ValorCobranca], 
		[QuantidadeParcelas],
		[StripePriceId],
		[StripeOrdem],
		[IdTipoSistema])
	VALUES (1,@semestral,@DescricaoServico,@DiaVencimento,@ValorCobranca, @QuantidadeParcelas, @StripePriceId, @StripeOrdem,@TipoSistemaFabricante);


	--FABRICANTE SEMESTRAL
	SET @DescricaoServico = 'Fabricator Yearly';
	SET @ValorCobranca = 1993.00;
	SET @QuantidadeParcelas = 1;
	SET @StripePriceId = 'price_1LqDDXLmrf2PWwIxRwVo7AIW';
	SET @StripeOrdem = 3;

   INSERT INTO ServicoFinanceiro([IdContaBancaria],
		[IdPeriodoCobranca], 
		[DescricaoServico], 
		[DiaVencimento],
		[ValorCobranca], 
		[QuantidadeParcelas],
		[StripePriceId],
		[StripeOrdem],
		[IdTipoSistema])
	VALUES (1,@anual,@DescricaoServico,@DiaVencimento,@ValorCobranca, @QuantidadeParcelas, @StripePriceId, @StripeOrdem, @TipoSistemaFabricante);



	-------------------------------#####################-----------------------------------------------

		--DISTRIBUTOR mensal
	SET @DescricaoServico = 'Distributor Montly';
	SET @ValorCobranca = 195.00;
	SET @QuantidadeParcelas = 12;
	SET @StripePriceId = 'price_1LqDDXLmrf2PWwIxl0gLJ1Y5';
	SET @StripeOrdem = 4;

   INSERT INTO ServicoFinanceiro([IdContaBancaria],
		[IdPeriodoCobranca], 
		[DescricaoServico], 
		[DiaVencimento],
		[ValorCobranca], 
		[QuantidadeParcelas],
		[StripePriceId],
		[StripeOrdem],
		[IdTipoSistema])
	VALUES (1,@mensal,@DescricaoServico,@DiaVencimento,@ValorCobranca, @QuantidadeParcelas, @StripePriceId, @StripeOrdem, @TipoSistemaDistribuidor);


	
		--DISTRIBUTOR SEMESTRAL
	SET @DescricaoServico = 'Distributor Semesterly';
	SET @ValorCobranca = 1048.00;
	SET @QuantidadeParcelas = 2;
	SET @StripePriceId = 'price_1LqDDYLmrf2PWwIxL1xG3O0A';
	SET @StripeOrdem = 5;

   INSERT INTO ServicoFinanceiro([IdContaBancaria],
		[IdPeriodoCobranca], 
		[DescricaoServico], 
		[DiaVencimento],
		[ValorCobranca], 
		[QuantidadeParcelas],
		[StripePriceId],
		[StripeOrdem],
		[IdTipoSistema])
	VALUES (1,@semestral,@DescricaoServico,@DiaVencimento,@ValorCobranca, @QuantidadeParcelas, @StripePriceId, @StripeOrdem, @TipoSistemaDistribuidor);
	

		--DISTRIBUTOR anual
	SET @DescricaoServico = 'Distributor Yearly';
	SET @ValorCobranca = 1999.00;
	SET @QuantidadeParcelas = 1;
	SET @StripePriceId = 'price_1LqDDZLmrf2PWwIx9DYWwu1q';
	SET @StripeOrdem = 6;

   INSERT INTO ServicoFinanceiro([IdContaBancaria],
		[IdPeriodoCobranca], 
		[DescricaoServico], 
		[DiaVencimento],
		[ValorCobranca], 
		[QuantidadeParcelas],
		[StripePriceId],
		[StripeOrdem],
		[IdTipoSistema])
	VALUES (1,@anual,@DescricaoServico,@DiaVencimento,@ValorCobranca, @QuantidadeParcelas, @StripePriceId, @StripeOrdem, @TipoSistemaDistribuidor);


	--------------------------- ######################## ----------------------------
	
		--exportador mensal
	SET @DescricaoServico = 'Exporter Monthly';
	SET @ValorCobranca = 199.00;
	SET @QuantidadeParcelas = 12;
	SET @StripePriceId = 'price_1LqDDZLmrf2PWwIxnNdCsgsW';
	SET @StripeOrdem = 7;

   INSERT INTO ServicoFinanceiro([IdContaBancaria],
	[IdPeriodoCobranca], 
		[DescricaoServico], 
		[DiaVencimento],
		[ValorCobranca], 
		[QuantidadeParcelas],
		[StripePriceId],
		[StripeOrdem],
		[IdTipoSistema])
	VALUES (1,@mensal,@DescricaoServico,@DiaVencimento,@ValorCobranca, @QuantidadeParcelas, @StripePriceId, @StripeOrdem, @TipoSistemaExportador);


			--exportador semestral
	SET @DescricaoServico = 'Exporter Semesterly';
	SET @ValorCobranca = 1134.00;
	SET @QuantidadeParcelas = 2;
	SET @StripePriceId = 'price_1LqDDaLmrf2PWwIx5yfxhQXb';
	SET @StripeOrdem = 8;

   INSERT INTO ServicoFinanceiro([IdContaBancaria],
		[IdPeriodoCobranca], 
		[DescricaoServico], 
		[DiaVencimento],
		[ValorCobranca], 
		[QuantidadeParcelas],
		[StripePriceId],
		[StripeOrdem],
		[IdTipoSistema])
	VALUES (1,@semestral,@DescricaoServico,@DiaVencimento,@ValorCobranca, @QuantidadeParcelas, @StripePriceId, @StripeOrdem, @TipoSistemaExportador);




	--exportador anual
	SET @DescricaoServico = 'Exporter Yearly';
	SET @ValorCobranca = 2148.00;
	SET @QuantidadeParcelas = 1;
	SET @StripePriceId = 'price_1LqDDaLmrf2PWwIxW6gmVsX0';
	SET @StripeOrdem = 9;

   INSERT INTO ServicoFinanceiro([IdContaBancaria],
		[IdPeriodoCobranca], 
		[DescricaoServico], 
		[DiaVencimento],
		[ValorCobranca], 
		[QuantidadeParcelas],
		[StripePriceId],
		[StripeOrdem],
		[IdTipoSistema])
	VALUES (1,@anual,@DescricaoServico,@DiaVencimento,@ValorCobranca, @QuantidadeParcelas, @StripePriceId, @StripeOrdem, @TipoSistemaExportador);


END

COMMIT TRAN
GO

BEGIN TRAN

	DECLARE @IdPaisInsert INT  = 0;

	IF NOT EXISTS((SELECT * FROM Pais WHERE [Padrao] = 1))
	BEGIN
		INSERT INTO Pais([NomePais],[Padrao]) VALUES('PADRAO',1);
		SET @IdPaisInsert = (SELECT @@IDENTITY AS [Last-Inserted Identity Value]);
	END

	IF NOT EXISTS( (SELECT * FROM Estado WHERE [Padrao]  = 1) )
	BEGIN
	 INSERT INTO Estado([IdPais],[NomeEstado],[Padrao])
	 VALUES(@IdPaisInsert,'PADRAO',1)
	END


COMMIT TRAN
GO