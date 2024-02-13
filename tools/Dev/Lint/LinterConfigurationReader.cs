using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Json.Schema;

namespace Sustenance.Dev.Lint;

internal static class LinterConfigurationReader
{
    private const string JsonFileName = "linters.json";
    private const string JsonSchemaFileName = "linters.schema.json";

    private static JsonSerializerOptions? _jsonOptions;

    private static JsonSerializerOptions JsonOptions
    {
        get
        {
            if (_jsonOptions is null)
            {
                _jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                _jsonOptions.Converters.Add(new JsonStringEnumConverter<PackageManagerPlatform>());
                _jsonOptions.Converters.Add(new PackageManagerNameJsonConverter());
            }

            return _jsonOptions;
        }
    }

    public static LinterConfigurationResult Read()
    {
        var directory = Directory.GetCurrentDirectory();
        if (!Debugger.IsAttached)
        {
            directory = Path.Combine(directory, "tools", "Dev");
        }

        var jsonPath = Path.Combine(directory, "Lint", JsonFileName);
        if (!File.Exists(jsonPath))
        {
            return LinterConfigurationResult.Failure("The linter configuration file was not found.");
        }

        var jsonSchemaPath = Path.Combine(directory, "Lint", JsonSchemaFileName);
        if (!File.Exists(jsonSchemaPath))
        {
            return LinterConfigurationResult.Failure("The linter configuration schema file was not found.");
        }

        using var jsonStream = new FileStream(jsonPath, FileMode.Open, FileAccess.Read);
        var jsonDocument = JsonDocument.Parse(jsonStream);
        var jsonSchema = JsonSchema.FromFile(jsonSchemaPath);

        var evaluationResults = jsonSchema.Evaluate(jsonDocument);
        if (!evaluationResults.IsValid)
        {
            return LinterConfigurationResult.Failure("The linter file did not match the expected schema.");
        }

        var linterConfiguration = JsonSerializer.Deserialize<LinterConfiguration>(jsonDocument, options: JsonOptions);
        return linterConfiguration is null
            ? LinterConfigurationResult.Failure("Unable to deserialize the linter configuration.")
            : LinterConfigurationResult.Success(linterConfiguration);
    }
}
