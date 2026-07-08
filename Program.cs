using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OllamaSharp;

const string ollamaUrl = "http://localhost:11434";
const string model = "llava";

var handler = new HttpClientHandler();
var httpClient = new HttpClient(handler)
{
    BaseAddress = new Uri(ollamaUrl),
    Timeout = TimeSpan.FromMinutes(2)
};

var ollama = new OllamaApiClient(httpClient);
var imageBytes = await File.ReadAllBytesAsync("Images/bandeira-do-brasil.png");
var base64Image = Convert.ToBase64String(imageBytes);

var agent = ollama
    .AsAIAgent(new ChatClientAgentOptions
    {
        Name = "ImageReviewerAgente",
        Description = "Agente especialista em revisão imagens",
        ChatOptions = new ChatOptions
        {
            ModelId = model,
            Temperature = 0.7f,
            Instructions = "Você vai receber uma imagem ou URL e vai dizer o que tem nela"
        }
    });

var message = new ChatMessage(ChatRole.User, [
        new TextContent("O que você vê nesta imagem?"),
        new DataContent(imageBytes, "image/png")
    ]);

Console.WriteLine(await agent.RunAsync(message));

