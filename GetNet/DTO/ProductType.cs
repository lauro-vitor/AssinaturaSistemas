using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Getnet.DTO
{

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProductType
    {
        cash_carry,
        digital_content,
        digital_goods,
        digital_physical,
        gift_card,
        physical_goods,
        renew_subs,
        shareware, 
        service
    }
}
