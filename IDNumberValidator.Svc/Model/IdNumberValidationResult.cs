﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IDNumberValidator.Svc.Model
{
    /// <summary>
    /// Validation result model.
    /// </summary>
    [JsonSerializable(typeof(IdNumberValidationResult))]
    public class IdNumberValidationResult
    {
        /// <summary>
        /// The ID number to validate (e.g., credit card).
        /// </summary>
        [JsonPropertyName("valid")]
        public bool Valid { get; set; }

        /// <summary>
        /// The type of Id
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
