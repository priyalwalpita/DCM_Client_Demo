namespace DCM_Client_Demo.Utils;

public interface IHttpClientDemo
{
    Task<object> GetAsync(string url, Type type);
}