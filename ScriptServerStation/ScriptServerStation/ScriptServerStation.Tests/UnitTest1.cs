using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using IdentityModel;
using IdentityModel.Client;

namespace ScriptServerStation.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task ClientCredentials_Test()
        {
            // request token
            //var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            var DiscoveryClient = new DiscoveryClient("http://localhost:5000") { Policy = { RequireHttps = false } };
            var disco = await DiscoveryClient.GetAsync();
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client1", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

            Assert.False(tokenResponse.IsError);
            Console.WriteLine(tokenResponse.Json);

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:5010/values");
            Assert.True(response.IsSuccessStatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
        }
    }
}
