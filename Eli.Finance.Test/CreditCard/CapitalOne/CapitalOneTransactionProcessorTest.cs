using Eli.Finance.CreditCard.CapitalOne;

namespace Eli.Finance.Test.CreditCard.CapitalOne;

[TestFixture]
public class CapitalOneTransactionProcessorTest
{
    private readonly CapitalOneTransactionProcessor tp = new();

    [Test]
    public void Read_ShouldParseOldDateColumnTransactions()
    {
        var sampleFile = Path.Combine(AppContext.BaseDirectory, "TestData", "2021_capital_one_transactions_1.csv");
        var transactions = tp.ParseTransactionsFromFile(sampleFile);
        Assert.Multiple(() =>
        {
            Assert.That(transactions, Has.Count.EqualTo(2));
            Assert.That(transactions[0].TransDate, Is.EqualTo(new DateOnly(2021, 11, 15)));
            Assert.That(transactions[0].PostDate, Is.Null);
            Assert.That(transactions[0].Description, Is.EqualTo("MICRO CENTER - QUEENSFLUSHINGNY"));
            Assert.That(transactions[0].Amount, Is.EqualTo(123.45m));
            Assert.That(transactions[0].TransactionType, Is.EqualTo(CapitalOneTransactionType.Purchase));
            Assert.That(transactions[1].TransDate, Is.EqualTo(new DateOnly(2021, 12, 1)));
            Assert.That(transactions[1].PostDate, Is.Null);
            Assert.That(transactions[1].Amount, Is.EqualTo(67.89m));
        });
    }

    [Test]
    public void Read_ShouldParseNewTransDateAndPostDateTransactions()
    {
        var sampleFile = Path.Combine(AppContext.BaseDirectory, "TestData", "2021_capital_one_transactions_2.csv");
        var transactions = tp.ParseTransactionsFromFile(sampleFile);
        Assert.Multiple(() =>
        {
            Assert.That(transactions, Has.Count.EqualTo(4));
            Assert.That(transactions[0].TransDate, Is.EqualTo(new DateOnly(2021, 4, 27)));
            Assert.That(transactions[0].PostDate, Is.EqualTo(new DateOnly(2021, 4, 27)));
            Assert.That(transactions[0].Description, Is.EqualTo("CAPITAL ONE ONLINE PYMTAuthDate 27-Apr"));
            Assert.That(transactions[0].Amount, Is.EqualTo(-123.45m));
            Assert.That(transactions[0].TransactionType, Is.EqualTo(CapitalOneTransactionType.Payment));
            Assert.That(transactions[1].Amount, Is.EqualTo(-678.90m));
            Assert.That(transactions[1].TransactionType, Is.EqualTo(CapitalOneTransactionType.Payment));
            Assert.That(transactions[2].TransDate, Is.EqualTo(new DateOnly(2021, 4, 22)));
            Assert.That(transactions[2].PostDate, Is.EqualTo(new DateOnly(2021, 4, 23)));
            Assert.That(transactions[2].Description, Is.EqualTo("CUBESMART 12345-12345NY"));
            Assert.That(transactions[2].TransactionType, Is.EqualTo(CapitalOneTransactionType.Purchase));
            Assert.That(transactions[3].Amount, Is.EqualTo(56.78m));
        });
    }

    [Test]
    public void Read_ShouldThrowWhenFileNameDoesNotStartWithYear()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "TestData", "capital_one_transactions.csv");
        var exception = Assert.Throws<ArgumentException>(() => tp.ParseTransactionsFromFile(filePath));
        Assert.That(exception!.Message, Does.Contain("Capital One transaction file name must begin with a four-digit statement year"));
    }
}
