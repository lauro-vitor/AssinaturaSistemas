using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Getnet.DTO.Credit
{
    /// <summary>
    /// Conjunto de dados referentes a transação de crédito.
    /// </summary>
    public class CreditRequest
    {
        /// <summary>
        ///     Identifica se o crédito será feito com confirmação tardia.
        /// </summary>
        [JsonRequired]
        [JsonProperty("delayed")]
        public bool Delayed { get; set; }

        /// <summary>
        /// Indicativo se a transação é uma pré autorização de crédito
        /// </summary>
        [JsonRequired]
        [JsonProperty("pre_authorization")]
        public bool PreAuthorization { get; set; }

        /// <summary>
        /// Identifica se o cartão deve ser salvo para futuras compras.
        /// </summary>
        /// 
        [JsonRequired]
        [JsonProperty("save_card_data")]
        public bool SaveCardData { get; set; }

        /// <summary>
        /// Tipo de transação. Pagamento completo à vista, parcelado sem juros, parcelado com juros.
        /// </summary>
        /// 
        [JsonRequired]
        [JsonProperty("transaction_type")]
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// Número de parcelas para uma transação de crédito parcelado
        /// </summary>
        ///
        [JsonRequired]
        [JsonProperty("number_installments")]
        public int NumberInstallments { get; set; }

        /// <summary>
        /// Texto exibido na fatura do cartão do comprador, este campo é opcional, não sendo informado nada,
        /// será considerado o nome fantasia cadastrado para o estabelecimento. É permitido neste campo somente 
        /// dados alfanuméricos e os seguintes caracteres especiais: % $ , . / & ( ) + = < > - * Espaços serão
        /// substituídos pelo caracter '*' A transação não será negada se forem enviados caracteres no permitidos. 
        /// Qualquer informação que exceda o tamanho máximo será desconsiderada
        /// </summary>
        [JsonProperty("soft_descriptor")]
        public string SoftDescriptor { get; set; }

        /// <summary>
        /// Campo utilizado para sinalizar a transação com outro Merchant Category Code (Código da Categoria do Estabelecimento) 
        /// diferente do cadastrado. Caso seja enviado um MCC inválido, será utilizado o padrão. Enviar somente dados numéricos
        /// </summary>
        ///
        [JsonProperty("dynamic_mcc")]
        public int DynamicMcc { get; set; }

        [JsonRequired]
        [JsonProperty("card")]
        public Card Card { get; set; }
            
    }
}
