using System;

namespace Gemini_api_test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var secret_key = WebApplication.CreateBuilder(args).Configuration["secret_key"];
            var chatSample = new MultiTurnChatSample();
            await chatSample.GenerateContent(
                projectId: secret_key
                );
        }
    }
}
