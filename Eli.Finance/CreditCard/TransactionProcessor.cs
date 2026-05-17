namespace Eli.Finance.CreditCard;

public interface TransactionProcessor<T>
{
    List<T> ParseTransactionsFromFile(string filePath);
}
