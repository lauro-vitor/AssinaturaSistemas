using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Getnet.DTO
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Brand
    {
        Mastercard,
        Visa,
        Amex,
        Elo,
        Hipercard
    }
}
