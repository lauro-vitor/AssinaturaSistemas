using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Administrativo.Models.SistemaModel
{
    public class SistemaViewModel
    {
        public string IdSistema { get; set; }
        public string IdTipoSistema { get; set; }
        public string TipoSistemaDescricao { get; set; }
        public string IdCliente { get; set; }
        public string ClienteNomeEmpresa { get; set; }
        public string DominioProvisorio { get; set; }
        public string Dominio { get; set; }
        public string Pasta { get; set; }
        public string BancoDeDados { get; set; }
        public bool Ativo { get; set; }
        public string DataInicio { get; set; }
        public string DataCancelamento { get; set; }

    }
}