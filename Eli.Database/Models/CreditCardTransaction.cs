using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eli.Database.Models;

[Table("credit_card_transaction", Schema = "finance")]
public partial class CreditCardTransaction
{
    [Key]
    [Column("credit_card_transaction_id")]
    public Guid CreditCardTransactionId { get; set; }

    [Column("transaction_date")]
    public DateOnly TransactionDate { get; set; }

    [Column("description")]
    public string Description { get; set; } = null!;

    [Column("purchase_channel_id")]
    public Guid PurchaseChannelId { get; set; }

    [Column("country")]
    public string? Country { get; set; }

    [Column("state_or_province")]
    public string? StateOrProvince { get; set; }

    [Column("amount")]
    [Precision(18, 6)]
    public decimal Amount { get; set; }

    [Column("credit_card_id")]
    public Guid CreditCardId { get; set; }

    [Column("source_uuid")]
    public Guid SourceUuid { get; set; }

    [ForeignKey("CreditCardId")]
    [InverseProperty("CreditCardTransactions")]
    public virtual CreditCard CreditCard { get; set; } = null!;

    [InverseProperty("Transaction")]
    public virtual ICollection<CreditCardTransactionCategory> CreditCardTransactionCategories { get; set; } = new List<CreditCardTransactionCategory>();

    [InverseProperty("Transaction")]
    public virtual ICollection<CreditCardTransactionLabel> CreditCardTransactionLabels { get; set; } = new List<CreditCardTransactionLabel>();

    [ForeignKey("PurchaseChannelId")]
    [InverseProperty("CreditCardTransactions")]
    public virtual PurchaseChannel PurchaseChannel { get; set; } = null!;
}
