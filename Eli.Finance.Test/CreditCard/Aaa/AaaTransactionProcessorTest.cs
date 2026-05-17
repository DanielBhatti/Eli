using Eli.Finance.CreditCard.Aaa;

namespace Eli.Finance.Test.CreditCard.Aaa;

[TestFixture]
public class AaaTransactionProcessorTest
{
    private string _sampleFile = null!;
    private readonly AaaTransactionProcessor tp = new();

    [SetUp]
    public void Setup()
    {
        _sampleFile = Path.Combine(
            AppContext.BaseDirectory,
            "TestData",
            "aaa_transactions.csv");
    }

    [Test]
    public void Read_ShouldParseTransactions()
    {
        var transactions = tp.ParseMinDetailTransactionsFromFile(_sampleFile);

        Assert.Multiple(() =>
        {
            Assert.That(transactions, Has.Count.EqualTo(3));

            Assert.That(
                transactions[0].Amount,
                Is.EqualTo(-54.25m));

            Assert.That(
                transactions[0].Description,
                Is.EqualTo(
                    "ABC #1234 TREEHOUSE CREDIT"));

            Assert.That(
                transactions[1].Amount,
                Is.EqualTo(183.81m));

            Assert.That(
                transactions[2].Date,
                Is.EqualTo(new DateOnly(2023, 12, 10)));
        });
    }
}