using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Eli.Database.Models;

[Table("country", Schema = "geo")]
[Index("AsciiCountryName", Name = "country_ascii_country_name_key", IsUnique = true)]
[Index("CountryName", Name = "country_country_name_key", IsUnique = true)]
[Index("IsoCode3", Name = "country_iso_code_3_key", IsUnique = true)]
[Index("IsoCode", Name = "country_iso_code_key", IsUnique = true)]
public partial class Country
{
    [Key]
    [Column("country_id")]
    public Guid CountryId { get; set; }

    [Column("iso_code")]
    [StringLength(2)]
    public string IsoCode { get; set; } = null!;

    [Column("ascii_country_name")]
    public string AsciiCountryName { get; set; } = null!;

    [Column("country_name")]
    public string CountryName { get; set; } = null!;

    [Column("iso_code_3")]
    [StringLength(3)]
    public string IsoCode3 { get; set; } = null!;

    [Column("num_code")]
    public int? NumCode { get; set; }

    [Column("phone_code")]
    public int PhoneCode { get; set; }
}
