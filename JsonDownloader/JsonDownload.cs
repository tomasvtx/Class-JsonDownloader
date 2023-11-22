using JsonDownloader.Model;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace JsonDownloader
{
    /// <summary>
    /// The <see cref="JsonDownload"/> class provides functionality for downloading and processing JSON data using HTTP requests.
    /// </summary>
    public class JsonDownload
{
    /// <summary>
    /// Gets or sets the <see cref="HttpClient"/> instance responsible for handling HTTP requests.
    /// </summary>
    private readonly HttpClient httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonDownload"/> class.
    /// </summary>
    /// <param name="httpClient">The <see cref="HttpClient"/> instance to be used for handling HTTP requests.</param>
    public JsonDownload(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    /// <summary>
    /// Asynchronously retrieves JSON data from the specified URL using the configured <see cref="HttpClient"/>.
    /// </summary>
    /// <typeparam name="JSON">The type to which the JSON data will be deserialized.</typeparam>
    /// <param name="url">The URL to send the HTTP GET request to.</param>
    /// <returns>
    /// A task representing the asynchronous operation that yields a tuple containing deserialized data, 
    /// a success indicator, and any potential error.
    /// </returns>
    /// <remarks>
    /// This method initiates an asynchronous HTTP GET request using the configured <see cref="HttpClient"/>. 
    /// If the response status is successful (HTTP 2xx), the JSON content is deserialized into the specified type. 
    /// If unsuccessful, an error message is generated based on the HTTP response status and reason phrase.
    /// Any network-related issues or failures result in a <see cref="HttpRequestException"/>.
    /// </remarks>
    public async Task<JsonOutput<JSON?>> GetJsonAsync<JSON>(string url) where JSON : class, new()
    {
        try
        {
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var dataJson = await response.Content.ReadFromJsonAsync<JSON?>();
                return CreateJsonOutput(dataJson, string.Empty, true);
            }
            else
            {
                return CreateJsonOutput<JSON?>(null, $"{response.StatusCode} {response.ReasonPhrase} {response.IsSuccessStatusCode}", false);
            }
        }
        catch (HttpRequestException ex)
        {
            return CreateJsonOutput<JSON?>(null, ex.Message, false);
        }
    }

    /// <summary>
    /// Creates a new instance of <see cref="JsonOutput{T}"/> with the specified JSON data, error message, and success indicator.
    /// </summary>
    /// <typeparam name="JSON">The type of the JSON data.</typeparam>
    /// <param name="dataJson">The deserialized JSON data.</param>
    /// <param name="error">The error message, if any.</param>
    /// <param name="isOk">A boolean indicating whether the operation was successful.</param>
    /// <returns>A new instance of <see cref="JsonOutput{T}"/> containing the provided parameters.</returns>
    private JsonOutput<JSON?> CreateJsonOutput<JSON>(JSON dataJson, string error, bool isOk)
    {
        return new JsonOutput<JSON?>
        {
            DataJson = dataJson,
            Error = error,
            IsOk = isOk
        };
    }
    }
}