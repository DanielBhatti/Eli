using Eli.Finance.CreditCard.Fidelity;

namespace Eli.Finance.Test.CreditCard.Fidelity;

[TestFixture]
public class FidelityTransactionProcessorTest
{
    private string _sampleFile = null!;
    private readonly FidelityTransactionProcessor tp = new();

    [SetUp]
    public void Setup() => _sampleFile = Path.Combine(AppContext.BaseDirectory, "TestData", "fidelity_transactions.csv");

    [Test]
    public void Read_ShouldParseTransactions()
    {
        var transactions = tp.ParseTransactionsFromFile(_sampleFile);

        Assert.Multiple(() =>
        {
            Assert.That(transactions, Has.Count.EqualTo(4));
            Assert.That(transactions[0].TransactionType, Is.EqualTo(FidelityTransactionType.Purchase));
            Assert.That(transactions[0].Category, Is.EqualTo("Health & fitness"));
            Assert.That(transactions[0].SubCategory, Is.EqualTo("Doctor"));
            Assert.That(transactions[0].TransactionDate, Is.EqualTo(new DateOnly(2025, 6, 7)));
            Assert.That(transactions[0].PostedDate, Is.EqualTo(new DateOnly(2025, 6, 9)));
            Assert.That(transactions[0].Merchant, Is.EqualTo("SUMMIT MEDICAL GROUP"));
            Assert.That(transactions[0].Amount, Is.EqualTo(381.42m));
            Assert.That(transactions[1].Amount, Is.EqualTo(1000.0m));
            Assert.That(transactions[2].Merchant, Is.EqualTo("7-ELEVEN 22413"));
            Assert.That(transactions[3].TransactionType, Is.EqualTo(FidelityTransactionType.Payment));
            Assert.That(transactions[3].Amount, Is.EqualTo(-2572.89m));
        });
    }
}
