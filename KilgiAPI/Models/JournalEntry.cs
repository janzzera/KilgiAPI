using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Models;

[Table("journal_entries")]
[Index("LotId", Name = "idx_journal_entries_lot_id")]
[Index("ReferenceType", "ReferenceId", Name = "idx_journal_entries_ref")]
[Index("UserId", Name = "idx_journal_entries_user_id")]
public partial class JournalEntry
{
    [Key]
    [Column("entry_id")]
    [StringLength(50)]
    public string EntryId { get; set; } = null!;

    [Column("user_id")]
    [StringLength(50)]
    public string UserId { get; set; } = null!;

    [Column("lot_id")]
    [StringLength(50)]
    public string? LotId { get; set; }

    [Column("reference_type")]
    [StringLength(100)]
    public string? ReferenceType { get; set; }

    [Column("reference_id")]
    [StringLength(50)]
    public string? ReferenceId { get; set; }

    [Column("event_type")]
    [StringLength(100)]
    public string EventType { get; set; } = null!;

    [Column("description")]
    [StringLength(500)]
    public string Description { get; set; } = null!;

    [Column("timestamp")]
    public long Timestamp { get; set; }

    [InverseProperty("Entry")]
    public virtual ICollection<JournalLine> JournalLines { get; set; } = new List<JournalLine>();

    [ForeignKey("LotId")]
    [InverseProperty("JournalEntries")]
    public virtual Lot? Lot { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("JournalEntries")]
    public virtual User User { get; set; } = null!;
}
