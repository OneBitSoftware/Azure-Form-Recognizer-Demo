using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using System.Threading.Tasks;

string key = "";
string endpoint = "https://globalazureforms.cognitiveservices.azure.com/";

var credential = new AzureKeyCredential(key);
var client = new DocumentAnalysisClient(new Uri(endpoint), credential);

await GetClassification(modelId: "test-model-5", client);

await GetContents(modelId: "prebuilt-read", client);

Console.WriteLine("Done.");
Console.ReadLine();

static async Task GetClassification(string modelId, DocumentAnalysisClient client)
{
    foreach (var file in Directory.GetFiles("classification-test-data"))
    {
        Console.WriteLine($"Analyzing: \\{file}");
        using var stream = File.OpenRead(file);

        // If using a classifier:
        var result = await client.ClassifyDocumentAsync(WaitUntil.Completed, modelId, stream);

        if (!result.HasValue) continue;

        if (result.Value.Documents.Any())
        {
            var documentType = result.Value.Documents.First().DocumentType;

            Console.WriteLine($"The document type is: {documentType}");
        }
    }
}

static async Task GetContents(string modelId, DocumentAnalysisClient client)
{
    foreach (var file in Directory.GetFiles("extraction-test-data"))
    {
        Console.WriteLine($"Analyzing: \\{file}");
        using var stream = File.OpenRead(file);

        // If using a analyze:
        var result = await client.AnalyzeDocumentAsync(WaitUntil.Completed, modelId, stream);

        if (!result.HasValue) continue;

        var documentContent = result.Value.Content;

        Console.WriteLine($"The document content is: {documentContent}");
    }
}
