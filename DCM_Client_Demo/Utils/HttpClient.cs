using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;

namespace DCM_Client_Demo.Utils;

public class HttpClientDemo : IHttpClientDemo, IDisposable
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public HttpClientDemo(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<object> GetAsync(string url, Type type)
    {
        using (HttpClient client = new HttpClient())
        {
            var accessToken = await _httpContextAccessor
                .HttpContext
                .GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
          
            using (var Response = await client.GetAsync("http://localhost:5096/"+url))
            {
                if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string result = Response.Content.ReadAsStringAsync().Result;
                    try
                    {
                        return JsonConvert.DeserializeObject(result, type);

                    }
                    catch
                    {
                        return null;
                    }
                }
                else if (Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException();
                }
                else
                {
                    return null;
                }
            }

        }
    }

    public void Dispose()
    {
        
    }
}