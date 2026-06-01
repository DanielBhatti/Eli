using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eli.Database.Models;

[Table("credit_card_transaction_category", Schema = "finance")]
[Index("TransactionId", "CategoryId", Name = "credit_card_transaction_category_transaction_id_category_id_key", IsUnique = true)]
public partial class CreditCardTransactionCategory
{
    [Key]
    [Column("credit_card_transaction_category_id")]
    public Guid CreditCardTransactionCategoryId { get; set; }

    [Column("transaction_id")]
    public Guid TransactionId { get; set; }

    [Column("category_id")]
    public Guid CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("CreditCardTransactionCategories")]
    public virtual Category Category { get; set; } = null!;

    [ForeignKey("TransactionId")]
    [InverseProperty("CreditCardTransactionCategories")]
    public virtual CreditCardTransaction Transaction { get; set; } = null!;
}
