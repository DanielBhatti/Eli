using Common.Text;
using NUnit.Framework;

namespace Common.Test;

[TestFixture]
public class CaseConverterTest
{
    [SetUp]
    public void Setup() { }

    [TestCase("hello world", ExpectedResult = "HelloWorld")]
    [TestCase("hello_world", ExpectedResult = "HelloWorld")]
    [TestCase("HelloWorld", ExpectedResult = "HelloWorld")]
    [TestCase("helloWorld", ExpectedResult = "HelloWorld")]
    [TestCase("Hello World", ExpectedResult = "HelloWorld")]
    public string ToPascalCaseTest(string input) => input.ToPascalCase();

    [TestCase("hello world", ExpectedResult = "helloWorld")]
    [TestCase("hello_world", ExpectedResult = "helloWorld")]
    [TestCase("HelloWorld", ExpectedResult = "helloWorld")]
    [TestCase("helloWorld", ExpectedResult = "helloWorld")]
    [TestCase("Hello World", ExpectedResult = "helloWorld")]
    public string ToCamelCaseTest(string input) => input.ToCamelCase();

    [TestCase("hello world", ExpectedResult = "hello_world")]
    [TestCase("hello_world", ExpectedResult = "hello_world")]
    [TestCase("HelloWorld", ExpectedResult = "hello_world")]
    [TestCase("helloWorld", ExpectedResult = "hello_world")]
    [TestCase("Hello World", ExpectedResult = "hello_world")]
    public string ToSnakeCaseTest(string input) => input.ToSnakeCase();

    [TestCase("hello world", ExpectedResult = "Hello World")]
    [TestCase("hello_world", ExpectedResult = "Hello World")]
    [TestCase("HelloWorld", ExpectedResult = "Hello World")]
    [TestCase("helloWorld", ExpectedResult = "Hello World")]
    [TestCase("Hello World", ExpectedResult = "Hello World")]
    public string ToSpacedPascalCaseTest(string input) => input.ToSpacedPascalCase();

    [TestCase("hello world", ExpectedResult = "hello world")]
    [TestCase("hello_world", ExpectedResult = "hello world")]
    [TestCase("HelloWorld", ExpectedResult = "hello world")]
    [TestCase("helloWorld", ExpectedResult = "hello world")]
    [TestCase("Hello World", ExpectedResult = "hello world")]
    public string ToSpacedLowerCaseTest(string input) => input.ToSpacedLowerCase();

    [TestCase("hello world", ExpectedResult = CaseType.SpacedLowerCase)]
    [TestCase("hello_world", ExpectedResult = CaseType.SnakeCase)]
    [TestCase("HelloWorld", ExpectedResult = CaseType.PascalCase)]
    [TestCase("helloWorld", ExpectedResult = CaseType.CamelCase)]
    [TestCase("Hello World", ExpectedResult = CaseType.SpacedPascalCase)]
    [TestCase("Hello_World", ExpectedResult = CaseType.Unknown)]
    [TestCase("helloWorldIsGood", ExpectedResult = CaseType.CamelCase)]
    [TestCase("HelloWorldIsGood", ExpectedResult = CaseType.PascalCase)]
    [TestCase("hello_world_is_good", ExpectedResult = CaseType.SnakeCase)]
    [TestCase("Hello World Is Good", ExpectedResult = CaseType.SpacedPascalCase)]
    [TestCase("HelloWorldIs Good", ExpectedResult = CaseType.Unknown)]
    public CaseType DetermineCaseTest(string input) => input.DetermineCase();
}