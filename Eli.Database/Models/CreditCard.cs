using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eli.Database.Models;

[Table("credit_card", Schema = "finance")]
[Index("CreditCardName", Name = "credit_card_credit_card_name_key", IsUnique = true)]
public partial class CreditCard
{
    [Key]
    [Column("credit_card_id")]
    public Guid CreditCardId { get; set; }

    [Column("credit_card_name")]
    public string CreditCardName { get; set; } = null!;

    [InverseProperty("CreditCard")]
    public virtual ICollection<CreditCardTransaction> CreditCardTransactions { get; set; } = new List<CreditCardTransaction>();
}
