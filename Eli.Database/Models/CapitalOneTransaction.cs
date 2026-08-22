using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eli.Database.Models;

[Table("capital_one_transaction", Schema = "stage_finance")]
public partial class CapitalOneTransaction
{
    [Key]
    [Column("capital_one_transaction_id")]
    public Guid CapitalOneTransactionId { get; set; }

    [Column("trans_date")]
    public DateOnly TransDate { get; set; }

    [Column("post_date")]
    public DateOnly? PostDate { get; set; }

    [Column("description")]
    public string Description { get; set; } = null!;

    [Column("amount")]
    [Precision(9, 2)]
    public decimal Amount { get; set; }

    [Column("transaction_type")]
    public string TransactionType { get; set; } = null!;

    [Column("file_name")]
    public string FileName { get; set; } = null!;

    [Column("is_excluded_from_processing")]
    public bool IsExcludedFromProcessing { get; set; }
}
