using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eli.Database.Models;

[Table("label", Schema = "finance")]
[Index("LabelName", Name = "label_label_name_key", IsUnique = true)]
public partial class Label
{
    [Key]
    [Column("label_id")]
    public Guid LabelId { get; set; }

    [Column("label_name")]
    public string LabelName { get; set; } = null!;

    [InverseProperty("Label")]
    public virtual ICollection<CreditCardTransactionLabel> CreditCardTransactionLabels { get; set; } = new List<CreditCardTransactionLabel>();
}
