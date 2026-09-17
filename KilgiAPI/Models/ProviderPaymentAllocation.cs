using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Models;

[Table("provider_payment_allocations")]
[Index("LotId", Name = "idx_ppa_lot_id")]
[Index("PaymentId", Name = "idx_ppa_payment_id")]
public partial class ProviderPaymentAllocation
{
    [Key]
    [Column("allocation_id")]
    [StringLength(50)]
    public string AllocationId { get; set; } = null!;

    [Column("payment_id")]
    [StringLength(50)]
    public string PaymentId { get; set; } = null!;

    [Column("lot_id")]
    [StringLength(50)]
    public string LotId { get; set; } = null!;

    [Column("amount_applied")]
    public double AmountApplied { get; set; }

    [Column("allocation_order")]
    public int AllocationOrder { get; set; }

    [Column("timestamp")]
    public long Timestamp { get; set; }

    [ForeignKey("LotId")]
    [InverseProperty("ProviderPaymentAllocations")]
    public virtual Lot Lot { get; set; } = null!;

    [ForeignKey("PaymentId")]
    [InverseProperty("ProviderPaymentAllocations")]
    public virtual ProviderPayment Payment { get; set; } = null!;
}
