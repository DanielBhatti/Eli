namespace Eli.Finance.CreditCard.BankOfAmerica;

public record BoaTransaction(DateOnly Date, string Description, string Location, decimal Amount);