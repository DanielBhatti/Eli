using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eli.Database.Models;

[Table("aaa_transaction", Schema = "stage_finance")]
public partial class AaaTransaction
{
    [Key]
    [Column("aaa_transaction_id")]
    public Guid AaaTransactionId { get; set; }

    [Column("trans_date")]
    public DateOnly TransDate { get; set; }

    [Column("description_location")]
    public string DescriptionLocation { get; set; } = null!;

    [Column("amount")]
    [Precision(9, 2)]
    public decimal Amount { get; set; }

    [Column("file_name")]
    public string FileName { get; set; } = null!;

    [Column("is_excluded_from_processing")]
    public bool IsExcludedFromProcessing { get; set; }
}
