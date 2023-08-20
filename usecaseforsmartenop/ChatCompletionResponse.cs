using System;
namespace usecaseforsmartenop
{
    public class ChatCompletionResponse
    {
        public ChatCompletionResponse()
        {}

        public string Id { get; set; }
        public string Object { get; set; }
        public long Created { get; set; }
        public string Model { get; set; }
        public ChatChoice[] Choices { get; set; }
        public Usage Usage { get; set; }
    }

    public class ChatChoice
    {
        public int Index { get; set; }
        public OpenAIMessage Message { get; set; }
        public string FinishReason { get; set; }
    }

    public class OpenAIMessage
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }

    public class Usage
    {
        public int PromptTokens { get; set; }
        public int CompletionTokens { get; set; }
        public int TotalTokens { get; set; }
    }

}

