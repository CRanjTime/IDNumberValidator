using System.Text.Json.Serialization;

namespace IDNumberValidator.Svc.Model
{
    /// <summary>
    /// Request model for validation.
    /// </summary>
    [JsonSerializable(typeof(IdNumberValidationRequest))]
    public class IdNumberValidationRequest
    {
        /// <summary>
        /// The ID number to validate (e.g., credit card).
        /// </summary>
        [JsonPropertyName("idNumber")]
        public string IdNumber { get; set; }

        /// <summary>
        /// The type of validation to perform.
        /// </summary>
        [JsonPropertyName("type")]
        public IdType Type { get; set; }
    }
}
