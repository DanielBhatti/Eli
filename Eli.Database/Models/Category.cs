using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eli.Database.Models;

[Table("category", Schema = "finance")]
[Index("CategoryName", Name = "category_category_name_key", IsUnique = true)]
public partial class Category
{
    [Key]
    [Column("category_id")]
    public Guid CategoryId { get; set; }

    [Column("category_name")]
    public string CategoryName { get; set; } = null!;

    [InverseProperty("Category")]
    public virtual ICollection<CreditCardTransactionCategory> CreditCardTransactionCategories { get; set; } = new List<CreditCardTransactionCategory>();
}
