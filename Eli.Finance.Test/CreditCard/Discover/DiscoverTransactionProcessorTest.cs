using Eli.Finance.CreditCard.Discover;

namespace Eli.Finance.Test.CreditCard.Discover;

[TestFixture]
public class DiscoverTransactionProcessorTest
{
    private string _sampleFile = null!;
    private readonly DiscoverTransactionProcessor tp = new();

    [SetUp]
    public void Setup() => _sampleFile = Path.Combine(AppContext.BaseDirectory, "TestData", "discover_transactions.csv");

    [Test]
    public void Read_ShouldParseTransactions()
    {
        var transactions = tp.ParseTransactionsFromFile(_sampleFile);

        Assert.Multiple(() =>
        {
            Assert.That(transactions, Has.Count.EqualTo(3));
            Assert.That(transactions[0].TransDate, Is.EqualTo(new DateOnly(2023, 12, 2)));
            Assert.That(transactions[0].Amount, Is.EqualTo(-108.26m));
            Assert.That(transactions[0].Category, Is.EqualTo("Payments and Credits"));
            Assert.That(transactions[1].Description, Is.EqualTo("AMZN MKTP US*AH2Z06323 AMZN.COM/BILLWA\r\n3E8YSZ0343Q"));
            Assert.That(transactions[1].Amount, Is.EqualTo(39.12m));
            Assert.That(transactions[2].TransDate, Is.EqualTo(new DateOnly(2023, 10, 26)));
            Assert.That(transactions[2].Category, Is.EqualTo("Merchandise"));
        });
    }
}
