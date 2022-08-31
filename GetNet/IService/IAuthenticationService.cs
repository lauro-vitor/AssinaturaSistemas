using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Getnet.IService
{
    public interface IAuthenticationService
    {
        Task<string> GetToken();
    }
}
