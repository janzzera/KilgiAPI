using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Models;

[Table("journal_lines")]
[Index("EntryId", Name = "idx_journal_lines_entry_id")]
[Index("LotId", Name = "idx_journal_lines_lot_id")]
public partial class JournalLine
{
    [Key]
    [Column("line_id")]
    [StringLength(50)]
    public string LineId { get; set; } = null!;

    [Column("entry_id")]
    [StringLength(50)]
    public string EntryId { get; set; } = null!;

    [Column("lot_id")]
    [StringLength(50)]
    public string? LotId { get; set; }

    [Column("account_code")]
    [StringLength(100)]
    public string AccountCode { get; set; } = null!;

    [Column("account_name")]
    [StringLength(255)]
    public string AccountName { get; set; } = null!;

    [Column("line_type")]
    [StringLength(50)]
    public string LineType { get; set; } = null!;

    [Column("amount")]
    public double Amount { get; set; }

    [Column("provider_id")]
    [StringLength(50)]
    public string? ProviderId { get; set; }

    [Column("customer_id")]
    [StringLength(50)]
    public string? CustomerId { get; set; }

    [Column("payment_source")]
    [StringLength(100)]
    public string? PaymentSource { get; set; }

    [Column("memo")]
    public string? Memo { get; set; }

    [ForeignKey("EntryId")]
    [InverseProperty("JournalLines")]
    public virtual JournalEntry Entry { get; set; } = null!;
}
