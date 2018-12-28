using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using IdentityModel.Client;

namespace ScriptServerStation.Tests
{
    public class UnitTest1
    {
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
        /// <summary>
        /// 检测IdentityServer服务，测试本条信息必须启动API与主项目
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task IdentityServerTest()
        {
            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync("http://localhost:56223");
            Assert.False(disco.IsError);
            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "wpf", "MyWpfSecret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("admin", "123456");

            Assert.False(tokenResponse.IsError);

            //call userInfo
            var client1 = new HttpClient();
            client1.SetBearerToken(tokenResponse.AccessToken);

            var response1 = await client1.GetAsync("http://localhost:56223/connect/userinfo");
            Assert.True(response1.IsSuccessStatusCode);

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:5001/identity");
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
