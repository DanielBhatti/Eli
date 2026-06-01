using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eli.Database.Models;

[Table("td_transaction", Schema = "stage_finance")]
public partial class TdTransaction
{
    [Key]
    [Column("td_transaction_id")]
    public Guid TdTransactionId { get; set; }

    [Column("date")]
    public DateOnly Date { get; set; }

    [Column("posted_date")]
    public DateOnly PostedDate { get; set; }

    [Column("reference_number")]
    public string ReferenceNumber { get; set; } = null!;

    [Column("activity_type")]
    public string ActivityType { get; set; } = null!;

    [Column("status")]
    public string Status { get; set; } = null!;

    [Column("card_number")]
    public string CardNumber { get; set; } = null!;

    [Column("merchant_category")]
    public string MerchantCategory { get; set; } = null!;

    [Column("merchant_name")]
    public string MerchantName { get; set; } = null!;

    [Column("merchant_city")]
    public string MerchantCity { get; set; } = null!;

    [Column("merchant_state_province")]
    public string MerchantStateProvince { get; set; } = null!;

    [Column("merchant_postal_code")]
    public string MerchantPostalCode { get; set; } = null!;

    [Column("amount")]
    [Precision(9, 2)]
    public decimal Amount { get; set; }

    [Column("rewards")]
    public int Rewards { get; set; }

    [Column("transaction_type")]
    public string TransactionType { get; set; } = null!;
}
