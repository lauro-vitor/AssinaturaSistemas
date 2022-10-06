using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    interface IParcelaDAL
    {
         List<Parcela> Criar(List<Parcela> parcelas);
         void AbrirParcela(int idParcela);
         void PagarParcela(int idParcela);
         void CancelarParcela(int idParcela);
    }
}
