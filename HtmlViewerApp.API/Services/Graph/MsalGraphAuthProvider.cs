using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Extensions.Configuration;

public class GraphClientFactory
{
    public static GraphServiceClient Create(IConfiguration config)
    {
        var tenantId = config["AzureAd:TenantId"];
        var clientId = config["AzureAd:ClientId"];
        var clientSecret = config["AzureAd:ClientSecret"];

        var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

        return new GraphServiceClient(credential);
    }
}
