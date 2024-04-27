using System;

namespace Gemini_api_test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var chatSample = new MultiTurnChatSample();
            await chatSample.GenerateContent();
        }
    }
}
