namespace IDNumberValidator.Svc.Model
{
    /// <summary>
    /// Request model for validation.
    /// </summary>
    public class IdNumberValidationRequest
    {
        /// <summary>
        /// The ID number to validate (e.g., credit card).
        /// </summary>
        public string IdNumber { get; set; }

        /// <summary>
        /// The type of validation to perform.
        /// </summary>
        public IdType Type { get; set; }
    }
}
