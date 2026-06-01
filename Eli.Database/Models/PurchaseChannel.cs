using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eli.Database.Models;

[Table("purchase_channel", Schema = "finance")]
[Index("PurchaseChannelName", Name = "purchase_channel_purchase_channel_name_key", IsUnique = true)]
public partial class PurchaseChannel
{
    [Key]
    [Column("purchase_channel_id")]
    public Guid PurchaseChannelId { get; set; }

    [Column("purchase_channel_name")]
    public string PurchaseChannelName { get; set; } = null!;

    [InverseProperty("PurchaseChannel")]
    public virtual ICollection<CreditCardTransaction> CreditCardTransactions { get; set; } = new List<CreditCardTransaction>();
}
