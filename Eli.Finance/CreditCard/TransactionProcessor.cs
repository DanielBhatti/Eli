namespace Eli.Finance.CreditCard;

public interface TransactionProcessor
{
    List<MinDetailTransaction> ParseMinDetailTransactionsFromFile(string filePath);
}
