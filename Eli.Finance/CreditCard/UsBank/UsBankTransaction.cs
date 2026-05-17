namespace Eli.Finance.CreditCard.UsBank;

public record UsBankTransaction(
    DateOnly PostDate,
    DateOnly TransDate,
    string RefNumber,
    string TransactionDescription,
    decimal Amount,
    UsBankTransactionType TransactionType);
