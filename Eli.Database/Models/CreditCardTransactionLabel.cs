using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eli.Database.Models;

[Table("credit_card_transaction_label", Schema = "finance")]
[Index("TransactionId", "LabelId", Name = "credit_card_transaction_label_transaction_id_label_id_key", IsUnique = true)]
public partial class CreditCardTransactionLabel
{
    [Key]
    [Column("credit_card_transaction_label_id")]
    public Guid CreditCardTransactionLabelId { get; set; }

    [Column("transaction_id")]
    public Guid TransactionId { get; set; }

    [Column("label_id")]
    public Guid LabelId { get; set; }

    [ForeignKey("LabelId")]
    [InverseProperty("CreditCardTransactionLabels")]
    public virtual Label Label { get; set; } = null!;

    [ForeignKey("TransactionId")]
    [InverseProperty("CreditCardTransactionLabels")]
    public virtual CreditCardTransaction Transaction { get; set; } = null!;
}
