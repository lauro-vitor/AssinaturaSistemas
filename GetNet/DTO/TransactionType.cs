using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Getnet.DTO
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TransactionType
    {
        FULL, //AVISTA
        INSTALL_NO_INTEREST, //PARCELADO SEM JUROS
        INSTALL_WITH_INTEREST //PARCELADO COM JUROS 
    }
}
