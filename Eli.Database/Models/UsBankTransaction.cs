using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eli.Database.Models;

[Table("us_bank_transaction", Schema = "stage_finance")]
public partial class UsBankTransaction
{
    [Key]
    [Column("us_bank_transaction_id")]
    public Guid UsBankTransactionId { get; set; }

    [Column("post_date")]
    public DateOnly PostDate { get; set; }

    [Column("trans_date")]
    public DateOnly TransDate { get; set; }

    [Column("ref_number")]
    public string RefNumber { get; set; } = null!;

    [Column("transaction_description")]
    public string TransactionDescription { get; set; } = null!;

    [Column("amount")]
    [Precision(9, 2)]
    public decimal Amount { get; set; }

    [Column("transaction_type")]
    public string TransactionType { get; set; } = null!;
}
