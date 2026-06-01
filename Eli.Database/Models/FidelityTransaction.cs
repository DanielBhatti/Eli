using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eli.Database.Models;

[Table("fidelity_transaction", Schema = "stage_finance")]
public partial class FidelityTransaction
{
    [Key]
    [Column("fidelity_transaction_id")]
    public Guid FidelityTransactionId { get; set; }

    [Column("transaction_type")]
    public string TransactionType { get; set; } = null!;

    [Column("category")]
    public string Category { get; set; } = null!;

    [Column("label")]
    public string Label { get; set; } = null!;

    [Column("transaction_date")]
    public DateOnly TransactionDate { get; set; }

    [Column("posted_date")]
    public DateOnly PostedDate { get; set; }

    [Column("merchant")]
    public string Merchant { get; set; } = null!;

    [Column("amount")]
    [Precision(9, 2)]
    public decimal Amount { get; set; }
}
