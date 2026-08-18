using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eli.Database.Models;

[Table("import_file", Schema = "stage_finance")]
[Index(nameof(ContentSha256), Name = "import_file_content_sha256_key", IsUnique = true)]
public class ImportFile
{
    [Key]
    [Column("import_file_id")]
    public Guid ImportFileId { get; set; }

    [Column("content_sha256")]
    [StringLength(64)]
    public string ContentSha256 { get; set; } = null!;

    [Column("source_name")]
    public string SourceName { get; set; } = null!;

    [Column("original_file_name")]
    public string OriginalFileName { get; set; } = null!;

    [Column("credit_card_id")]
    public Guid CreditCardId { get; set; }

    [Column("imported_at")]
    public DateTimeOffset ImportedAt { get; set; }

    [Column("row_count")]
    public int RowCount { get; set; }

    [ForeignKey(nameof(CreditCardId))]
    public CreditCard CreditCard { get; set; } = null!;
}
