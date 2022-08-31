using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Getnet.DTO
{
    /// <summary>
    /// Conjunto de dados referentes ao dispositivo utilizado pelo comprador
    /// </summary>
    public class Device
    {
        /// <summary>
        /// Endereço IP (IPv4) do dispositivo do comprador. Este atributo deve ser capturado 
        /// pela sua aplicação, a partir do dispositivo do comprador (mobile; browser; etc.)
        /// e enviado no payload, afim de enriquecer nossas análises preventivas.
        /// </summary>
        [JsonRequired]
        [JsonProperty("ip_address")]
        public string IpAddress { get; set; }

        /// <summary>
        /// Hash de identificação (Device Fingerprint) do dispositivo.
        /// Mais informações no tópico Antifraude.
        /// </summary>
        [JsonRequired]
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

    }
}
