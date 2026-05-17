using Eli.Finance.CreditCard.Td;

namespace Eli.Finance.Test.CreditCard.Td;

[TestFixture]
public class TdTransactionProcessorTest
{
    private string _sampleFile = null!;
    private readonly TdTransactionProcessor tp = new();

    [SetUp]
    public void Setup() => _sampleFile = Path.Combine(AppContext.BaseDirectory, "TestData", "td_Transaction History_2022-02-10.csv");

    [Test]
    public void Read_ShouldParseTransactions()
    {
        var transactions = tp.ParseTransactionsFromFile(_sampleFile);

        Assert.Multiple(() =>
        {
            Assert.That(transactions, Has.Count.EqualTo(3));
            Assert.That(transactions[0].Date, Is.EqualTo(new DateOnly(2022, 2, 9)));
            Assert.That(transactions[0].PostedDate, Is.EqualTo(new DateOnly(2022, 2, 10)));
            Assert.That(transactions[0].ReferenceNumber, Is.EqualTo("1"));
            Assert.That(transactions[0].ActivityType, Is.EqualTo("TRANS"));
            Assert.That(transactions[0].Status, Is.EqualTo("Approved"));
            Assert.That(transactions[0].CardNumber, Is.EqualTo("************1234"));
            Assert.That(transactions[0].MerchantCategory, Is.EqualTo("Quick Payment Service-Fast Food Restaurants"));
            Assert.That(transactions[0].MerchantName, Is.EqualTo("DOMINO'S 3534"));
            Assert.That(transactions[0].MerchantCity, Is.EqualTo("123-456-7890"));
            Assert.That(transactions[0].MerchantStateProvince, Is.EqualTo("NY"));
            Assert.That(transactions[0].MerchantPostalCode, Is.EqualTo("11580"));
            Assert.That(transactions[0].Amount, Is.EqualTo(57.53m));
            Assert.That(transactions[0].Rewards, Is.EqualTo(86));
            Assert.That(transactions[0].TransactionType, Is.EqualTo(TdTransactionType.Purchase));
            Assert.That(transactions[1].ReferenceNumber, Is.EqualTo("289473892"));
            Assert.That(transactions[1].MerchantName, Is.EqualTo("PAYMENT RECEIVED -- THANK"));
            Assert.That(transactions[1].MerchantCity, Is.EqualTo("YOU"));
            Assert.That(transactions[1].Amount, Is.EqualTo(-123.45m));
            Assert.That(transactions[1].Rewards, Is.Zero);
            Assert.That(transactions[1].TransactionType, Is.EqualTo(TdTransactionType.Payment));
            Assert.That(transactions[2].ReferenceNumber, Is.EqualTo("2384798234237"));
            Assert.That(transactions[2].MerchantCategory, Is.EqualTo("Eating Places and Restaurants"));
            Assert.That(transactions[2].Rewards, Is.EqualTo(1));
        });
    }
}
