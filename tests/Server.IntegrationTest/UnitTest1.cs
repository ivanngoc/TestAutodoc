using System.Text.Json.Nodes;
using Castle.Components.DictionaryAdapter.Xml;
using DAL.EFCore;
using Moq;
using Moq.AutoMock;

namespace Server.IntegrationTest
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var factory = new TargetApplicationFactory<Server.Program>();
            var client = factory.CreateClient();
            var prob = await client.GetAsync("");
            Assert.NotNull(prob);
            prob.EnsureSuccessStatusCode();

            JsonObject obj = new JsonObject()
            {
                [nameof(SomeTask.Name)] = $"Task: {Guid.NewGuid()}"
            };
            var result = await client.PostAsync("api/SomeTasks", new StringContent(obj.ToJsonString()));
            Assert.ThrowsAny<Exception>(() => result.EnsureSuccessStatusCode());
        }
    }
}