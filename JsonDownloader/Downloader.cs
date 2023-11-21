using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

/// <summary>
/// The <see cref="MyHttpClientHandler"/> class provides a simple interface for making asynchronous HTTP requests using <see cref="HttpClient"/>.
/// </summary>
/// <remarks>
/// This class simplifies the management of the <see cref="HttpClient"/> instance and provides a method for asynchronously retrieving a response from a given URL.
/// </remarks>
public class MyHttpClientHandler : IDisposable
{
    /// <summary>
    /// Gets or sets the underlying <see cref="HttpClient"/> instance used for making HTTP requests.
    /// </summary>
    private readonly HttpClient httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="MyHttpClientHandler"/> class.
    /// </summary>
    /// <remarks>
    /// The constructor creates a new instance of <see cref="HttpClient"/> for handling HTTP requests.
    /// </remarks>
    public MyHttpClientHandler()
    {
        httpClient = new HttpClient();
    }

    /// <summary>
    /// Performs an asynchronous HTTP GET request to the specified URL.
    /// </summary>
    /// <param name="url">The URL to send the HTTP GET request to.</param>
    /// <returns>A task representing the asynchronous operation that yields an <see cref="HttpResponseMessage"/>.</returns>
    /// <remarks>
    /// This method initiates an asynchronous HTTP GET request using the <see cref="HttpClient"/> instance and returns the
    /// corresponding <see cref="HttpResponseMessage"/>.
    /// </remarks>
    public async Task<HttpResponseMessage> GetAsync(string url)
    {
        return await httpClient.GetAsync(url);
    }

    public void Dispose() => httpClient.Dispose();
}

/// <summary>
/// The <see cref="JsonDownloader"/> class is used for downloading and processing JSON data using HTTP requests.
/// </summary>
/// <remarks>
/// The class takes an instance of <see cref="MyHttpClientHandler"/> for handling HTTP requests. The <see cref="GetJsonAsync{JSON}"/> method
/// retrieves JSON data from the specified URL and returns a tuple containing deserialized data, a success indicator, and any potential error.
/// </remarks>
public class JsonDownloader
{
    /// <summary>
    /// Gets or sets the <see cref="MyHttpClientHandler"/> instance responsible for handling HTTP requests.
    /// </summary>
    private readonly MyHttpClientHandler httpClientHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonDownloader"/> class.
    /// </summary>
    /// <param name="httpClientHandler">The <see cref="MyHttpClientHandler"/> instance to be used for handling HTTP requests.</param>
    /// <remarks>
    /// This constructor receives an instance of <see cref="MyHttpClientHandler"/> to be used for making HTTP requests.
    /// </remarks>
    public JsonDownloader(MyHttpClientHandler httpClientHandler)
    {
        this.httpClientHandler = httpClientHandler;
    }

    /// <summary>
    /// Asynchronously retrieves JSON data from the specified URL using the configured <see cref="MyHttpClientHandler"/>.
    /// </summary>
    /// <typeparam name="JSON">The type to which the JSON data will be deserialized.</typeparam>
    /// <param name="url">The URL to send the HTTP GET request to.</param>
    /// <returns>A task representing the asynchronous operation that yields a tuple containing deserialized data, a success indicator, and any potential error.</returns>
    /// <remarks>
    /// This method initiates an asynchronous HTTP GET request using the configured <see cref="MyHttpClientHandler"/> and returns a tuple containing
    /// deserialized data, a success indicator, and any potential error encountered during the process.
    /// </remarks>
    public async Task<(JSON? DataJson, bool IsOk, string Error)> GetJsonAsync<JSON>(string url) where JSON : class, new()
    {
        JSON? jsonResponse = new();

        try
        {
            var response = await httpClientHandler.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                jsonResponse = await response.Content.ReadFromJsonAsync<JSON>();
                return (jsonResponse, true, string.Empty);
            }
            else
            {
                return (jsonResponse, false, $"{response.StatusCode} {response.ReasonPhrase} {response.IsSuccessStatusCode}");
            }
        }
        catch (HttpRequestException ex)
        {
            return (jsonResponse, false, ex.Message);
        }
    }
}
