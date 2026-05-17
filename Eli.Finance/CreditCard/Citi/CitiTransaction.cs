namespace Eli.Finance.CreditCard.Citi;

public record CitiTransaction(DateOnly Date, string Description, decimal Debit, decimal Credit, string Category);