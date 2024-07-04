using System.Text.Json;
using Graeae.Models.SchemaDraft4;
using Json.Schema;

namespace graeae_validate;

public class Tests
{
    [OneTimeSetUp]
    public void Setup()
    {
        Draft4Support.Enable();
    }

    [Test]
    [TestCase("./Files/payload-valid.json")]
    public void TestValid(string file)
    {
        IBaseDocument schema = JsonSchema.FromFile("./Files/schema-components.json");
        SchemaRegistry.Global.Register(schema);

        var componentRef = "#/components/schemas/outer";

        var successJson = File.ReadAllText(file);
        var success = JsonDocument.Parse(successJson);
        var options = new EvaluationOptions
        {
            EvaluateAs = Draft4Support.Draft4Version,
        };

        JsonSchema validateSchema = new JsonSchemaBuilder()
            .Ref(new Uri(schema.BaseUri, componentRef));

        var results = validateSchema.Evaluate(success, options);
        Assert.True(results.IsValid);
    }


    [Test]
    [TestCase("./Files/payload-invalid1.json")]
    [TestCase("./Files/payload-invalid2.json")]
    [TestCase("./Files/payload-invalid3.json")]
    [TestCase("./Files/payload-invalid4.json")]
    public void TestInvalid(string file)
    {
        IBaseDocument schema = JsonSchema.FromFile("./Files/schema-components.json");
        SchemaRegistry.Global.Register(schema);

        var componentRef = "#/components/schemas/outer";

        var successJson = File.ReadAllText(file);
        var success = JsonDocument.Parse(successJson);
        var options = new EvaluationOptions
        {
            EvaluateAs = Draft4Support.Draft4Version,
        };

        JsonSchema validateSchema = new JsonSchemaBuilder()
            .Ref(new Uri(schema.BaseUri, componentRef));

        var results = validateSchema.Evaluate(success, options);
        Assert.False(results.IsValid);
    }}