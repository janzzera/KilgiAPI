using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Models;

[Table("accounting_periods")]
public partial class AccountingPeriod
{
    [Key]
    [Column("period_id")]
    public long PeriodId { get; set; }

    [Column("period_name")]
    [StringLength(255)]
    public string PeriodName { get; set; } = null!;

    [Column("start_date")]
    public long StartDate { get; set; }

    [Column("end_date")]
    public long EndDate { get; set; }

    [Column("is_closed")]
    public bool IsClosed { get; set; }

    [Column("closed_at")]
    public long? ClosedAt { get; set; }

    [Column("user_id")]
    [StringLength(50)]
    public string? UserId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("AccountingPeriods")]
    public virtual User? User { get; set; }
}
