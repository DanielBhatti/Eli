namespace Eli.Finance.CreditCard.Discover;

public record DiscoverTransaction(DateOnly TransDate, string Description, decimal Amount, string Category);
