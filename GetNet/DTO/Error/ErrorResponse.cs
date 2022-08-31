using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Getnet.DTO.Error
{
    public class ErrorResponse
    {
        [JsonProperty("status_code")]
        public string SatusCode { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("details")]
        public List<ErrorDetail> ErrorDetails { get; set; }

        public override string ToString()
        {
            string details = "";

            foreach(var errorDetail  in ErrorDetails)
            {
                details += errorDetail.ToString();
            }

            return $@"
 status_code = {SatusCode},
 name ={Name},
 message = {Message},
 details = {details}";

        }

    }
}
