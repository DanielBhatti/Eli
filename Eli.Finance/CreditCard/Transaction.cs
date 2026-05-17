namespace Eli.Finance.CreditCard;

public record Transaction(DateOnly Date, string Description, string Location, decimal Amount, SpendingCategory SpendingCategory, SpendingSubcategory SpendingSubcategory, SpendingType SpendingType);
