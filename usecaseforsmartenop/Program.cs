// See https://aka.ms/new-console-template for more information
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using usecaseforsmartenop;

string csvFilePath =  "chatbot_data.csv";
var prompt = new List<PromptItem>();
var records = new List<CsvClass>();

// read chatbot_data file
using (var reader = new StreamReader(csvFilePath))
using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
{
  records = csv.GetRecords<CsvClass>().ToList(); 
}

double sum = 0;
double countOfSamples = 0;
double accuracy = 0;

// vaidate the responses 
foreach (var record in records)
{
    prompt.Add(new PromptItem
    {
        role = "user",
        content = record.Questions
    });
    //call completion request
    var responseOfCompletion = await RequestOpenAIChatAPI(prompt);
    double responseOfSimilarity = 0;

    if (responseOfCompletion != "Error")
    {
        // call accuracy request for semantic similarity
        responseOfSimilarity = await RequestSemanticAPI(record.Answers, responseOfCompletion);

        if (responseOfSimilarity != -1)
        {
            sum = sum + responseOfSimilarity;
            countOfSamples++;
        }
    }

    prompt.Add(new PromptItem
    {
        role = "assistant",
        content = record.Answers
    });
}

//calculate accuracy average for all samples
accuracy = sum / countOfSamples;


Console.WriteLine($"accuracy of validation the chatbot: {accuracy}");



static async Task<string> RequestOpenAIChatAPI(List<PromptItem> prompt)
{
    string apiKey = "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";
    string apiUrl = "https://api.openai.com/v1/chat/completions";

    using (HttpClient client = new HttpClient())
    {
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        JsonRequestBody requestBody = new JsonRequestBody
        {
            model = "gpt-3.5-turbo",
            messages = prompt.ToArray()
        };
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
        
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync(apiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            ChatCompletionResponse openAIResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ChatCompletionResponse>(responseBody);
            return openAIResponse.Choices[0].Message.Content;
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            return "Error";
        }
    }
}

static async Task<double> RequestSemanticAPI(string text1, string text2)
{
    string apiUrl = "https://api.dandelion.eu/datatxt/sim/v1/";
    string token = "f59f3fad1b3440e18658ffcea005cff9"; 
    string queryString = $"?text1={Uri.EscapeDataString(text1)}&text2={Uri.EscapeDataString(text2)}&token={token}";

    using (HttpClient httpClient = new HttpClient())
    {
        HttpResponseMessage response = await httpClient.GetAsync(apiUrl + queryString);

        if (response.IsSuccessStatusCode)
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            SimilarityResponse similarityResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<SimilarityResponse>(responseContent);
            return similarityResponse.Similarity;
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
            return -1;
        }
    }


}



