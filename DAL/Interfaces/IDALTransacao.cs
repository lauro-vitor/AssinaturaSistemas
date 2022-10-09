using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IDALTransacao<T>
    {
        T Criar(T objeto, SqlConnection sqlConnection, SqlTransaction sqlTransaction);
    }
}
