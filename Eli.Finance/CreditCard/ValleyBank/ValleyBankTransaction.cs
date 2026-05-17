namespace Eli.Finance.CreditCard.ValleyBank;

public record ValleyBankTransaction(
    DateOnly PostDate,
    DateOnly TransDate,
    string RefNumber,
    string TransactionDescription,
    decimal Amount,
    ValleyBankTransactionType TransactionType);
