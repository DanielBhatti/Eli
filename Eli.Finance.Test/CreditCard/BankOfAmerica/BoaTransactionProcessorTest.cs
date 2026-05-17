using Eli.Finance.CreditCard.BankOfAmerica;

namespace Eli.Finance.Test.CreditCard.BankOfAmerica;

[TestFixture]
public class AaaTransactionProcessorTest
{
    private string _sampleFile = null!;
    private readonly BoaTransactionProcessor tp = new();

    [SetUp]
    public void Setup() => _sampleFile = Path.Combine(AppContext.BaseDirectory, "TestData", "boa_transactions.csv");

    [Test]
    public void Read_ShouldParseTransactions()
    {
        var transactions = tp.ParseTransactionsFromFile(_sampleFile);

        Assert.Multiple(() =>
        {
            Assert.That(transactions, Has.Count.EqualTo(3));
            Assert.That(transactions[0].Amount, Is.EqualTo(23.27m));
            Assert.That(transactions[0].Description, Is.EqualTo("PIZZA 3325"));
            Assert.That(transactions[1].Amount, Is.EqualTo(3.04m));
            Assert.That(transactions[2].Date, Is.EqualTo(new DateOnly(2017, 9, 11)));
            Assert.That(transactions[2].Location, Is.EqualTo("123-456-7890, NY"));
        });
    }
}