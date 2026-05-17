using Eli.Finance.CreditCard.UsBank;

namespace Eli.Finance.Test.CreditCard.UsBank;

[TestFixture]
public class UsBankTransactionProcessorTest
{
    private string _sampleFile = null!;
    private readonly UsBankTransactionProcessor tp = new();

    [SetUp]
    public void Setup() => _sampleFile = Path.Combine(AppContext.BaseDirectory, "TestData", "2025_us_bank_transactions.csv");

    [Test]
    public void Read_ShouldParseTransactions()
    {
        var transactions = tp.ParseTransactionsFromFile(_sampleFile);

        Assert.Multiple(() =>
        {
            Assert.That(transactions, Has.Count.EqualTo(6));
            Assert.That(transactions[0].PostDate, Is.EqualTo(new DateOnly(2025, 12, 30)));
            Assert.That(transactions[0].TransDate, Is.EqualTo(new DateOnly(2025, 12, 30)));
            Assert.That(transactions[0].RefNumber, Is.EqualTo("MTC"));
            Assert.That(transactions[0].TransactionDescription, Is.EqualTo("PAYMENT THANK YOU"));
            Assert.That(transactions[0].Amount, Is.EqualTo(123.45m));
            Assert.That(transactions[0].TransactionType, Is.EqualTo(UsBankTransactionType.Purchase));
            Assert.That(transactions[1].RefNumber, Is.EqualTo("ET"));
            Assert.That(transactions[1].Amount, Is.EqualTo(-78.90m));
            Assert.That(transactions[1].TransactionType, Is.EqualTo(UsBankTransactionType.Payment));
            Assert.That(transactions[2].TransactionDescription, Is.EqualTo("MTA*NYCT PAYGO NEW YORK NY"));
            Assert.That(transactions[2].Amount, Is.EqualTo(10.11m));
            Assert.That(transactions[2].TransactionType, Is.EqualTo(UsBankTransactionType.Purchase));
            Assert.That(transactions[4].TransDate, Is.EqualTo(new DateOnly(2025, 4, 2)));
            Assert.That(transactions[4].TransactionDescription, Is.EqualTo("MTA*LIRR ETIX TICKET 123-456-7890 NY"));
            Assert.That(transactions[5].PostDate, Is.EqualTo(new DateOnly(2025, 4, 7)));
            Assert.That(transactions[5].Amount, Is.EqualTo(15.16m));
        });
    }

    [Test]
    public void Read_ShouldThrowWhenFileNameDoesNotStartWithYear()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "TestData", "us_bank_transactions.csv");

        var exception = Assert.Throws<ArgumentException>(() => tp.ParseTransactionsFromFile(filePath));

        Assert.That(
            exception!.Message,
            Does.Contain("US Bank transaction file name must begin with a four-digit statement year"));
    }
}
