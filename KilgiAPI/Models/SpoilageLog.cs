using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Models;

[Table("spoilage_logs")]
[Index("LotId", Name = "idx_spoilage_logs_lot_id")]
public partial class SpoilageLog
{
    [Key]
    [Column("log_id")]
    [StringLength(50)]
    public string LogId { get; set; } = null!;

    [Column("lot_id")]
    [StringLength(50)]
    public string LotId { get; set; } = null!;

    [Column("kilos_lost")]
    public double KilosLost { get; set; }

    [Column("loss_type")]
    [StringLength(100)]
    public string LossType { get; set; } = null!;

    [Column("timestamp")]
    public long Timestamp { get; set; }

    [ForeignKey("LotId")]
    [InverseProperty("SpoilageLogs")]
    public virtual Lot Lot { get; set; } = null!;
}
