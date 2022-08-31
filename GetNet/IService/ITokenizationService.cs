using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Getnet.IService
{
    
    public interface ITokenizationService
    {
        //Para efetuar transações de forma segura os dados do cartão, exceto CVV, devem ser tokenizados,
        //esse processo é feito pelo endpoint descrito  mediante autenticação.
        Task<string> GetCreditCardToken(string cardNumber, string custumerId);
    }
}
