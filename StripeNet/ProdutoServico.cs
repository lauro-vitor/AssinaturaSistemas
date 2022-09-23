using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stripe;

namespace StripeNet
{
    /// <summary>
    /// Produtos descrevem específicamente bens ou servicos que eh oferecido aos clientes
    /// Eles sao utilizados juntamente com Price "Precos" para configurar   links de pagamento, checkout, e subscriptions
    /// </summary>
    public class ProdutoServico
    {
        private readonly ProductService _service;
        public ProdutoServico()
        {
            StripeConfiguration.ApiKey = System.Configuration.ConfigurationManager.AppSettings["secret_key"];
            _service = new ProductService();
        }

        public StripeList<Product> ListarTodosProdutos()
        {
            var produtos = _service.List();

            return produtos;
        }

        public void CriarProduto(string nomeProduto, string descricao, string codigoTaxa)
        {
            var options = new ProductCreateOptions()
            {
                Name = nomeProduto,
                Description = descricao,
                TaxCode = codigoTaxa
            };

            _service.Create(options);
        }

        #region DOCUMENTACAO PRODUCT
        //essa region descrecve os  atributos do objeto Product

        //campo: object
        //string representa o tipo do objeto seu valor sempre sera product

        //campo: active
        //identifica se o produto esta disponivel para a compra

        //campo: created
        //quando que o produto foi registrado

        //campo: default_price
        //o id do preco que esta vinculado ao produto

        //campo: description
        //descricao longa sobre o produto, o campo eh utilizado para fins de renderizacao

        //campo: images
        //uma lista de ate 8 URLS das imagens do produto

        //campo: livemode
        //indica se o produto existe em producao ou false quando esta em ambiente de teste

        //campo:metadata

        //campo: name
        //nome do produto que sera exibido ao cliente

        //campo: package_dimensions
        //define altura, largura, profundidade, peso

        //campo: shippable
        //indica se o produto esta disponivel para entrega

        //campo: statement_descriptor
        //descreve o que sera mostrado na fatura de cartao de credito do cliente
        //no caso de varios produtos faturados de uma vez so, entao sera utilizado o priemeiro descritor

        //campo: tax_code
        //imposto, no caso " Software as a service (SaaS) - business use " entao a taxa do nosso produto sera  "txcd_10103001"
        //fazer um drop com as prinicipais  

        //campo:unit_label
        //representa as unidades deste produto no stripe  e nas faturas dos clientes, quando definido eh incluido na linha da fatura associada

        //campo: url
        //representa a url publica assecivel do produto
        #endregion
    }
}
