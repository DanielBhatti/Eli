using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eli.Database.Models;

[Table("boa_transaction", Schema = "stage_finance")]
public partial class BoaTransaction
{
    [Key]
    [Column("boa_transaction_id")]
    public Guid BoaTransactionId { get; set; }

    [Column("date")]
    public DateOnly Date { get; set; }

    [Column("description")]
    public string Description { get; set; } = null!;

    [Column("location")]
    public string Location { get; set; } = null!;

    [Column("amount")]
    [Precision(9, 2)]
    public decimal Amount { get; set; }
}
