namespace Eli.Finance.CreditCard.Td;

public record TdTransaction(
    DateOnly Date,
    DateOnly PostedDate,
    string ReferenceNumber,
    string ActivityType,
    string Status,
    string CardNumber,
    string MerchantCategory,
    string MerchantName,
    string MerchantCity,
    string MerchantStateProvince,
    string MerchantPostalCode,
    decimal Amount,
    int Rewards,
    TdTransactionType TransactionType);
