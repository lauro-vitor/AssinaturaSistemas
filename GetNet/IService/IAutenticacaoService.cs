using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Getnet.IService
{
    public interface IAutenticacaoService
    {
        Task<string> GeracaoTokenAcesso();
    }
}
