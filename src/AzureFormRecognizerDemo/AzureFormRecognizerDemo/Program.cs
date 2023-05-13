using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using System.Threading.Tasks;

string key = "";
string endpoint = "https://globalazureforms.cognitiveservices.azure.com/";
string modelId = "test-model-5";

var credential = new AzureKeyCredential(key);
var client = new DocumentAnalysisClient(new Uri(endpoint), credential);

foreach (var file in Directory.GetFiles("testdata"))
{
    Console.WriteLine($"Analyzing: \\{file}");
    using var stream = File.OpenRead(file);

    // If using a pre-built model:
    //var result = await client.AnalyzeDocumentAsync(WaitUntil.Completed, modelId, stream);

    // If using a classifier:
    var result = await client.ClassifyDocumentAsync(WaitUntil.Completed, modelId, stream);

    if (result.HasValue && result.Value.Documents.Any())
    {
        var documentType = result.Value.Documents.First().DocumentType;

        Console.WriteLine($"The document type is: {documentType}");
    }
}

Console.WriteLine("Done.");
Console.ReadLine();