namespace JsonDownloader.Model
{
    /// <summary>
    /// Represents the output of a Json-related operation, containing deserialized data, 
    /// a success indicator, and an optional error message.
    /// </summary>
    /// <typeparam name="Json">The type of the deserialized Json data.</typeparam>
    public class JsonOutput<Json>
    {
        /// <summary>
        /// Gets or sets the deserialized Json data.
        /// </summary>
        public Json? DataJson { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating the success of the operation.
        /// </summary>
        public bool IsOk { get; set; }

        /// <summary>
        /// Gets or sets an optional error message. It is an empty string by default.
        /// </summary>
        public string Error { get; set; } = string.Empty;
    }
}
