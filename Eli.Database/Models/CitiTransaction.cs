using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eli.Database.Models;

[Table("citi_transaction", Schema = "stage_finance")]
public partial class CitiTransaction
{
    [Key]
    [Column("citi_transaction_id")]
    public Guid CitiTransactionId { get; set; }

    [Column("date")]
    public DateOnly Date { get; set; }

    [Column("description")]
    public string Description { get; set; } = null!;

    [Column("debit")]
    [Precision(9, 2)]
    public decimal Debit { get; set; }

    [Column("credit")]
    [Precision(9, 2)]
    public decimal Credit { get; set; }

    [Column("category")]
    public string Category { get; set; } = null!;
}
