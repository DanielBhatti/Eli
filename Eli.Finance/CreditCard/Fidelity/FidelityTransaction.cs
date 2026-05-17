namespace Eli.Finance.CreditCard.Fidelity;

public record FidelityTransaction(
    FidelityTransactionType TransactionType,
    string Category,
    string SubCategory,
    DateOnly TransactionDate,
    DateOnly PostedDate,
    string Merchant,
    decimal Amount);
