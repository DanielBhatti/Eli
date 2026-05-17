using Eli.Finance.CreditCard.ValleyBank;

namespace Eli.Finance.Test.CreditCard.ValleyBank;

[TestFixture]
public class ValleyBankTransactionProcessorTest
{
    private string _sampleFile = null!;
    private readonly ValleyBankTransactionProcessor tp = new();

    [SetUp]
    public void Setup() => _sampleFile = Path.Combine(AppContext.BaseDirectory, "TestData", "2024_valley_bank_transactions.csv");

    [Test]
    public void Read_ShouldParseTransactions()
    {
        var transactions = tp.ParseTransactionsFromFile(_sampleFile);

        Assert.Multiple(() =>
        {
            Assert.That(transactions, Has.Count.EqualTo(7));
            Assert.That(transactions[0].PostDate, Is.EqualTo(new DateOnly(2024, 1, 16)));
            Assert.That(transactions[0].TransDate, Is.EqualTo(new DateOnly(2024, 1, 16)));
            Assert.That(transactions[0].RefNumber, Is.EqualTo("MTC"));
            Assert.That(transactions[0].TransactionDescription, Is.EqualTo("PAYMENT THANK YOU"));
            Assert.That(transactions[0].Amount, Is.EqualTo(-154.90m));
            Assert.That(transactions[0].TransactionType, Is.EqualTo(ValleyBankTransactionType.Payment));
            Assert.That(transactions[1].TransactionDescription, Is.EqualTo("TMOBILE*AUTO PAY 123-456-7890 WA"));
            Assert.That(transactions[1].Amount, Is.EqualTo(154.93m));
            Assert.That(transactions[1].TransactionType, Is.EqualTo(ValleyBankTransactionType.Purchase));
            Assert.That(transactions[2].Amount, Is.EqualTo(-154.93m));
            Assert.That(transactions[5].TransDate, Is.EqualTo(new DateOnly(2024, 3, 25)));
            Assert.That(transactions[6].PostDate, Is.EqualTo(new DateOnly(2024, 4, 16)));
            Assert.That(transactions[6].TransactionType, Is.EqualTo(ValleyBankTransactionType.Payment));
        });
    }

    [Test]
    public void Read_ShouldThrowWhenFileNameDoesNotStartWithYear()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "TestData", "valley_bank_transactions.csv");

        var exception = Assert.Throws<ArgumentException>(() => tp.ParseTransactionsFromFile(filePath));

        Assert.That(
            exception!.Message,
            Does.Contain("Valley Bank transaction file name must begin with a four-digit statement year"));
    }
}
