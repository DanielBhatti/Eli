using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eli.Database.Models;

[Table("discover_transaction", Schema = "stage_finance")]
public partial class DiscoverTransaction
{
    [Key]
    [Column("discover_transaction_id")]
    public Guid DiscoverTransactionId { get; set; }

    [Column("trans_date")]
    public DateOnly TransDate { get; set; }

    [Column("description")]
    public string Description { get; set; } = null!;

    [Column("amount")]
    [Precision(9, 2)]
    public decimal Amount { get; set; }

    [Column("category")]
    public string Category { get; set; } = null!;
}
