namespace Eli.Finance.CreditCard.CapitalOne;

public record CapitalOneTransaction(
    DateOnly TransDate,
    DateOnly? PostDate,
    string Description,
    decimal Amount,
    CapitalOneTransactionType TransactionType);
