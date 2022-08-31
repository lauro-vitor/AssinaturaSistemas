using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Getnet.DTO.Error
{
    public class ErrorDetail
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("error_code")]
        public string ErrorCode { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("description_detail")]
        public string DescriptionDetail { get; set; }

        public override string ToString()
        {
            return $@"
[
  status = {Status},
  error_code = {ErrorCode},
  description = {Description},
  description_detail = {DescriptionDetail}
]
            ";
        }
    }
}
