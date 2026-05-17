using Eli.Finance.CreditCard.Citi;

namespace Eli.Finance.Test.CreditCard.Citi;

[TestFixture]
public class CitiTransactionProcessorTest
{
    private string _sampleFile = null!;
    private readonly CitiTransactionProcessor tp = new();

    [SetUp]
    public void Setup() => _sampleFile = Path.Combine(AppContext.BaseDirectory, "TestData", "citi_transactions.csv");

    [Test]
    public void Read_ShouldParseTransactions()
    {
        var transactions = tp.ParseTransactionsFromFile(_sampleFile);

        Assert.Multiple(() =>
        {
            Assert.That(transactions, Has.Count.EqualTo(4));
            Assert.That(transactions[0].Date, Is.EqualTo(new DateOnly(2024, 8, 20)));
            Assert.That(transactions[0].Description, Is.EqualTo("AIRLINE 06952458241246 NEW YORK NY"));
            Assert.That(transactions[0].Debit, Is.EqualTo(1171.8m));
            Assert.That(transactions[0].Credit, Is.EqualTo(0m));
            Assert.That(transactions[0].Category, Is.EqualTo("Air Travel"));
            Assert.That(transactions[1].Debit, Is.EqualTo(55m));
            Assert.That(transactions[2].Date, Is.EqualTo(new DateOnly(2024, 8, 18)));
            Assert.That(transactions[3].Category, Is.EqualTo("Health Care"));
        });
    }
}
