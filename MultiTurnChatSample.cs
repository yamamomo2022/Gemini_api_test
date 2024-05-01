
using Google.Cloud.AIPlatform.V1;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gemini_api_test
{

    public class MultiTurnChatSample
    {        
        public async Task<string> GenerateContent(
            string projectId,
            string location = "us-central1",
            string publisher = "google",
            string model = "gemini-1.0-pro"
        )
        {
            // Create a chat session to keep track of the context
            ChatSession chatSession = new ChatSession($"projects/{projectId}/locations/{location}/publishers/{publisher}/models/{model}", location);

            string prompt = "Hello.";
            Console.WriteLine($"\nUser: {prompt}");

            string response = await chatSession.SendMessageAsync(prompt);
            Console.WriteLine($"Response: {response}");

            prompt = "What are all the colors in a rainbow?";
            Console.WriteLine($"\nUser: {prompt}");

            response = await chatSession.SendMessageAsync(prompt);
            Console.WriteLine($"Response: {response}");

            prompt = "Why does it appear when it rains?";
            Console.WriteLine($"\nUser: {prompt}");

            response = await chatSession.SendMessageAsync(prompt);
            Console.WriteLine($"Response: {response}");

            return response;
        }

        private class ChatSession
        {
            private readonly string _modelPath;
            private readonly PredictionServiceClient _predictionServiceClient;

            private readonly List<Content> _contents;

            public ChatSession(string modelPath, string location)
            {
                _modelPath = modelPath;

                // Create a prediction service client.
                _predictionServiceClient = new PredictionServiceClientBuilder
                {
                    Endpoint = $"{location}-aiplatform.googleapis.com"
                }.Build();

                // Initialize contents to send over in every request.
                _contents = new List<Content>();
            }


            public async Task<string> SendMessageAsync(string prompt)
            {
                // Initialize the content with the prompt.
                var content = new Content
                {
                    Role = "USER"
                };
                content.Parts.AddRange(new List<Part>()
                {
                    new() {
                        Text = prompt
                    }
                });
                _contents.Add(content);


                // Create a request to generate content.
                var generateContentRequest = new GenerateContentRequest
                {
                    Model = _modelPath,
                    GenerationConfig = new GenerationConfig
                    {
                        Temperature = 0.9f,
                        TopP = 1,
                        TopK = 32,
                        CandidateCount = 1,
                        MaxOutputTokens = 2048
                    }
                };
                generateContentRequest.Contents.AddRange(_contents);

                // Make a non-streaming request, get a response.
                GenerateContentResponse response = await _predictionServiceClient.GenerateContentAsync(generateContentRequest);

                // Save the content from the response.
                _contents.Add(response.Candidates[0].Content);

                // Return the text
                return response.Candidates[0].Content.Parts[0].Text;
            }
        }
    }
}